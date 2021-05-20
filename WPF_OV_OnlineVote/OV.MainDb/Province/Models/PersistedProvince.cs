using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using OV.MainDb.User.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OV.MainDb.Province.Models
{
    [Table("tblProvinces")]
    public class PersistedProvince
    {
        [Column("Id")] public int Id { get; set; }
        [Column("Name")] public string Name { get; set; }
        [Column("tblAutonomousCommunity_UID")] public int tblAutonomousCommunity_UID { get; set; }

        public PersistedAutonomousCommunity AutonomousCommunity { get; set; }
        public ICollection<PersistedUser>? Users { get; set; }
        public ICollection<PersistedElection>? Elections { get; set; }

        public OV.Models.MainDb.Province.Province ToProvince()
        {
            return new OV.Models.MainDb.Province.Province()
            {
                Id = this.Id,
                Name = this.Name,
                AutonomousCommunity = this.AutonomousCommunity.ToAutonomousCommunity(),
                Elections = Elections != null ? Elections.Select(e => e.ToElection()).ToList() : null,
                Users = Users != null ? Users.Select(u => u.ToUser()).ToList() : null
            };
        }

    }

    public class ProvinceConfiguration : EntityConfiguration<PersistedProvince>
    {
        public override void Configure(EntityTypeBuilder<PersistedProvince> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasMany(p => p.Users).WithOne(u => u.Province).HasForeignKey(p => p.TblProvince_UID);
        }
    }
}
