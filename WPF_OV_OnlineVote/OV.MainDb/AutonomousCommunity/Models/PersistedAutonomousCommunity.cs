using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using OV.MainDb.Province.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OV.MainDb.AutonomousCommunity.Models
{
    [Table("tblAutonomousCommunities")]
    public class PersistedAutonomousCommunity
    {
        [Column("Id")] public int Id { get; set; }
        [Column("Name")] public string Name { get; set; }

        public ICollection<PersistedProvince>? Provinces { get; set; }
        public ICollection<PersistedElection>? Elections { get; set; }

        public OV.Models.MainDb.AutonomousCommunity.AutonomousCommunity ToAutonomousCommunity()
        {
            return new OV.Models.MainDb.AutonomousCommunity.AutonomousCommunity()
            {
                Id = this.Id,
                Name = this.Name,
                Provinces = this.Provinces?.Select(p => p.ToProvince()),
                Election = this.Elections?.Select(e => e.ToElection())
            };
        }

    }

    public class AutonomousCommunityConfiguration : EntityConfiguration<PersistedAutonomousCommunity>
    {
        public override void Configure(EntityTypeBuilder<PersistedAutonomousCommunity> builder)
        {
            builder.HasKey(ac => ac.Id);
            builder.HasMany(p => p.Provinces).WithOne(u => u.AutonomousCommunity).HasForeignKey(p => p.tblAutonomousCommunity_UID);
        }
    }
}
