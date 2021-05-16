using System;

namespace OV.MainDb.Organizer.Find.Models.Public
{
    public class OrganizerFilter
    {
        public int Id { get; }
        public string? DNI_NIE { get; }
        public string? Password { get; }
        public string? ReferenceNumber { get; }
        public bool UserIncluded { get; } = false;

        public OrganizerFilter() { }

        public OrganizerFilter(int id, string? dni_nie, string? password, string referenceNumber, bool userIncluded)
        {
            Id = id;
            DNI_NIE = dni_nie;
            Password = password;
            ReferenceNumber = referenceNumber;
            UserIncluded = userIncluded;
        }

        // Do not filter
        public static OrganizerFilter All = new OrganizerFilter();

        public static OrganizerFilter ById(int id) => All.AndById(id);
        public static OrganizerFilter ByDNI_NIE(string dni_nie) => All.AndByDNI_NIE(dni_nie);
        public static OrganizerFilter ByDNI_NIE_Password_ReferenceNumber(string dni_nie, string password, string referenceNumber) 
                            => All.AndByDNI_NIE_Password_ReferenceNumber(dni_nie, password, referenceNumber);
        public static OrganizerFilter IncludeUser() => All.AndIncludeUser();


        public OrganizerFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new OrganizerFilter(id, DNI_NIE, Password, ReferenceNumber, UserIncluded);
        }
        public OrganizerFilter AndByDNI_NIE(string dni_nie)
        {
            if (dni_nie == default(string)) return this;
            return new OrganizerFilter(Id, dni_nie, Password, ReferenceNumber, UserIncluded);
        }
        public OrganizerFilter AndByDNI_NIE_Password_ReferenceNumber(string dni_nie, string password, string referenceNumber)
        {
            if (dni_nie == default(string) || password == default(string) || referenceNumber == default(string) ) return this;
            return new OrganizerFilter(Id, dni_nie, password, referenceNumber, UserIncluded);
        }
        public OrganizerFilter AndIncludeUser()
        {
            return new OrganizerFilter(Id, DNI_NIE, Password, ReferenceNumber, true);
        }

        public bool Equals(OrganizerFilter other)
        {
            return Id.Equals(other.Id) && (DNI_NIE?.Equals(other.DNI_NIE) ?? (other.DNI_NIE == null))
                && (Password?.Equals(other.Password) ?? (other.Password == null)) && (ReferenceNumber?.Equals(other.ReferenceNumber) ?? (other.ReferenceNumber == null))
                        && UserIncluded == other.UserIncluded;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OrganizerFilter && Equals((OrganizerFilter)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCode.Combine(Id.GetHashCode(), DNI_NIE.GetHashCode(), Password.GetHashCode(), ReferenceNumber.GetHashCode(), UserIncluded.GetHashCode());
            }
        }
    }
}
