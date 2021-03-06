using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.Configuration;
using OV.MainDb.Habitant.Models;
using OV.MainDb.Organizer.Models;
using OV.MainDb.Province.Models;
using OV.MainDb.SuperAdmin.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OV.MainDb.User.Models
{
    [Table("tblUsers")]
    public class PersistedUser
    {
        [Column("Id")] public int? Id { get; set; }
        [Column("FirstName")] public string FirstName { get; set; }
        [Column("Secondname")] public string SecondName { get; set; }
        [Column("SurName")] public string SurName { get; set; }
        [Column("SecondSurName")] public string SecondSurName { get; set; }
        [Column("Password")] public string Password { get; set; }
        [Column("DateOfBirth")] public DateTime DOB { get; set; }
        [Column("TblProvinces_UID")] public int TblProvince_UID { get; set; }
        [Column("Email")] public string Email { get; set; }
        [Column("PhoneNumber")] public string PhoneNumber { get; set; }
        [Column("IsAutorized")] public bool IsAutorized { get; set; }
        [Column("DNI_NIE")] public string DNI_NIE { get; set; }

        public PersistedProvince? Province { get; set; }
        public PersistedHabitant? Habitant { get; set; }
        public PersistedOrganizer? Organizer { get; set; }
        public PersistedSuperAdmin? SuperAdmin { get; set; }

        public OV.Models.MainDb.User.User ToUser()
        {
            return new OV.Models.MainDb.User.User() 
            {
                Id = Id,
                FirstName = FirstName,
                SecondName = SecondName,
                SurName = SurName,
                SecondSurName = SecondSurName,
                Password = Password,
                DOB = DOB,
                TblProvince_UID = TblProvince_UID,
                Email = Email,
                PhoneNumber = PhoneNumber,
                DNI_NIE = DNI_NIE,
                IsAutorized = IsAutorized,
                Province = Province?.ToProvince()
            };
        }

    }
    public class UserConfiguration : EntityConfiguration<PersistedUser>
    {
        public override void Configure(EntityTypeBuilder<PersistedUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Habitant).WithOne(h => h.User).HasForeignKey<PersistedHabitant>(h => h.tblUser_UID);
            builder.HasOne(u => u.Organizer).WithOne(o => o.User).HasForeignKey<PersistedOrganizer>(o => o.tblUser_UID);
            builder.HasOne(u => u.SuperAdmin).WithOne(sa => sa.User).HasForeignKey<PersistedSuperAdmin>(o => o.tblUser_UID);
            builder.HasOne(u => u.Province).WithMany(p => p.Users).HasForeignKey(u => u.TblProvince_UID);
        }
    }
}
