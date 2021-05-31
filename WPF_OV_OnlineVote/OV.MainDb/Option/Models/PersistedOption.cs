using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OV.MainDb.Option.Models
{
    [Table("tblOption")]
    public class PersistedOption
    {
        [Column("Id")] public int? Id { get; set; }
        [Column("tblElection_UID")] public int tblElection_UID { get; set; }
        [Column("Name")] public string Name { get; set; }
        [Column("Description")] public string Description { get; set; }
        [Column("Votes")] public int? Votes { get; set; }

        public PersistedElection? Election { get; set; }

        public OV.Models.MainDb.Option.Option ToOption()
        {
            return new OV.Models.MainDb.Option.Option()
            {
                Id = Id,
                tblElection_UID = tblElection_UID,
                Name = Name,
                Description = Description,
                Votes = Votes,
                Election = Election?.ToElection()
            };
        }

    }
    public class OptionConfiguration : EntityConfiguration<PersistedOption>
    {
        public override void Configure(EntityTypeBuilder<PersistedOption> builder)
        {
            builder.HasKey(o => o.Id);
        }
    }
}
