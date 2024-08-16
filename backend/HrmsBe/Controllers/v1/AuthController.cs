using HrmsBe.Dto.V1.Auth;
using HrmsBe.Dto.V1.Common;
using HrmsBe.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrmsBe.Controllers.v1
{
    //[Route("api/v{version:apiVersion}/[controller]")]
    //[ApiVersion("1.0")]
     [Route("api/[controller]")]
    [ApiController]
    
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo _repo;

        public AuthController(IAuthRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("Register")]
        public async Task<CommonResponseDto> Register(RegisterDto model)
        {
            try
            {
                var data = await _repo.Register(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpPost("Login")]
        public async Task<CommonResponseDto> Login(LoginDto model)
        {
            try
            {
                var data = await _repo.Login(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<CommonResponseDto> ChangePassword(ChangePasswordDto model)
        {
            try
            {
                var data = await _repo.ChangePassword(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpGet("GetProfile")]
        [Authorize]
        public async Task<CommonResponseDto> GetProfile(string userId)
        {
            try
            {
                var data = await _repo.GetProfile(userId);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpPut("UpdateProfile")]
        [Authorize]
        public async Task<CommonResponseDto> UpdateProfile(UpdateProfileDto model)
        {
            try
            {
                var data = await _repo.UpdateProfile(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
