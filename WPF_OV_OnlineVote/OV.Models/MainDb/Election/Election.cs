using System;
using System.Collections.Generic;

namespace OV.Models.MainDb.Election
{
    public class Election
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

        public Type.TypeObject Type { get; set; }
        public Province.Province? Province { get; set; }
        public AutonomousCommunity.AutonomousCommunity? AutonomousCommunity { get; set; }
        public Result.Result? Result{ get; set; }
        public ICollection<Option.Option>? Options{ get; set; }
        public ICollection<Organizer.Organizer> Organizers { get; set; }
    }
}
