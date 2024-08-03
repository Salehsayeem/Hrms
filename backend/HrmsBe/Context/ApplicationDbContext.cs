using HrmsBe.Helper;
using HrmsBe.Models;
using HrmsBe.Models.BaseModel;
using Microsoft.EntityFrameworkCore;

namespace HrmsBe.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
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


    }

}
