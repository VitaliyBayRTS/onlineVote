using System;

namespace OV.MainDb.Election.Find.Models.Public
{
    public class ElectionFilter
    {
        public int? Id{ get; }
        public bool TypeIncluded { get; } = false;
        public bool ACIncluded { get; } = false;
        public bool ProvinceIncluded { get; } = false;
        public bool OrganizersIncluded { get; } = false;

        public ElectionFilter() { }

        public ElectionFilter(int? id, bool typeIncluded, bool aCIncluded, bool provinceIncluded, bool organizersIncluded)
        {
            Id = id;
            TypeIncluded = typeIncluded;
            ACIncluded = aCIncluded;
            ProvinceIncluded = provinceIncluded;
            OrganizersIncluded = organizersIncluded;
        }

        // Do not filter
        public static ElectionFilter All = new ElectionFilter();

        public static ElectionFilter ById(int id) => All.AndById(id);
        public static ElectionFilter IncludType() => All.AndTypeIncluded();
        public static ElectionFilter IncludAC() => All.AndACIncluded();
        public static ElectionFilter IncludProvince() => All.AndProvinceIncluded();
        public static ElectionFilter IncludOrganizers() => All.AndOrganizersIncluded();


        public ElectionFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new ElectionFilter(id, true, ACIncluded, ProvinceIncluded, OrganizersIncluded);
        }

        public ElectionFilter AndTypeIncluded()
        {
            return new ElectionFilter(Id, true, ACIncluded, ProvinceIncluded, OrganizersIncluded);
        }
        public ElectionFilter AndACIncluded()
        {
            return new ElectionFilter(Id, TypeIncluded, true, ProvinceIncluded, OrganizersIncluded);
        }
        public ElectionFilter AndProvinceIncluded()
        {
            return new ElectionFilter(Id, TypeIncluded, ACIncluded, true, OrganizersIncluded);
        }
        public ElectionFilter AndOrganizersIncluded()
        {
            return new ElectionFilter(Id, TypeIncluded, ACIncluded, ProvinceIncluded, true);
        }

        public bool Equals(ElectionFilter other)
        {
            return Id == other.Id && TypeIncluded == other.TypeIncluded && ACIncluded == other.ACIncluded 
                && ProvinceIncluded == other.ProvinceIncluded && OrganizersIncluded == other.OrganizersIncluded;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ElectionFilter && Equals((ElectionFilter)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCode.Combine(Id.GetHashCode(), TypeIncluded.GetHashCode(), ACIncluded.GetHashCode(), 
                    ProvinceIncluded.GetHashCode(), OrganizersIncluded.GetHashCode());
            }
        }
    }
}
