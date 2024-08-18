using HrmsBe.Dto.V1.Auth;
using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.House;
using HrmsBe.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrmsBe.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class HousesController : ControllerBase
    {
        private readonly IHouseRepo _repo;

        public HousesController(IHouseRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("CreateOrUpdateHouse")]
        public async Task<CommonResponseDto> CreateOrUpdateHouse(CreateOrUpdateHouseDto model)
        {
            try
            {
                var data = await _repo.CreateUpdateHouse(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpGet("HouseLandingPagination")]
        public CommonResponseDto HouseLandingPagination(string? search, string userId, int pageNo, int pageSize)
        {
            try
            {
                var data =  _repo.HouseLandingPagination(search,userId, pageNo, pageSize);
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
        [HttpGet("GetHouseById")]
        public async Task<CommonResponseDto> GetHouseById(long id)
        {
            try
            {
                var data = await _repo.GetHouseById(id);
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
        [HttpDelete("DeleteHouse")]
        public async Task<CommonResponseDto> DeleteHouse(long id,string userId)
        {
            try
            {
                var data = await _repo.DeleteHouse(id, userId);
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
