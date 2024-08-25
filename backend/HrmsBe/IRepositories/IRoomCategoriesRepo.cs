using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.RoomCategory;

namespace HrmsBe.IRepositories
{
    public interface IRoomCategoriesRepo
    {
        public Task<CommonResponseDto> CreateUpdateRoomCategory(CreateOrUpdateRoomCategoriesDto obj);

        public RoomCategoryPaginationDto RoomCategoryLandingPagination(string? search, long houseId, int pageNo, int pageSize);

        public Task<GetRoomCategoryDto> GetRoomCategoryById(long id);

        public Task<CommonResponseDto> DeleteRoomCategory(List<long> id, string userId);
    }
}
