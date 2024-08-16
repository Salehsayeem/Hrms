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

            return services;
        }
    }
}
