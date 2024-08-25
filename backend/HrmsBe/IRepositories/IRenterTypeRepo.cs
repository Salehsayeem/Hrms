using HrmsBe.Dto.V1.Common;
using HrmsBe.Dto.V1.RenterType;
using HrmsBe.Dto.V1.RoomCategory;

namespace HrmsBe.IRepositories
{
    public interface IRenterTypeRepo
    {
        public Task<CommonResponseDto> CreateOrUpdateRenterTypes(CreateOrUpdateRenterTypesDto obj);

        public RenterTypePaginationDto RenterTypeLandingPagination(string? search, string userId, int pageNo, int pageSize);

        public Task<GetRenterTypeDto> GetRenterTypeById(long id);

        public Task<CommonResponseDto> DeleteRenterTypes(List<long> id, string userId);
    }
}
