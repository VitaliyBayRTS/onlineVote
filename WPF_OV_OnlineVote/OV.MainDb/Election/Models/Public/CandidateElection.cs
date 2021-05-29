using System;

namespace OV.MainDb.Election.Models.Public
{
    public class CandidateElection
    {
        public int? Id { get; set; }
        public int tblType_UID { get; set; }
        public int? tblResult_UID { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime FinalizeDate { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int? tblAutonomousCommunity_UID { get; set; }
        public int? tblProvince_UID { get; set; }

        public CandidateElection(OV.Models.MainDb.Election.Election election)
        {
            Id = election.Id;
            tblType_UID = election.tblType_UID;
            tblResult_UID = election.tblResult_UID;
            InitDate = election.InitDate;
            FinalizeDate = election.FinalizeDate;
            Description = election.Description;
            Name = election.Name;
            tblAutonomousCommunity_UID = election.tblAutonomousCommunity_UID;
            tblProvince_UID = election.tblProvince_UID;
        }

        public CandidateElection()
        {

        }

    }
}
