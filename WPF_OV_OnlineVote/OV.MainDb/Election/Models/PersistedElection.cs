using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Configuration;
using OV.MainDb.Option.Models;
using OV.MainDb.Province.Models;
using OV.MainDb.Result.Models;
using OV.MainDb.Type.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OV.MainDb.Election.Models
{
    [Table("tblElection")]
    public class PersistedElection
    {
        [Column("Id")] public int? Id { get; set; }
        [Column("tblType_UID")] public int tblType_UID { get; set; }
        [Column("tblResult_UID")] public int tblResult_UID { get; set; }
        [Column("InitDate")] public DateTime InitDate { get; set; }
        [Column("FinalizeDate")] public DateTime FinalizeDate { get; set; }
        [Column("Description")] public string Description { get; set; }
        [Column("tblAutonomousCommunity_UID")] public int tblAutonomousCommunity_UID { get; set; }
        [Column("tblProvince_UID")] public int tblProvince_UID { get; set; }

        public PersistedProvince? Province { get; set; }
        public PersistedAutonomousCommunity? AutonomousCommunity { get; set; }
        public PersistedType Type { get; set; }
        public PersistedResult Result { get; set; }
        public ICollection<PersistedOption> Options { get; set; }

        public OV.Models.MainDb.Election.Election ToElection()
        {
            return new OV.Models.MainDb.Election.Election()
            {
                Id = Id,
                tblType_UID = tblType_UID,
                tblResult_UID = tblResult_UID,
                InitDate = InitDate,
                FinalizeDate = FinalizeDate,
                Description = Description,
                tblAutonomousCommunity_UID = tblAutonomousCommunity_UID,
                tblProvince_UID = tblProvince_UID,
                Province = Province?.ToProvince(),
                AutonomousCommunity = AutonomousCommunity?.ToAutonomousCommunity(),
                Type = Type.ToType(),
                Result = Result?.ToResult(),
                Options = Options?.Select(o => o.ToOption()).ToList()
            };
        }


        public class ElectionConfiguration : EntityConfiguration<PersistedElection>
        {
            public override void Configure(EntityTypeBuilder<PersistedElection> builder)
            {
                builder.HasKey(e => e.Id);

                builder.HasOne(e => e.Type).WithOne(t => t.Election).HasForeignKey<PersistedElection>(e => e.tblType_UID);
                builder.HasOne(e => e.Result).WithOne(t => t.Election).HasForeignKey<PersistedElection>(e => e.tblType_UID);
                builder.HasOne(e => e.Province).WithMany(p => p.Elections).HasForeignKey(e => e.tblProvince_UID);
                builder.HasOne(e => e.AutonomousCommunity).WithMany(ac => ac.Elections).HasForeignKey(e => e.tblAutonomousCommunity_UID);
                builder.HasMany(e => e.Options).WithOne(o => o.Election).HasForeignKey(o => o.tblElection_UID);
            }
        }

    }
}
