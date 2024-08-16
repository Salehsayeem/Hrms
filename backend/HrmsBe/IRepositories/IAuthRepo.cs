using HrmsBe.Dto.V1.Auth;
using HrmsBe.Dto.V1.Common;
using Microsoft.AspNetCore.Mvc;

namespace HrmsBe.IRepositories
{
    public interface IAuthRepo
    {
        public Task<CommonResponseDto> Register([FromBody] RegisterDto model);
        public Task<CommonResponseDto> Login([FromBody] LoginDto model);
        public Task<CommonResponseDto> ChangePassword(ChangePasswordDto model);
        public  Task<CommonResponseDto> GetProfile(string userId);
        public Task<CommonResponseDto> UpdateProfile(UpdateProfileDto model);

    }
}
