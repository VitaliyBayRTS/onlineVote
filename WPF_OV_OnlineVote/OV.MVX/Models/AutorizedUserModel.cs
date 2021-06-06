using OV.Models.MainDb.Province;
using OV.Models.MainDb.User;
using OV.MVX.Helpers;
using OV.Services.DocumentValidator;

namespace OV.MVX.Models
{
    public class AutorizedUserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string SurName { get; set; }
        public string SecondSurName { get; set; }
        public string SurNames { get; set; }
        public string DOB { get; set; }
        public string DNI_NIE { get; set; }
        public string DocumentType { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NamesSurnamesAndDOB { get; set; }
        public Province Province { get; set; }

        public void SetData(User user)
        {
            Id = user.Id.Value;
            FirstName = user.FirstName;
            SecondName = user.SecondName;
            SurName = user.SurName;
            SecondSurName = user.SecondSurName;
            SurNames = user.SurName + " " + user.SecondSurName;
            DOB = user.DOB.ToString("yyyy/MM/dd");
            DNI_NIE = user.DNI_NIE;
            DocumentType = DocumentValidation.GetDocumentType(user.DNI_NIE);
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            NamesSurnamesAndDOB = user.FirstName + " " + user.SecondName + " " + SurNames + " " + DOB;
            Province = user.Province;
        }
    }
}
