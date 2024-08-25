using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.House;
using HrmsBe.Dto.V1.RoomCategory;
using HrmsBe.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrmsBe.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomCategoriesController : ControllerBase
    {
        private readonly IRoomCategoriesRepo _repo;

        public RoomCategoriesController(IRoomCategoriesRepo repo)
        {
            _repo = repo;
        }

        [HttpPost("CreateUpdateRoomCategory")]
        public async Task<CommonResponseDto> CreateUpdateRoomCategory(CreateOrUpdateRoomCategoriesDto model)
        {
            try
            {
                var data = await _repo.CreateUpdateRoomCategory(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpGet("RoomCategoryLandingPagination")]
        public CommonResponseDto RoomCategoryLandingPagination(string? search, long houseId, int pageNo, int pageSize)
        {
            try
            {
                var data = _repo.RoomCategoryLandingPagination(search, houseId, pageNo, pageSize);
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
        [HttpGet("GetRoomCategoryById")]
        public async Task<CommonResponseDto> GetRoomCategoryById(long id)
        {
            try
            {
                var data = await _repo.GetRoomCategoryById(id);
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
        [HttpDelete("DeleteRoomCategory")]
        public async Task<CommonResponseDto> DeleteRoomCategory(List<long> id, string userId)
        {
            try
            {
                var data = await _repo.DeleteRoomCategory(id, userId);
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
