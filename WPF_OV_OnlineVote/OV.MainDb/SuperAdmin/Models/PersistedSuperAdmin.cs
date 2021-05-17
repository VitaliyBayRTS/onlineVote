using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.Configuration;
using OV.MainDb.User.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace OV.MainDb.SuperAdmin.Models
{
    [Table("tblSuperAdmin")]
    public class PersistedSuperAdmin
    {
        [Column("Id")] public int Id { get; set; }
        [Column("tblUser_UID")] public int tblUser_UID { get; set; }
        [Column("ReferenceNumber")] public string ReferenceNumber { get; set; }

        public PersistedUser? User { get; set; }

        public OV.Models.MainDb.SuperAdmin.SuperAdmin ToSuperAdmin() 
        {
            return new OV.Models.MainDb.SuperAdmin.SuperAdmin()
            {
                Id = this.Id,
                tblUser_UID = this.tblUser_UID,
                ReferenceNumber = this.ReferenceNumber,
                User = this.User?.ToUser()
            };
        }
    }

    public class SuperAdminConfiguration : EntityConfiguration<PersistedSuperAdmin>
    {
        public override void Configure(EntityTypeBuilder<PersistedSuperAdmin> builder)
        {
            builder.HasKey(sa => sa.Id);
        }
    }
}
