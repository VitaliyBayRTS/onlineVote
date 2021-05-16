using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.Configuration;
using OV.MainDb.User.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OV.MainDb.Habitant.Models
{
    [Table("tblHabitant")]
    public class PersistedHabitant
    {
            [Column("Id")] public int Id { get; set; }
            [Column("tblUser_UID")] public int tblUser_UID { get; set; }

            public PersistedUser? User { get; set; }

            public OV.Models.MainDb.Habitant.Habitant ToHabitant()
            {
            return new OV.Models.MainDb.Habitant.Habitant()
            {
                Id = this.Id,
                tblUser_UID = this.tblUser_UID,
                User = this.User?.ToUser()
                };
            }

        }

        public class HabitantConfiguration : EntityConfiguration<PersistedHabitant>
        {
            public override void Configure(EntityTypeBuilder<PersistedHabitant> builder)
            {
                builder.HasKey(h => h.Id);
            }
        }
}
