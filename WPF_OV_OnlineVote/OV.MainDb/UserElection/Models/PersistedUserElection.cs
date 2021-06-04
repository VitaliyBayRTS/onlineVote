using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Models;
using OV.MainDb.User.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OV.MainDb.UserElection.Models
{
    [Table("tblUserElection")]
    public class PersistedUserElection
    {
        [Column("Id")] public int? Id { get; set; }
        [Column("TblUser_UID")] public int TblUser_UID { get; set; }
        [Column("TblElection_UID")] public int TblElection_UID { get; set; }

        public PersistedUser? User { get; set; }
        public PersistedElection? Election { get; set; }

        public OV.Models.MainDb.UserElection.UserElection ToUserElection()
        {
            return new OV.Models.MainDb.UserElection.UserElection()
            {
                Id = Id,
                TblUser_UID = TblUser_UID,
                TblElection_UID = TblElection_UID
            };
        }

    }

    public class UserElectionConfiguration : EntityConfiguration<PersistedUserElection>
    {
        public override void Configure(EntityTypeBuilder<PersistedUserElection> builder)
        {
            builder.HasKey(u => u.Id);
        }
    }
}
