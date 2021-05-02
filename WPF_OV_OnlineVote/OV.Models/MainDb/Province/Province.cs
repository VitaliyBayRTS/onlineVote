using System;
using System.Collections.Generic;
using System.Text;

namespace OV.Models.MainDb.Province
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AutonomousCommunity.AutonomousCommunity AutonomousCommunity { get; set; }
    }
}
