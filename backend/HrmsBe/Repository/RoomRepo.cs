using HrmsBe.Context;
using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.RenterType;
using HrmsBe.Dto.V1.Room;
using HrmsBe.Helper;
using HrmsBe.IRepositories;
using HrmsBe.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using HrmsBe.StoredProcedure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.Common;

namespace HrmsBe.Repository
{
    public class RoomRepo : IRoomRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly MongoDbService _mongoDbService;
        public RoomRepo(ApplicationDbContext context, MongoDbService mongoDbService)
        {
            _context = context;
            _mongoDbService = mongoDbService;
        }
        public async Task<CommonResponseDto> CreateRoom(CreateRoomDto obj)
        {
            string methodName = nameof(CreateRoom);
            var auditInfo = new AuditDto();
            var response = new CommonResponseDto();
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                #region User And House Checking

                var userNotFound = await _context.Users
                    .FirstOrDefaultAsync(a => a.Id == CommonHelper.StringToUlidConverter(obj.UserId) && a.IsActive);
                if (userNotFound == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "Please log in again!",
                        StatusCode = 500,
                        Succeed = false
                    };
                }

                var houseNotFound = await _context.Houses
                    .FirstOrDefaultAsync(a => a.Id == obj.HouseId && a.IsActive);
                if (houseNotFound == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "This house doesn't exist!",
                        StatusCode = 500,
                        Succeed = false
                    };
                }


                #endregion

                var alreadyExist = await _context.Rooms
                    .Where(r => r.Name == obj.Name && r.HouseId == obj.HouseId && r.IsActive)
                    .FirstOrDefaultAsync();
                if (alreadyExist != null)
                {
                    response.Message = $"{obj.Name} room already exists in this house";
                    response.StatusCode = 500;
                    response.Succeed = false;
                }
                var newRoom = new Room()
                {
                    BasePrice = obj.BasePrice,
                    Name = obj.Name,
                    HouseId = obj.HouseId,
                    RoomCategoryId = obj.RoomCategoryId,
                    BillGenerationDate = obj.BillGenerationDate,
                    IsRented = obj.IsRented,
                    CreatedBy = CommonHelper.StringToUlidConverter(obj.UserId),
                    IsActive = true
                };
                await _context.Rooms.AddAsync(newRoom);
                await _context.SaveChangesAsync();

                var roomDetails = obj.Details.Select(detail => new RoomDetails
                {
                    RoomId = newRoom.Id,
                    BillType = detail.BillType,
                    BillOptions = detail.BillOptions,
                    UnitPrice = detail.UnitPrice,
                    NoOfUnits = detail.NoOfUnits,
                    IsRecurring = detail.IsRecurring,
                }).ToList();

                await _context.RoomDetails.AddRangeAsync(roomDetails);

                auditInfo = _mongoDbService.CreateAuditInfo($"{newRoom.Name} Room Added", methodName, obj, 200, obj.UserId);
                _context.AuditInfo = auditInfo;
                await _context.SaveChangesAsync();


                await transaction.CommitAsync();

                return new CommonResponseDto
                {
                    Message = "Room created successfully",
                    StatusCode = 200,
                    Succeed = true,
                    Data = new { newRoom, roomDetails }
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                auditInfo.StatusCode = 500;
                throw new CustomExceptionWithAudit(e.Message, auditInfo);
            }
        }

        public async Task<CommonResponseDto> UpdateRoom(UpdateRoomDto obj)
        {
            string methodName = nameof(UpdateRoom);
            var auditInfo = new AuditDto();
            var addRoomDetails = new List<RoomDetails>();
            var updateRoomDetails = new List<RoomDetails>();
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                #region User And House Checking

                var userNotFound = await _context.Users
                    .FirstOrDefaultAsync(a => a.Id == CommonHelper.StringToUlidConverter(obj.UserId) && a.IsActive);
                if (userNotFound == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "Please log in again!",
                        StatusCode = 500,
                        Succeed = false
                    };
                }

                var houseNotFound = await _context.Houses
                    .FirstOrDefaultAsync(a => a.Id == obj.HouseId && a.IsActive);
                if (houseNotFound == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "This house doesn't exist!",
                        StatusCode = 500,
                        Succeed = false
                    };
                }
                #endregion

                var room = await _context.Rooms.FirstOrDefaultAsync(a => a.Id == obj.Id && a.IsActive);
                if (room == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "This Room doesn't exist!",
                        StatusCode = 500,
                        Succeed = false
                    };
                }

                room.BasePrice = obj.BasePrice;
                room.Name = obj.Name;
                room.HouseId = obj.HouseId;
                room.RoomCategoryId = obj.RoomCategoryId;
                room.BillGenerationDate = obj.BillGenerationDate;
                room.IsRented = obj.IsRented;
                room.ModifiedBy = CommonHelper.StringToUlidConverter(obj.UserId);
                room.ModifiedAt = CommonHelper.CurrentDateTime();
                _context.Rooms.Update(room);

                foreach (var item in obj.Details)
                {
                    if (item.Id == 0)
                    {
                        var newRoomDetails = new RoomDetails()
                        {
                            RoomId = room.Id,
                            BillType = item.BillType,
                            BillOptions = item.BillOptions,
                            UnitPrice = item.UnitPrice,
                            NoOfUnits = item.NoOfUnits,
                            IsRecurring = item.IsRecurring,
                        };
                        addRoomDetails.Add(newRoomDetails);
                    }
                    else
                    {
                        var roomDetails = await _context.RoomDetails.FirstOrDefaultAsync(a => a.Id == item.Id);
                        if (roomDetails == null) continue;

                        roomDetails.BillType = item.BillType;
                        roomDetails.BillOptions = item.BillOptions;
                        roomDetails.UnitPrice = item.UnitPrice;
                        roomDetails.NoOfUnits = item.NoOfUnits;
                        roomDetails.IsRecurring = item.IsRecurring;
                        updateRoomDetails.Add(roomDetails);
                    }
                }

                if (addRoomDetails.Count > 0)
                {
                    await _context.AddRangeAsync(addRoomDetails);
                }
                if (updateRoomDetails.Count > 0)
                {
                     _context.UpdateRange(updateRoomDetails);
                }
                auditInfo = _mongoDbService.CreateAuditInfo($"Room Updated Successfully", methodName, obj, 200, obj.UserId);
                _context.AuditInfo = auditInfo;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new CommonResponseDto
                {
                    Message = "Room Updated Successfully",
                    StatusCode = 200,
                    Succeed = true,
                    Data = null
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                auditInfo.StatusCode = 500;
                throw new CustomExceptionWithAudit(e.Message, auditInfo);
            }
        }

        public async Task<CommonResponseDto> GetRoom(long roomId)
        {
            try
            {
                var roomData = new GetRoomDto();
                var data = await _context.Rooms.FirstOrDefaultAsync(a => a.Id == roomId && a.IsActive);
                var details = await _context.RoomDetails.Where(a => a.RoomId == roomId).ToListAsync();
                
                
                if (data != null)
                {
                    roomData.Id = data.Id;
                    roomData.Name = data.Name;
                    roomData.BasePrice = data.BasePrice;
                    roomData.BillGenerationDate = data.BillGenerationDate;
                    roomData.HouseId = data.HouseId;
                    roomData.RoomCategoryId = data.RoomCategoryId;
                    roomData.IsRented = data.IsRented;
                    roomData.Details = details.Select(item => new GetRoomDetailsDto
                    {
                        Id = item.Id,
                        BillType = item.BillType,
                        BillOptions = item.BillOptions,
                        UnitPrice = item.UnitPrice,
                        NoOfUnits = item.NoOfUnits,
                        IsRecurring = item.IsRecurring
                    }).ToList();
                }

                return new CommonResponseDto()
                {
                    Message = "",
                    StatusCode = 200,
                    Succeed = true,
                    Data = roomData
                };

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> DeleteRoomAndDetails(long roomId, List<long> roomDetailsId, string userId)
        {
            string methodName = nameof(DeleteRoomAndDetails);
            try
            {
                
                if (roomId != 0) // delete room along with room details 
                {
                    var room = await _context.Rooms.FirstOrDefaultAsync(a=>a.Id == roomId && a.IsActive);
                    if (room != null)
                    {
                        room.IsActive = false;
                        room.ModifiedBy = CommonHelper.StringToUlidConverter(userId);
                        room.ModifiedAt = CommonHelper.CurrentDateTime();
                        _context.Rooms.Update(room);
                        var getRoomDetails = await _context.RoomDetails.Where(a => a.RoomId == roomId).ToListAsync();
                        if (getRoomDetails.Count > 0)
                        {
                            _context.RoomDetails.RemoveRange(getRoomDetails);
                        }
                    }
                }
                else if(roomDetailsId.Count > 0) //delete only room details
                {
                    var roomDetailsToDelete = await _context.RoomDetails
                        .Where(a => roomDetailsId.Contains(a.Id))
                        .ToListAsync();

                    if (roomDetailsToDelete.Count > 0)
                    {
                        _context.RoomDetails.RemoveRange(roomDetailsToDelete);
                       
                    }
                }
                var auditInfo = new AuditDto
                {
                    ControllerName = $"{methodName}",
                    Description = $"Deleted Rooms",
                    RequestParameters = JsonSerializer.Serialize(new { roomId, roomDetailsId, userId }),
                    StatusCode = 200,
                    UserId = CommonHelper.StringToUlidConverter(userId).ToString()
                };
                _context.AuditInfo = auditInfo;
                await _context.SaveChangesAsync();
                return new CommonResponseDto
                {
                    StatusCode = 200,
                    Succeed = true,
                    Message = "Deleted successfully."
                };
            }
            catch (Exception e)
            {
                throw new CustomExceptionWithAudit(e.Message, new AuditDto
                {
                    ControllerName = $"{methodName}",
                    Description = e.InnerException!.ToString(),
                    RequestParameters = JsonSerializer.Serialize(new { roomId, roomDetailsId, userId }),
                    StatusCode = 500,
                    UserId = CommonHelper.StringToUlidConverter(userId).ToString()
                });
            }

        }

        public async Task<CommonResponseDto> RoomsPagination( string? search, long houseId, long roomCategoryId, string userId, int pageNumber, int pageSize)
        {
            try
            {
                await using var dbConnection = _context.Database.GetDbConnection();

                var data = PostgresFunctionCalls.GetRoomPagination(dbConnection, search, houseId, roomCategoryId,userId, pageNumber, pageSize);
                var room = new RoomPaginationDto
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalCount = data.TotalCount,
                    Response = CommonHelper.ConvertDataTableToList<RoomDataDto>(data.dataTable)
                };
                return new CommonResponseDto()
                {
                    Message = "",
                    Data = room,
                    StatusCode = 200,
                    Succeed = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
