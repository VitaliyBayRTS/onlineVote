using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Configuration;
using System.ComponentModel.DataAnnotations.Schema;

namespace OV.MainDb.Province.Models
{
    [Table("tblProvinces")]
    public class PersistedProvince
    {
        [Column("Id")] public int Id { get; set; }
        [Column("Name")] public string Name { get; set; }
        [Column("tblAutonomousCommunity_UID")] public int tblAutonomousCommunity_UID { get; set; }

        public PersistedAutonomousCommunity? AutonomousCommunity { get; set; }

        public OV.Models.MainDb.Province.Province ToProvince()
        {
            return new OV.Models.MainDb.Province.Province()
            {
                Id = this.Id,
                Name = this.Name,
                AutonomousCommunity = this.AutonomousCommunity.ToAutonomousCommunity()
            };
        }

    }

    public class ProvinceConfiguration : EntityConfiguration<PersistedProvince>
    {
        public override void Configure(EntityTypeBuilder<PersistedProvince> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.AutonomousCommunity).WithMany(ac => ac.Provinces).HasForeignKey(ac => ac.tblAutonomousCommunity_UID);
        }
    }
}
