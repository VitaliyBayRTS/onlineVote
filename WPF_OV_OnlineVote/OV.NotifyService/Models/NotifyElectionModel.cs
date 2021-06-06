using System;

namespace OV.NotifyService.Models
{
    public class NotifyElectionModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InitDateString { get; set; }
        public string Type { get; set; }

        public void SetDate(OV.Models.MainDb.Election.Election election)
        {
            Id = election.Id.Value;
            Name = election.Name;
            InitDateString = election.InitDate.ToString("yyyy/MM/dd");
            Type = election.Type != null ? election.Type.Name : "";
        }
    }
}
