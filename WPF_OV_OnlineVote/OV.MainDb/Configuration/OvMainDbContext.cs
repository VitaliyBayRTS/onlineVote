using Microsoft.EntityFrameworkCore;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Habitant.Models;
using OV.MainDb.Province.Models;
using OV.MainDb.User.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Configuration
{
    public interface IOvMainDbContext : IDisposable
    {
        DbSet<PersistedProvince> Provinces { get; set; }
        DbSet<PersistedAutonomousCommunity> AutonomousCommunities { get; set; }
        DbSet<PersistedUser> Users { get; set; }
        DbSet<PersistedHabitant> Habitants { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
    public class OvMainDbContext : DbContext, IOvMainDbContext
    {
        public OvMainDbContext(DbContextOptions<OvMainDbContext> options) : base(options)
        {
        }

        public DbSet<PersistedProvince> Provinces { get; set; }
        public DbSet<PersistedAutonomousCommunity> AutonomousCommunities { get; set; }
        public DbSet<PersistedUser> Users { get; set; }
        public DbSet<PersistedHabitant> Habitants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProvinceConfiguration());
            modelBuilder.ApplyConfiguration(new AutonomousCommunityConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new HabitantConfiguration());
        }
    }
}
