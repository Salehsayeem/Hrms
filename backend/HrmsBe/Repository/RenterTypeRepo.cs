using HrmsBe.Context;
using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.RenterType;
using HrmsBe.Dto.V1.RoomCategory;
using HrmsBe.Helper;
using HrmsBe.IRepositories;
using HrmsBe.Models;
using HrmsBe.StoredProcedure;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace HrmsBe.Repository
{
    public class RenterTypeRepo : IRenterTypeRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly MongoDbService _mongoDbService;
        public RenterTypeRepo(ApplicationDbContext context, MongoDbService mongoDbService)
        {
            _context = context;
            _mongoDbService = mongoDbService;
        }

        public async Task<CommonResponseDto> CreateOrUpdateRenterTypes(CreateOrUpdateRenterTypesDto obj)
        {
            var auditInfo = new AuditDto();
            var response = new CommonResponseDto();
            var addRenterTypes = new List<RenterType>();
            var updateRenterTypes = new List<RenterType>();
            var alreadyExistsRenterTypesName = new List<string>();
            var notUpdatedRenterTypesName = new List<string>();
            string methodName = nameof(CreateOrUpdateRenterTypes);
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

              

                foreach (var data in obj.Data)
                {
                    if (data.Id == 0)
                    {
                        var alreadyExist = await _context.RenterTypes
                            .Where(rc => rc.Name == data.Name && rc.CreatedBy == CommonHelper.StringToUlidConverter(obj.UserId)  && rc.IsActive)
                            .FirstOrDefaultAsync();
                        if (alreadyExist != null)
                        {
                            alreadyExistsRenterTypesName.Add(alreadyExist.Name);
                            continue;
                        }
                        addRenterTypes.Add(new RenterType()
                        {
                            Name = data.Name,
                            IsActive = true,
                            CreatedBy = CommonHelper.StringToUlidConverter(obj.UserId)
                        });
                    }
                    else
                    {
                        var updateData = await _context.RenterTypes
                            .FirstOrDefaultAsync(rc => rc.Id == data.Id && rc.IsActive);
                        if (updateData == null)
                        {
                            notUpdatedRenterTypesName.Add(data.Name);
                            continue;
                        }
                        updateData.Name = data.Name;
                        updateData.ModifiedBy = CommonHelper.StringToUlidConverter(obj.UserId);
                        updateRenterTypes.Add(updateData);
                    }
                }

                if (addRenterTypes.Count > 0)
                {
                    await _context.RenterTypes.AddRangeAsync(addRenterTypes);
                    var addedNames = string.Join(", ", addRenterTypes.Select(rc => rc.Name));
                    auditInfo = _mongoDbService.CreateAuditInfo($"{addedNames} Renter Types added", methodName, obj, 200, obj.UserId);
                    response.Message = $"{addedNames} Renter Types added to house";
                    response.Data = addRenterTypes;
                }

                if (updateRenterTypes.Count > 0)
                {
                    _context.RenterTypes.UpdateRange(updateRenterTypes);
                    var updatedNames = string.Join(", ", updateRenterTypes.Select(rc => rc.Name));
                    auditInfo = _mongoDbService.CreateAuditInfo($"{updatedNames} Renter Types updated", methodName, obj, 200, obj.UserId);
                    response.Message = $"{updatedNames} Renter Types updated";
                }

                if (alreadyExistsRenterTypesName.Any() || notUpdatedRenterTypesName.Any())
                {
                    response.Message += $"{string.Join(", ", alreadyExistsRenterTypesName.Concat(notUpdatedRenterTypesName))} were not updated.";
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

        public RenterTypePaginationDto RenterTypeLandingPagination(string? search, string userId, int pageNo, int pageSize)
        {
            try
            {
                using var dbConnection = _context.Database.GetDbConnection();

                var data = PostgresFunctionCalls.GetRenterTypesPagination(dbConnection, search, userId, pageNo, pageSize);
                return new RenterTypePaginationDto()
                {
                    Response = CommonHelper.ConvertDataTableToList<RenterTypeDataDto>(data.dataTable),
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

        public async Task<GetRenterTypeDto> GetRenterTypeById(long id)
        {
            try
            {

                var data = await _context.RenterTypes.FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
                if (data != null)
                {
                    return new GetRenterTypeDto()
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

        public async Task<CommonResponseDto> DeleteRenterTypes(List<long> id, string userId)
        {
            string methodName = nameof(DeleteRenterTypes);
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var data = await _context.RenterTypes
                    .Where(a => id.Contains(a.Id) && a.IsActive)
                    .ToListAsync();
                if (data.Count == 0 || !data.Any())
                {
                    return new CommonResponseDto()
                    {
                        Message = "No records found",
                        StatusCode = 500,
                        Succeed = false,
                        Data = id
                    };
                }
                foreach (var item in data)
                {
                    item.ModifiedBy = CommonHelper.StringToUlidConverter(userId);
                    item.IsActive = false;
                }
                _context.RenterTypes.UpdateRange(data);

                var auditInfo = new AuditDto
                {
                    ControllerName = $"{methodName}",
                    Description = $"Deleted Renter Types: {string.Join(", ", data.Select(d => d.Name))}",
                    RequestParameters = JsonSerializer.Serialize(new { id, userId }),
                    StatusCode = 200,
                    UserId = CommonHelper.StringToUlidConverter(userId).ToString()
                };
                _context.AuditInfo = auditInfo;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new CommonResponseDto
                {
                    Message = $"{data.Count} Renter Types deleted successfully",
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
                    Description = $"Failed to delete Renter Types: {string.Join(", ", id)}",
                    RequestParameters = JsonSerializer.Serialize(new { id, userId }),
                    StatusCode = 500,
                    UserId = CommonHelper.StringToUlidConverter(userId).ToString()
                });
            }
        }
    }
}
