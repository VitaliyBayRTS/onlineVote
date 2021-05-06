using System;

namespace OV.MainDb.User.Models.Public
{
    public class CandidateUser
    {
        public int? Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string SurName { get; set; }
        public string SecondSurName { get; set; }
        public string Password { get; set; }
        public DateTime DOB { get; set; }
        public int TblAutonomousCommunities_UID { get; set; }
        public int TblProvince_UID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public CandidateUser(OV.Models.MainDb.User.User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            SecondName = user.SecondName;
            SurName = user.SurName;
            SecondSurName = user.SecondSurName;
            Password = user.Password;
            DOB = user.DOB;
            TblAutonomousCommunities_UID = user.TblAutonomousCommunities_UID;
            TblProvince_UID = user.TblProvince_UID;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
        }

        public CandidateUser()
        {
        }
    }
}
