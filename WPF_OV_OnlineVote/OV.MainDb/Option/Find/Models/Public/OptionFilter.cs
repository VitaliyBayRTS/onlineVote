using System;

namespace OV.MainDb.Option.Find.Models.Public
{
    public class OptionFilter
    {
        public int? Id { get; }
        public int? ElectionId { get; }

        public OptionFilter() { }

        public OptionFilter(int? id, int? electionId)
        {
            Id = id;
            ElectionId = electionId;
        }

        // Do not filter
        public static OptionFilter All = new OptionFilter();

        public static OptionFilter ById(int id) => All.AndById(id);
        public static OptionFilter ByElectionId(int id) => All.AndByElectionId(id);


        public OptionFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new OptionFilter(id, ElectionId);
        }
        public OptionFilter AndByElectionId(int electionId)
        {
            if (electionId == default(int)) return this;
            return new OptionFilter(Id, electionId);
        }

        public bool Equals(OptionFilter other)
        {
            return Id.Equals(other.Id) && ElectionId.Equals(other.ElectionId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OptionFilter && Equals((OptionFilter)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCode.Combine(Id.GetHashCode(), ElectionId.GetHashCode());
            }
        }
    }
}
