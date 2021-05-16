using System;
using System.Collections.Generic;
using System.Text;

namespace OV.MainDb.Province.Find.Models.Public
{
    public class ProvinceFilter
    {
        public int Id { get; }
        public string? Name { get; }
        public bool AutonomousCommunityIncluded { get; } = false;

        public ProvinceFilter() { }

        public ProvinceFilter(int id, string? name, bool autonomousCommunityIncluded)
        {
            Id = id;
            Name = name;
            AutonomousCommunityIncluded = autonomousCommunityIncluded;
        }

        // Do not filter
        public static ProvinceFilter All = new ProvinceFilter();

        public static ProvinceFilter ById(int id) => All.AndById(id);
        public static ProvinceFilter ByName(string name) => All.AndByName(name);
        public static ProvinceFilter IncludeProvince() => All.AndIncludeAutonomousCommunity();


        public ProvinceFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new ProvinceFilter(id, Name, AutonomousCommunityIncluded);
        }
        public ProvinceFilter AndByName(string name)
        {
            if (name == default(string)) return this;
            return new ProvinceFilter(Id, name, AutonomousCommunityIncluded);
        }
        public ProvinceFilter AndIncludeAutonomousCommunity()
        {
            return new ProvinceFilter(Id, Name, true);
        }

        public bool Equals(ProvinceFilter other)
        {
            return Id.Equals(other.Id) && (Name?.Equals(other.Name) ?? (other.Name == null)) 
                        && AutonomousCommunityIncluded == other.AutonomousCommunityIncluded;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ProvinceFilter && Equals((ProvinceFilter)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCode.Combine(Id.GetHashCode(), Name.GetHashCode(), AutonomousCommunityIncluded.GetHashCode());
            }
        }
    }
}
