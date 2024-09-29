using HrmsBe.Dto.V1.Common;
using HrmsBe.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrmsBe.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController(ICommonRepository repo) : ControllerBase
    {
        private readonly ICommonRepository _repo = repo;

        [HttpGet("HouseListByUser")]
        public async Task<CommonResponseDto> HouseListByUser(string userId)
        {
            try
            {
                var data = await _repo.HouseListByUser(userId);
                return new CommonResponseDto()
                {
                    Data = data,
                    Message = string.Empty,
                    StatusCode = 200,
                    Succeed = true
                };
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
