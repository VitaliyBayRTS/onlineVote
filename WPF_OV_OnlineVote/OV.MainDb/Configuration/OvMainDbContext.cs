using Microsoft.EntityFrameworkCore;
using OV.MainDb.Province.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OV.MainDb.Configuration
{
    public interface IOvMainDbContext
    {
        DbSet<PersistedProvince> Provinces { get; set; }
    }
    public class OvMainDbContext : DbContext, IOvMainDbContext
    {
        public OvMainDbContext(DbContextOptions<OvMainDbContext> options) : base(options)
        {
        }

        public DbSet<PersistedProvince> Provinces { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProvinceConfiguration());
        }
    }
}
