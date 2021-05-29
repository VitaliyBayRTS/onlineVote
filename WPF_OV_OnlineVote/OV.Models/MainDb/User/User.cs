using System;

namespace OV.Models.MainDb.User
{
    public class User
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string SurName { get; set; }
        public string SecondSurName { get; set; }
        public string Password { get; set; }
        public DateTime DOB { get; set; }
        public int TblProvince_UID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAutorized { get; set; }
        public string DNI_NIE { get; set; }
        public Province.Province Province { get; set; }
    }
}
