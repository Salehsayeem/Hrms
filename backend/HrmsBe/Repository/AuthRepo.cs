using System.Text.Json;
using HrmsBe.Context;
using HrmsBe.Dto.V1.Auth;
using HrmsBe.Dto.V1.Common;
using HrmsBe.Helper;
using HrmsBe.IRepositories;
using HrmsBe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HrmsBe.Repository
{
    public class AuthRepo : IAuthRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthHelper _authHelper;

        public AuthRepo(ApplicationDbContext context, AuthHelper authHelper)
        {
            _context = context;
            _authHelper = authHelper;
        }

        public async Task<CommonResponseDto> Register(RegisterDto model)
        {
            var auditInfo = new AuditDto();
            try
            {
                var response = new CommonResponseDto();
                if (await _context.Users.AnyAsync(u => u.Phone == model.Phone))
                {
                    response.Message = "Phone is already in use.";
                    response.StatusCode = 500;
                    response.Succeed = false;
                    return response;
                }
                var user = new User
                {
                    Id = Ulid.NewUlid(),
                    Name = model.Name,
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = _authHelper.HashPassword(model.Password),
                    IsActive = true
                };
                user.CreatedBy = user.Id;
                await _context.Users.AddAsync(user);
                auditInfo = new AuditDto()
                {
                    ControllerName = System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    Description = $"{user.Phone} Added",
                    RequestParameters = JsonSerializer.Serialize(model).ToString(),
                    StatusCode = 200,
                    UserId = user.Id.ToString()
                };
                _context.AuditInfo = auditInfo;
                await _context.SaveChangesAsync();
                var token = _authHelper.GenerateJwtToken(user);

                response.Message = "User Registered Successfully";
                response.StatusCode = 200;
                response.Succeed = true;
                response.Data = token;
                return response;
            }
            catch (Exception e)
            {
                auditInfo.StatusCode = 500;
                throw new CustomExceptionWithAudit(e.Message, auditInfo);
            }
        }

        public async Task<CommonResponseDto> Login(LoginDto model)
        {
            var auditInfo = new AuditDto();
            try
            {
                var response = new CommonResponseDto();
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Phone == model.Phone && u.IsActive);
                if (user == null || !_authHelper.VerifyPassword(model.Password, user.Password))
                {
                    response.Message = "Invalid phone or password.";
                    response.StatusCode = 500;
                    response.Succeed = false;
                    return response;
                }
                var token = _authHelper.GenerateJwtToken(user);
                response.Message = "Logged in Successfully";
                response.StatusCode = 200;
                response.Succeed = true;
                response.Data = token;
                return response;
            }
            catch (Exception e)
            {
                auditInfo.StatusCode = 500;
                throw new CustomExceptionWithAudit(e.Message, auditInfo);
            }
        }

        public async Task<CommonResponseDto> ChangePassword(ChangePasswordDto model)
        {
            var auditInfo = new AuditDto();
            try
            {
                var response = new CommonResponseDto();
                Ulid userId = CommonHelper.StringToUlidConverter(model.UserId);
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == userId && m.IsActive);
                if (user == null)
                {
                    response.Message = "User not found.";
                    response.StatusCode = 404;
                    response.Succeed = false;
                    return response;
                }
                if (!_authHelper.VerifyPassword(model.CurrentPassword, user.Password))
                {
                    response.Message = "Incorrect password.";
                    response.StatusCode = 400;
                    response.Succeed = false;
                    return response;
                }
                user.Password = _authHelper.HashPassword(model.NewPassword);
                user.ModifiedBy = user.Id;
                user.ModifiedAt = CommonHelper.CurrentDateTime();

                auditInfo = new AuditDto()
                {
                    ControllerName = System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                    Description = $"{user.Phone} has changed password",
                    RequestParameters = JsonSerializer.Serialize(model).ToString(),
                    StatusCode = 200,
                    UserId = user.Id.ToString()
                };
                _context.AuditInfo = auditInfo;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                response.Message = "Password changed successfully.";
                response.StatusCode = 200;
                response.Succeed = true;
                return response;
            }
            catch (Exception e)
            {
                auditInfo.StatusCode = 500;
                throw new CustomExceptionWithAudit(e.Message, auditInfo);
            }
        }

        public async Task<CommonResponseDto> GetProfile(string userId)
        {
            try
            {
                var response = new CommonResponseDto();
                Ulid id = CommonHelper.StringToUlidConverter(userId);
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
                if (user == null)
                {
                    response.Message = "User not found.";
                    response.StatusCode = 404;
                    response.Succeed = false;
                    return response;
                }
                else
                {
                    var profile = new UserProfileDto
                    {
                        Phone = user.Phone,
                        Name = user.Name,
                        Email = user.Email
                    };

                    response.Message = "";
                    response.Data = profile;
                    response.StatusCode = 200;
                    response.Succeed = true;
                    return response;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> UpdateProfile(UpdateProfileDto model)
        {
            var auditInfo = new AuditDto();
            try
            {
                var response = new CommonResponseDto();
                Ulid id = CommonHelper.StringToUlidConverter(model.UserId);
                var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id && m.IsActive);
                if (user == null)
                {
                    response.Message = "User not found.";
                    response.StatusCode = 404;
                    response.Succeed = false;
                    return response;
                }
                else
                {
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.Phone = model.Phone;
                    user.ModifiedBy = user.Id;
                    user.ModifiedAt = CommonHelper.CurrentDateTime();
                    _context.Users.Update(user);
                    auditInfo = new AuditDto()
                    {
                        ControllerName = System.Reflection.MethodBase.GetCurrentMethod()!.Name,
                        Description = $"{user.Phone} has changed password",
                        RequestParameters = JsonSerializer.Serialize(model).ToString(),
                        StatusCode = 200,
                        UserId = user.Id.ToString()
                    };
                    _context.AuditInfo = auditInfo;
                   
                    await _context.SaveChangesAsync();

                    response.Message = "Profile updated successfully.";
                    response.Data = user;
                    response.StatusCode = 200;
                    response.Succeed = true;
                    return response;
                }
            }
            catch (Exception e)
            {
                auditInfo.StatusCode = 500;
                throw new CustomExceptionWithAudit(e.Message, auditInfo);
            }
        }
    }
}
