using HrmsBe.Helper;
using HrmsBe.Models;
using HrmsBe.Models.BaseModel;
using Microsoft.EntityFrameworkCore;

namespace HrmsBe.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly MongoDbService _mongoDbService;
        public AuditDto? AuditInfo { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,MongoDbService mongoDbService)
        : base(options)
        {
            _mongoDbService = mongoDbService;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<RoomCategory> RoomCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            CommonHelper.ApplyCommonConfigurations(modelBuilder);
            modelBuilder.Ignore<BaseEntity>();
        }

        public override int SaveChanges()
        {
            LogAuditDetails().Wait();
            return base.SaveChanges();
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await LogAuditDetails();
            return await base.SaveChangesAsync(cancellationToken);
        }
        private async Task LogAuditDetails()
        {
            if (AuditInfo !=null)
            {
                await _mongoDbService.CreateOrUpdateAudit(AuditInfo);
            }
        }
    }

}
