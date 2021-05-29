using Microsoft.EntityFrameworkCore;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Election.Models;
using OV.MainDb.Habitant.Models;
using OV.MainDb.Option.Models;
using OV.MainDb.Organizer.Models;
using OV.MainDb.Province.Models;
using OV.MainDb.Result.Models;
using OV.MainDb.SuperAdmin.Models;
using OV.MainDb.Type.Models;
using OV.MainDb.User.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using static OV.MainDb.Election.Models.PersistedElection;

namespace OV.MainDb.Configuration
{
    public interface IOvMainDbContext : IDisposable
    {
        DbSet<PersistedProvince> Provinces { get; set; }
        DbSet<PersistedAutonomousCommunity> AutonomousCommunities { get; set; }
        DbSet<PersistedUser> Users { get; set; }
        DbSet<PersistedHabitant> Habitants { get; set; }
        DbSet<PersistedOrganizer> Organizers { get; set; }
        DbSet<PersistedSuperAdmin> SuperAdmin { get; set; }
        DbSet<PersistedElection> Elections { get; set; }
        DbSet<PersistedType> Types { get; set; }
        DbSet<PersistedResult> Results { get; set; }
        DbSet<PersistedOption> Options { get; set; }

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
        public DbSet<PersistedOrganizer> Organizers { get; set; }
        public DbSet<PersistedSuperAdmin> SuperAdmin { get; set; }
        public DbSet<PersistedElection> Elections { get; set; }
        public DbSet<PersistedType> Types { get; set; }
        public DbSet<PersistedResult> Results { get; set; }
        public DbSet<PersistedOption> Options { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProvinceConfiguration());
            modelBuilder.ApplyConfiguration(new AutonomousCommunityConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new HabitantConfiguration());
            modelBuilder.ApplyConfiguration(new OrganizerConfiguration());
            modelBuilder.ApplyConfiguration(new SuperAdminConfiguration());
            modelBuilder.ApplyConfiguration(new ElectionConfiguration());
            modelBuilder.ApplyConfiguration(new TypeConfiguration());
            modelBuilder.ApplyConfiguration(new ResultConfiguration());
            modelBuilder.ApplyConfiguration(new OptionConfiguration());
        }
    }
}
