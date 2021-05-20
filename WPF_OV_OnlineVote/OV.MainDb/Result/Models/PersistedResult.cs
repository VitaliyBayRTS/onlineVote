using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OV.MainDb.Result.Models
{
    [Table("tblResult")]
    public class PersistedResult
    {
        [Column("Id")] public int? Id { get; set; }
        [Column("TotalVotes")] public int TotalVotes { get; set; }
        [Column("Winner_UID")] public int? Winner_UID { get; set; }
        [Column("TotalHabitants")] public int TotalHabitants { get; set; }
        [Column("HabitantsThatParticipate")] public int HabitantsThatParticipate { get; set; }

        public PersistedElection Election { get; set; }

        public OV.Models.MainDb.Result.Result ToResult()
        {
            return new OV.Models.MainDb.Result.Result()
            {
                Id = Id,
                TotalVotes = TotalVotes,
                Winner_UID = Winner_UID,
                TotalHabitants = TotalHabitants,
                HabitantsThatParticipate = HabitantsThatParticipate,
                Election = Election.ToElection()
            };
        }

    }
    public class ResultConfiguration : EntityConfiguration<PersistedResult>
    {
        public override void Configure(EntityTypeBuilder<PersistedResult> builder)
        {
            builder.HasKey(u => u.Id);
        }
    }
}
