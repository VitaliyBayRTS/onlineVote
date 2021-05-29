using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Province.Models;
using OV.MainDb.User.Models;
using System;

namespace OV.MainDb.Tests.User.Find
{
    public static class FindUserPersistedGenerator
    {
        public static PersistedUser _dummyUser1 = new PersistedUser()
        {
            Id = 100,
            DNI_NIE = "12345678z",
            DOB = DateTime.Now,
            Email = "asd@asd",
            FirstName = "FirstName1",
            IsAutorized = false,
            Password = "asdasdasd",
            PhoneNumber = "123987685",
            SurName = "SurName",
            TblProvince_UID = 1
        };
        public static PersistedUser _dummyUser2 = new PersistedUser()
        {
            Id = 200,
            DNI_NIE = "12345678z",
            DOB = DateTime.Now,
            Email = "asd@asd",
            FirstName = "FirstName2",
            IsAutorized = false,
            Password = "asdasdasd",
            PhoneNumber = "123987685",
            SurName = "SurName",
            TblProvince_UID = 1
        };
        public static PersistedUser _dummyUser3 = new PersistedUser()
        {
            Id = 300,
            DNI_NIE = "12345678z",
            DOB = DateTime.Now,
            Email = "asd@asd",
            FirstName = "FirstName3",
            IsAutorized = true,
            Password = "asdasdasd",
            PhoneNumber = "123987685",
            SurName = "SurName",
            TblProvince_UID = 1
        };

        public static PersistedProvince _dummyProvince = new PersistedProvince()
        {
            Id = 1,
            Name = "ProvinceName",
            tblAutonomousCommunity_UID = 1,
            AutonomousCommunity = new PersistedAutonomousCommunity()
            {
                Id = 1,
                Name = "ACname"
            }
        };
    }
}
