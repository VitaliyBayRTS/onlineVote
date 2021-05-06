using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Configuration;
using OV.MainDb.Province.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OV.MainDb.User.Models
{
    [Table("tblUsers")]
    public class PersistedUser
    {
        [Column("Id")] public int Id { get; set; }
        [Column("FirstName")] public string FirstName { get; set; }
        [Column("Secondname")] public string SecondName { get; set; }
        [Column("SurName")] public string SurName { get; set; }
        [Column("SecondSurName")] public string SecondSurName { get; set; }
        [Column("Password")] public string Password { get; set; }
        [Column("DateOfBirth")] public DateTime DOB { get; set; }
        [Column("AutonomousCommunities")] public int TblAutonomousCommunities_UID { get; set; }
        [Column("Province")] public int TblProvince_UID { get; set; }
        [Column("Email")] public string Email { get; set; }
        [Column("PhoneNumber")] public string PhoneNumber { get; set; }

        public PersistedAutonomousCommunity? AutonomousCommunity { get; set; }
        public PersistedProvince? Province { get; set; }

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
                TblAutonomousCommunities_UID = TblAutonomousCommunities_UID,
                TblProvince_UID = TblProvince_UID,
                Email = Email,
                PhoneNumber = PhoneNumber
            };
        }

    }
    public class UserConfiguration : EntityConfiguration<PersistedUser>
    {
        public override void Configure(EntityTypeBuilder<PersistedUser> builder)
        {
            builder.HasKey(p => p.Id);

            builder.HasOne(p => p.AutonomousCommunity).WithMany(ac => ac.Users).HasForeignKey(ac => ac.TblAutonomousCommunities_UID);
            builder.HasOne(p => p.Province).WithMany(ac => ac.Users).HasForeignKey(ac => ac.TblProvince_UID);
        }
    }
}
