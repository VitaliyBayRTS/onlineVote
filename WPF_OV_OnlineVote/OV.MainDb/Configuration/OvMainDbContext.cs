using Microsoft.EntityFrameworkCore;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Province.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OV.MainDb.Configuration
{
    public interface IOvMainDbContext
    {
        DbSet<PersistedProvince> Provinces { get; set; }
        DbSet<PersistedAutonomousCommunity> AutonomousCommunities { get; set; }
    }
    public class OvMainDbContext : DbContext, IOvMainDbContext
    {
        public OvMainDbContext(DbContextOptions<OvMainDbContext> options) : base(options)
        {
        }

        public DbSet<PersistedProvince> Provinces { get; set; } = default!;
        public DbSet<PersistedAutonomousCommunity> AutonomousCommunities { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AutonomousCommunityConfiguration());
            modelBuilder.ApplyConfiguration(new ProvinceConfiguration());
        }
    }
}
