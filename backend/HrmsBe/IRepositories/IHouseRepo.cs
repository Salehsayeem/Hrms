using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.House;

namespace HrmsBe.IRepositories
{
    public interface IHouseRepo
    {
        public Task<CommonResponseDto> CreateUpdateHouse(CreateOrUpdateHouseDto obj);

        public HousePaginationDto HouseLandingPagination(string? search, string userId, int pageNo, int pageSize);

        public Task<GetHouseDto> GetHouseById(long id);

        public Task<CommonResponseDto> DeleteHouse(long id, string userId);
    }
}
