using OV.MainDb.AutonomousCommunity.Models;
using OV.MainDb.Election.Models;
using OV.MainDb.Organizer.Models;
using OV.MainDb.Province.Models;
using OV.MainDb.Type.Models;
using System;

namespace OV.MainDb.Tests.Election.Find
{
    internal static class FindElectionPersistedGenerator
    {
        public static PersistedElection _dummyPersistedElection1 = new PersistedElection()
        {
            Id = 1,
            Name = "name",
            Description = "Desc",
            InitDate = DateTime.Today,
            FinalizeDate = DateTime.Today.AddDays(2),
            tblType_UID = 1,
            Type = new PersistedType()
            {
                Id = 1,
                Name = "TypeName",
                Description = "TypeDesc",
                Code = "TypeCode"
            },
            tblProvince_UID = 1,
            Province = new PersistedProvince()
            {
                Id = 1,
                Name = "ProvinceName",
                tblAutonomousCommunity_UID = 1,
                AutonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Id = 1,
                    Name = "ACname"
                }
            },
            tblAutonomousCommunity_UID = 1
        };
        
        public static PersistedElection _dummyPersistedElection2 = new PersistedElection()
        {
            Id = 2,
            Name = "name",
            Description = "Desc",
            InitDate = DateTime.Today,
            FinalizeDate = DateTime.Today.AddDays(2),
            tblType_UID = 2,
            Type = new PersistedType()
            {
                Id = 2,
                Name = "TypeName",
                Description = "TypeDesc",
                Code = "TypeCode"
            },
            tblProvince_UID = 2,
            Province = new PersistedProvince()
            {
                Id = 2,
                Name = "ProvinceName",
                tblAutonomousCommunity_UID = 2,
                AutonomousCommunity = new PersistedAutonomousCommunity()
                {
                    Id = 2,
                    Name = "ACname"
                }
            },
            tblAutonomousCommunity_UID = 1
        };

        public static PersistedOrganizer _dummyPersistedOrganizer_1 = new PersistedOrganizer()
        {
            Id = 1,
            tblUser_UID = 1,
            tblElection_UID = 1,
            ReferenceNumber = "RefNumber2"
        };
        
        public static PersistedOrganizer _dummyPersistedOrganizer_2 = new PersistedOrganizer()
        {
            Id = 2,
            tblUser_UID = 2,
            tblElection_UID = 2,
            ReferenceNumber = "RefNumber2"
        };
    }
}
