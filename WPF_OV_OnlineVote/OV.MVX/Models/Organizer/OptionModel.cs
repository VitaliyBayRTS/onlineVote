namespace OV.MVX.Models.Organizer
{
    public class OptionModel
    {
        public int? Id { get; set; }
        public string Index { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Votes{ get; set; }
        public string VotesString{ get; set; }


        public void SetData(OV.Models.MainDb.Option.Option option, int index)
        {
            Id = option.Id;
            Index = "#" + index;
            Name = option.Name;
            Description = option.Description;
            Votes = option.Votes ?? 0;
            VotesString = "Votos: " + (option.Votes ?? 0);
        }

    }
}
