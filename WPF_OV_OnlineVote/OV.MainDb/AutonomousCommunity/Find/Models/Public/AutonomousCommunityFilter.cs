using System;

namespace OV.MainDb.AutonomousCommunity.Find.Models.Public
{
    public class AutonomousCommunityFilter
    {
        public int Id { get; }
        public string Name { get; }
        public bool ProvinceIncluded { get; } = false;

        public AutonomousCommunityFilter() { }

        public AutonomousCommunityFilter(int id, string name, bool provinceIncluded)
        {
            Id = id;
            Name = name;
            ProvinceIncluded = provinceIncluded;
        }

        // Do not filter
        public static AutonomousCommunityFilter All = new AutonomousCommunityFilter();

        public static AutonomousCommunityFilter ById(int id) => All.AndById(id);
        public static AutonomousCommunityFilter ByName(string name) => All.AndByName(name);
        public static AutonomousCommunityFilter IncludeProvince() => All.AndIncludeProvince();


        public AutonomousCommunityFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new AutonomousCommunityFilter(id, Name, ProvinceIncluded);
        }
        public AutonomousCommunityFilter AndByName(string name)
        {
            if (name == default(string)) return this;
            return new AutonomousCommunityFilter(Id, name, ProvinceIncluded);
        }
        public AutonomousCommunityFilter AndIncludeProvince()
        {
            return new AutonomousCommunityFilter(Id, Name, true);
        }

        public bool Equals(AutonomousCommunityFilter other)
        {
            return Id.Equals(other.Id) && (Name?.Equals(other.Name) ?? (other.Name == null)) && ProvinceIncluded == other.ProvinceIncluded;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is AutonomousCommunityFilter && Equals((AutonomousCommunityFilter)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCode.Combine(Id.GetHashCode(), Name.GetHashCode(), ProvinceIncluded.GetHashCode());
            }
        }
    }
}
