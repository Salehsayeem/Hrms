using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.RenterType;
using HrmsBe.Dto.V1.Room;
using HrmsBe.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrmsBe.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController(IRoomRepo repo) : ControllerBase
    {
        [HttpPost("CreateRoom")]
        public async Task<CommonResponseDto> CreateRoom(CreateRoomDto model)
        {
            try
            {
                var data = await repo.CreateRoom(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpPut("UpdateRoom")]
        public async Task<CommonResponseDto> UpdateRoom(UpdateRoomDto model)
        {
            try
            {
                if (model.BasePrice < 0)
                {
                    return new CommonResponseDto()
                    {
                        StatusCode = 500,
                        Succeed = false,
                        Message = "Base Price must be positive."
                    };
                }

                if (model.BillGenerationDate is < 1 or > 30)
                {
                    return new CommonResponseDto()
                    {
                        StatusCode = 400,
                        Succeed = false,
                        Message = "Date must be in between 1 and 30."
                    };
                }
                var data = await repo.UpdateRoom(model);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpGet("GetRoom")]
        public async Task<CommonResponseDto> GetRoom(long roomId)
        {
            try
            {
                var data = await repo.GetRoom(roomId);
                return data;
            }
            catch (Exception ex)
            {
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        [HttpGet("RoomsPagination")]
        public async Task<CommonResponseDto> RoomsPagination(string? search, long houseId, long roomCategoryId, string userId, int pageNumber, int pageSize)
        {
            try
            {
                var data = await repo.RoomsPagination(search, houseId, roomCategoryId, userId, pageNumber, pageSize);
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
