using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.RenterType;
using HrmsBe.Dto.V1.RoomCategory;
using HrmsBe.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrmsBe.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class RenterTypesController : ControllerBase
    {
        private readonly IRenterTypeRepo _repo;

        public RenterTypesController(IRenterTypeRepo repo)
        {
            _repo = repo;
        }


        [HttpPost("CreateOrUpdateRenterType")]
        public async Task<CommonResponseDto> CreateOrUpdateRenterTypes(CreateOrUpdateRenterTypesDto model)
        {
            try
            {
                var data = await _repo.CreateOrUpdateRenterTypes(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpGet("RenterTypeLandingPagination")]
        public CommonResponseDto RenterTypeLandingPagination(string? search, string userId, int pageNo, int pageSize)
        {
            try
            {
                var data = _repo.RenterTypeLandingPagination(search, userId, pageNo, pageSize);
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
        [HttpGet("GetRenterTypeById")]
        public async Task<CommonResponseDto> GetRenterTypeById(long id)
        {
            try
            {
                var data = await _repo.GetRenterTypeById(id);
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
        [HttpDelete("DeleteRenterTypes")]
        public async Task<CommonResponseDto> DeleteRenterTypes(List<long> id, string userId)
        {
            try
            {
                var data = await _repo.DeleteRenterTypes(id, userId);
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
