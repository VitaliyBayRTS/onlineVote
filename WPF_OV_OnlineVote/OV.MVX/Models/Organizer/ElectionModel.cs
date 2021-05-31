using System;

namespace OV.MVX.Models.Organizer
{
    public class ElectionModel
    {
        public int? Id { get; set; }
        public string TypeName { get; set; }
        public int? tblResult_UID { get; set; }
        public DateTime InitDate { get; set; }
        public DateTime FinilizeDate { get; set; }
        public string InitDateString { get; set; }
        public string FinalizeDateString { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string AutonomousCommunityName { get; set; }
        public string ProvinceName { get; set; }
        public string CurrentState { get; set; }

        public void SetData(OV.Models.MainDb.Election.Election election)
        {
            Id = election.Id;
            TypeName = election?.Type?.Name;
            tblResult_UID = election.tblResult_UID;
            InitDate = election.InitDate;
            FinilizeDate = election.FinalizeDate;
            InitDateString = election.InitDate.ToString("yyyy/MM/dd");
            FinalizeDateString = election.FinalizeDate.ToString("yyyy/MM/dd");
            Description = election.Description;
            Name = election.Name;
            AutonomousCommunityName = election?.AutonomousCommunity?.Name;
            ProvinceName = election?.Province?.Name;
            SetElectionState(election.InitDate, election.FinalizeDate);
        }

        private void SetElectionState(DateTime initDate, DateTime finalizeDate)
        {
            if (initDate <= DateTime.Now && finalizeDate >= DateTime.Now)
            {
                CurrentState = State.EnCurso.ToString();
            }
            else if (initDate > DateTime.Now)
            {
                CurrentState = State.Pendiente.ToString();
            }
            else if (finalizeDate < DateTime.Now)
            {
                CurrentState = State.Terminado.ToString();
            }
        }
    }
}
