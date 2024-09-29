using HrmsBe.Dto.V1.Common;

namespace HrmsBe.IRepositories
{
    public interface ICommonRepository
    {
        public Task<List<CommonDdlDto>> HouseListByUser(string userId);
    }
}
