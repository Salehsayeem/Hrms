using HrmsBe.Context;
using HrmsBe.Dto.V1.Common;
using HrmsBe.Helper;
using HrmsBe.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace HrmsBe.Repository
{
    public class CommonRepository:ICommonRepository
    {
        private readonly ApplicationDbContext _context;

        public CommonRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<CommonDdlDto>> HouseListByUser(string userId)
        {
            try
            {
                var list = await _context.Houses
                    .Where(a => a.CreatedBy == CommonHelper.StringToUlidConverter(userId) && a.IsActive)
                    .Select(item => new CommonDdlDto
                    {
                        Value = item.Id,
                        Label = item.Name
                    })
                    .ToListAsync();

                return list;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
