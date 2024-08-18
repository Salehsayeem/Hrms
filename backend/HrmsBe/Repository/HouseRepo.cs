using System.Runtime.InteropServices.JavaScript;
using HrmsBe.Context;
using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.House;
using HrmsBe.IRepositories;
using HrmsBe.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using HrmsBe.Helper;
using HrmsBe.StoredProcedure;
using MongoDB.Driver.Core.Configuration;
using Npgsql;

namespace HrmsBe.Repository
{
    public class HouseRepo : IHouseRepo
    {
        private readonly ApplicationDbContext _context;

        public HouseRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CommonResponseDto> CreateUpdateHouse(CreateOrUpdateHouseDto obj)
        {
            var auditInfo = new AuditDto();
            try
            {
                var response = new CommonResponseDto();

                if (obj.Id == 0)
                {
                    var alreadyExist = await _context.Houses.Where(a => a.Name.ToLower() == obj.Name.ToLower() && a.CreatedBy == CommonHelper.StringToUlidConverter(obj.UserId) && a.IsActive).FirstOrDefaultAsync();
                    if (alreadyExist != null)
                    {
                        response.Message = obj.Name + " Already exists";
                        response.StatusCode = 500;
                        response.Succeed = false;
                        return response;
                    }
                    var data = new House()
                    {
                        Name = obj.Name,
                        Address = obj.Address,
                        Contact = obj.Contact,
                        CreatedBy = CommonHelper.StringToUlidConverter(obj.UserId),
                        IsActive = true,
                    };

                    await _context.Houses.AddAsync(data);
                    auditInfo = new AuditDto()
                    {
                        ControllerName = System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                        Description = $"{obj.Name} house Added",
                        RequestParameters = JsonSerializer.Serialize(obj).ToString(),
                        StatusCode = 200,
                        UserId = data.CreatedBy.ToString()
                    };
                    _context.AuditInfo = auditInfo;
                    await _context.SaveChangesAsync();

                    response.Message = data.Name + " -House has been added!";
                    response.StatusCode = 200;
                    response.Succeed = true;
                    response.Data = data;
                    return response;
                }
                else
                {
                    var data = await _context.Houses.Where(a => a.Id == obj.Id && a.IsActive).FirstOrDefaultAsync();
                    if (data == null)
                    {
                        response.Message = "Problem with updating " + obj.Name;
                        response.StatusCode = 500;
                        response.Succeed = false;
                        return response;
                    }
                    data.Name = obj.Name;
                    data.Address = obj.Address;
                    data.Contact = obj.Contact;
                    data.ModifiedBy = CommonHelper.StringToUlidConverter(obj.UserId);
                    data.IsActive = true;
                    _context.Houses.Update(data);
                    auditInfo = new AuditDto()
                    {
                        ControllerName = System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                        Description = $"{obj.Name} house Updated",
                        RequestParameters = JsonSerializer.Serialize(obj).ToString(),
                        StatusCode = 200,
                        UserId = data.CreatedBy.ToString()
                    };
                    _context.AuditInfo = auditInfo;
                    await _context.SaveChangesAsync();

                    response.Message = data.Name + " - House has been updated!";
                    response.StatusCode = 200;
                    response.Succeed = true;
                    return response;

                }
            }
            catch (Exception ex)
            {
                auditInfo.StatusCode = 500;
                throw new CustomExceptionWithAudit(ex.Message, auditInfo);
            }
        }

        public HousePaginationDto HouseLandingPagination(string? search, string userId, int pageNo, int pageSize)
        {
            try
            {
                using var dbConnection = _context.Database.GetDbConnection();

                var data = PostgresFunctionCalls.GetHousesPagination(dbConnection, search, userId, pageNo, pageSize);
                return new HousePaginationDto()
                {
                    Response = CommonHelper.ConvertDataTableToList<HouseDataDto>(data.dataTable),
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

        public async Task<GetHouseDto> GetHouseById(long id)
        {
            try
            {
                var response = new GetHouseDto();
                var data = await _context.Houses.FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
                if (data == null)
                {
                    return response;
                }

                return new GetHouseDto()
                {
                    Address = data.Address,
                    Contact = data.Contact,
                    Name = data.Name,
                    Id = data.Id
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> DeleteHouse(long id, string userId)
        {
            try
            {
                var data = await _context.Houses.FirstOrDefaultAsync(a => a.Id == id && a.IsActive);
                if (data == null)
                {
                    return new CommonResponseDto()
                    {
                        Message = "No records found",
                        StatusCode = 500,
                        Succeed = false,
                        Data = id
                    };
                }
                data.ModifiedBy = CommonHelper.StringToUlidConverter(userId);
                data.IsActive = true;
                _context.Houses.Update(data);
                var obj = new
                {
                    id = id,
                    userId = userId
                };
                var auditInfo = new AuditDto()
                {
                    ControllerName = System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    Description = $"{data.Name} house Deleted ",
                    RequestParameters = JsonSerializer.Serialize(obj).ToString(),
                    StatusCode = 200,
                    UserId = data.ModifiedBy.ToString() ?? string.Empty
                };
                _context.AuditInfo = auditInfo;
                await _context.SaveChangesAsync();

                return new CommonResponseDto()
                {
                    Message = data.Name + "- House Deleted",
                    StatusCode = 200,
                    Succeed = true
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
