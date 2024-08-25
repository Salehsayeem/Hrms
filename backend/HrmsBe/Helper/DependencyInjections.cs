using HrmsBe.IRepositories;
using HrmsBe.Repository;

namespace HrmsBe.Helper
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddDependencyInjections(this IServiceCollection services)
        {
            services.AddSingleton<AuthHelper>();
            services.AddSingleton<MongoDbService>();
            services.AddTransient<IAuthRepo, AuthRepo>();
            services.AddTransient<IHouseRepo, HouseRepo>();
            services.AddTransient<IRoomCategoriesRepo, RoomCategoryRepo>();
            services.AddTransient<IRenterTypeRepo, RenterTypeRepo>();
            services.AddTransient<IRoomRepo, RoomRepo>();
            return services;
        }
    }
}
