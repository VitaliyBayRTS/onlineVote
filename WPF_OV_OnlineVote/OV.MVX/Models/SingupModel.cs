using System;
using System.Collections.Generic;
using System.Text;

namespace OV.MVX.Models
{
    class SingupModel
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string FirstSurName { get; set; }
        public string SecondSurName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int AutonomousCommunity { get; set; }
        public int Province { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DNI_NIE { get; set; }
        public string Password { get; set; }
    }
}
