using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace OV.MVX.Models
{
    enum State
    {
        Terminado,
        Pendiente,
        EnCurso
    }
    public class SuperAdminAllElectionModel
    {
        public int? Id { get; set; }
        public string TypeName { get; set; }
        public int? tblResult_UID { get; set; }
        public DateTime InitDate { get; set; }
        public string InitDateString { get; set; }
        public string FinalizeDateString { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string AutonomousCommunityName { get; set; }
        public int? AutonomousCommunityId { get; set; }
        public string ProvinceName { get; set; }
        public int? ProvinceId { get; set; }
        public IEnumerable<OV.Models.MainDb.Organizer.Organizer> Organizers { get; set; }
        public int OrganizersCount { get; set; }
        public string CurrentState { get; set; }

        public void SetData(OV.Models.MainDb.Election.Election election)
        {
            Id = election.Id;
            TypeName = election?.Type?.Name;
            tblResult_UID = election.tblResult_UID;
            InitDate = election.InitDate;
            InitDateString = election.InitDate.ToString("yyyy/MM/dd");
            FinalizeDateString = election.FinalizeDate.ToString("yyyy/MM/dd");
            Description = election.Description;
            Name = election.Name;
            AutonomousCommunityName = election?.AutonomousCommunity?.Name;
            AutonomousCommunityId = election?.tblAutonomousCommunity_UID;
            ProvinceName = election?.Province?.Name;
            ProvinceId = election?.tblProvince_UID;
            Organizers = election?.Organizers;
            OrganizersCount = election?.Organizers.ToList().Count ?? 0;
            SetElectionState(election.InitDate, election.FinalizeDate);
        }

        private void SetElectionState(DateTime initDate, DateTime finalizeDate)
        {
            ResourceManager rm = new ResourceManager("OV.MVX.Translation.Translation", Assembly.GetExecutingAssembly());
                
            if (initDate <= DateTime.Now && finalizeDate >= DateTime.Now)
            {
                CurrentState = rm.GetString(State.EnCurso.ToString());
            } 
            else if(initDate > DateTime.Now)
            {
                CurrentState = State.Pendiente.ToString();
            } 
            else if(finalizeDate < DateTime.Now)
            {
                CurrentState = State.Terminado.ToString();
            }
        }
    }
}
