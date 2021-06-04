namespace OV.MVX.Models.Habitant
{
    public class ShortOrganizerInfo
    {
        public int? Id { get; set; }
        public string FullName { get; set; }
        public string DNI { get; set; }

        public void SetData(OV.Models.MainDb.Organizer.Organizer organizer)
        {
            Id = organizer.Id;
            FullName = organizer?.User.FirstName + " " + organizer?.User.SecondName + ", " + organizer?.User.SurName + " " + organizer?.User.SecondSurName;
            DNI = organizer?.User.DNI_NIE;
        }
    }
}
