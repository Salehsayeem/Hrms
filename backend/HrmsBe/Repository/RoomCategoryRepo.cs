using HrmsBe.Context;
using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.House;
using HrmsBe.Dto.V1.RoomCategory;
using HrmsBe.Helper;
using HrmsBe.IRepositories;
using HrmsBe.Models;
using HrmsBe.StoredProcedure;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HrmsBe.Repository
{
    public class RoomCategoryRepo : IRoomCategoriesRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly MongoDbService _mongoDbService;
        public RoomCategoryRepo(ApplicationDbContext context, MongoDbService mongoDbService)
        {
            _context = context;
            _mongoDbService = mongoDbService;
        }
        public async Task<CommonResponseDto> CreateUpdateRoomCategory(CreateOrUpdateRoomCategoriesDto obj)
        {
            var auditInfo = new AuditDto();
            var response = new CommonResponseDto();
            var addRoomCategories = new List<RoomCategory>();
            var updateRoomCategories = new List<RoomCategory>();
            var alreadyExistsRoomCategoriesName = new List<string>();
            var notUpdatedRoomCategoriesName = new List<string>();
            string methodName = nameof(CreateUpdateRoomCategory);
            try
            {
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

                foreach (var category in obj.Data)
                {
                    if (category.Id == 0)
                    {
                        var alreadyExist = await _context.RoomCategories
                            .Where(rc => rc.Name == category.Name && rc.HouseId == obj.HouseId && rc.IsActive)
                            .FirstOrDefaultAsync();
                        if (alreadyExist != null)
                        {
                            alreadyExistsRoomCategoriesName.Add(alreadyExist.Name);
                            continue;
                        }
                        addRoomCategories.Add(new RoomCategory
                        {
                            Name = category.Name,
                            HouseId = obj.HouseId,
                            IsActive = true,
                            CreatedBy = CommonHelper.StringToUlidConverter(obj.UserId)
                        });
                    }
                    else
                    {
                        var data = await _context.RoomCategories
                            .FirstOrDefaultAsync(rc => rc.Id == category.Id && rc.IsActive);
                        if (data == null)
                        {
                            notUpdatedRoomCategoriesName.Add(category.Name);
                            continue;
                        }
                        data.Name = category.Name;
                        data.HouseId = obj.HouseId;
                        data.ModifiedBy = CommonHelper.StringToUlidConverter(obj.UserId);
                        updateRoomCategories.Add(data);
                    }
                }

                if (addRoomCategories.Count > 0)
                {
                    await _context.RoomCategories.AddRangeAsync(addRoomCategories);
                    var addedNames = string.Join(", ", addRoomCategories.Select(rc => rc.Name));
                    auditInfo = _mongoDbService.CreateAuditInfo($"{addedNames} Room Categories added", methodName, obj, 200, obj.UserId);
                    response.Message = $"{addedNames} Room Categories added to house";
                    response.Data = addRoomCategories;
                }

                if (updateRoomCategories.Count > 0)
                {
                    _context.RoomCategories.UpdateRange(updateRoomCategories);
                    var updatedNames = string.Join(", ", updateRoomCategories.Select(rc => rc.Name));
                    auditInfo = _mongoDbService.CreateAuditInfo($"{updatedNames} Room Categories added",methodName, obj, 200, obj.UserId);
                    response.Message = $"{updatedNames} Room Categories updated";
                }

                if (alreadyExistsRoomCategoriesName.Any() || notUpdatedRoomCategoriesName.Any())
                {
                    response.Message += $"{string.Join(", ", alreadyExistsRoomCategoriesName.Concat(notUpdatedRoomCategoriesName))} were not updated.";
                }
                _context.AuditInfo = auditInfo;
                await _context.SaveChangesAsync();

                response.StatusCode = 200;
                response.Succeed = true;
                return response;
            }
            catch (Exception ex)
            {
                auditInfo.StatusCode = 500;
                throw new CustomExceptionWithAudit(ex.Message, auditInfo);
            }
        }

        public RoomCategoryPaginationDto RoomCategoryLandingPagination(string? search, long houseId, int pageNo, int pageSize)
        {
            try
            {
                using var dbConnection = _context.Database.GetDbConnection();

                var data = PostgresFunctionCalls.GetRoomCategoriesPagination(dbConnection, search, houseId, pageNo, pageSize);
                return new RoomCategoryPaginationDto()
                {
                    Response = CommonHelper.ConvertDataTableToList<RoomCategoryDataDto>(data.dataTable),
                    CurrentPage = pageNo,
                    PageSize = pageSize,
                    TotalCount = data.TotalCount
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<GetRoomCategoryDto> GetRoomCategoryById(long id)
        {
            try
            {

                var data = await _context.RoomCategories.FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
                if (data != null)
                {
                    return new GetRoomCategoryDto()
                    {
                        Id = data.Id,
                        Name = data.Name
                    };
                }

                return null;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> DeleteRoomCategory(List<long> id,  string userId)
        {
            string methodName = nameof(DeleteRoomCategory);
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var data = await _context.RoomCategories
                    .Where(a => id.Contains(a.Id) && a.IsActive)
                    .ToListAsync();
                if (data.Count ==0  || !data.Any())
                {
                    return new CommonResponseDto()
                    {
                        Message = "No records found",
                        StatusCode = 500,
                        Succeed = false,
                        Data = id
                    };
                }
                foreach (var category in data)
                {
                    category.ModifiedBy = CommonHelper.StringToUlidConverter(userId);
                    category.IsActive = false; 
                }
                _context.RoomCategories.UpdateRange(data);

                var auditInfo = new AuditDto
                {
                    ControllerName = $"{methodName}",
                    Description = $"Deleted room categories: {string.Join(", ", data.Select(d => d.Name))}",
                    RequestParameters = JsonSerializer.Serialize(new { id, userId }),
                    StatusCode = 200,
                    UserId = CommonHelper.StringToUlidConverter(userId).ToString()
                };
                _context.AuditInfo = auditInfo;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CommonResponseDto
                {
                    Message = $"{data.Count} Room Categories deleted successfully",
                    StatusCode = 200,
                    Succeed = true,
                    Data = id
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new CustomExceptionWithAudit(e.Message, new AuditDto
                {
                    ControllerName = $"{methodName}",
                    Description = $"Failed to delete room categories: {string.Join(", ", id)}",
                    RequestParameters = JsonSerializer.Serialize(new { id, userId }),
                    StatusCode = 500,
                    UserId = CommonHelper.StringToUlidConverter(userId).ToString()
                });
            }
        }
    }
}
