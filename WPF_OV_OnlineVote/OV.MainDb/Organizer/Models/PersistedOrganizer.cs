using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using OV.MainDb.Organizer.Models.Public;
using OV.MainDb.User.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OV.MainDb.Organizer.Models
{
    [Table("tblOrganizer")]
    public class PersistedOrganizer
    {

        [Column("Id")] public int? Id { get; set; }
        [Column("tblUser_UID")] public int tblUser_UID { get; set; }
        [Column("tblElection_UID")] public int tblElection_UID { get; set; }
        [Column("ReferenceNumber")] public string ReferenceNumber { get; set; }

        public PersistedUser? User { get; set; }
        public PersistedElection? Election { get; set; }

        public OV.Models.MainDb.Organizer.Organizer ToOrganizer()
        {
            return new OV.Models.MainDb.Organizer.Organizer()
            {
                Id = this.Id,
                tblUser_UID = this.tblUser_UID,
                tblElection_UID = this.tblElection_UID,
                ReferenceNumber = this.ReferenceNumber,
                User = this.User?.ToUser(),
                Election = Election?.ToElection()
            };
        }
        public PersistedOrganizer(CandidateOrganizer candidate)
        {
            Id = candidate?.Id;
            tblUser_UID = candidate.tblUser_UID;
            tblElection_UID = candidate.tblElection_UID;
            ReferenceNumber = candidate.ReferenceNumber;
        }

        public PersistedOrganizer()
        {
        }
    }

    public class OrganizerConfiguration : EntityConfiguration<PersistedOrganizer>
    {
        public override void Configure(EntityTypeBuilder<PersistedOrganizer> builder)
        {
            builder.HasKey(h => h.Id);
        }
    }
}
