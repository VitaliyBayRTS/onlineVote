namespace OV.Models.MainDb.Type
{
    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Election.Election Election { get; set; }
    }
}
