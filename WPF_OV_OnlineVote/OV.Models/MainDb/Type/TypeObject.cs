namespace OV.Models.MainDb.Type
{
    public class TypeObject
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }

        public Election.Election? Election { get; set; }
    }
}
