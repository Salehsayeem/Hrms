using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.RenterType;
using HrmsBe.Dto.V1.Room;

namespace HrmsBe.IRepositories
{
    public interface IRoomRepo
    {
        public Task<CommonResponseDto> CreateRoom(CreateRoomDto obj);
        public Task<CommonResponseDto> UpdateRoom(UpdateRoomDto obj);
        public Task<CommonResponseDto> GetRoom(long roomId);
        public Task<CommonResponseDto> DeleteRoomAndDetails(long roomId,List<long> roomDetailsId, string userId);
        public Task<CommonResponseDto> RoomsPagination(string? search, long houseId, long roomCategoryId, string userId, int pageNumber, int pageSize);

    }
}
