using System;

namespace OV.MainDb.SuperAdmin.Find.Models.Public
{
    public class SuperAdminFilter
    {
        public int Id { get; }
        public string? DNI_NIE { get; }
        public string? Password { get; }
        public string? ReferenceNumber { get; }
        public bool UserIncluded { get; } = false;

        public SuperAdminFilter() { }

        public SuperAdminFilter(int id, string? dni_nie, string? password, string referenceNumber, bool userIncluded)
        {
            Id = id;
            DNI_NIE = dni_nie;
            Password = password;
            ReferenceNumber = referenceNumber;
            UserIncluded = userIncluded;
        }

        // Do not filter
        public static SuperAdminFilter All = new SuperAdminFilter();

        public static SuperAdminFilter ById(int id) => All.AndById(id);
        public static SuperAdminFilter ByDNI_NIE(string dni_nie) => All.AndByDNI_NIE(dni_nie);
        public static SuperAdminFilter ByDNI_NIE_Password_ReferenceNumber(string dni_nie, string password, string referenceNumber)
                            => All.AndByDNI_NIE_Password_ReferenceNumber(dni_nie, password, referenceNumber);
        public static SuperAdminFilter IncludeUser() => All.AndIncludeUser();


        public SuperAdminFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new SuperAdminFilter(id, DNI_NIE, Password, ReferenceNumber, UserIncluded);
        }
        public SuperAdminFilter AndByDNI_NIE(string dni_nie)
        {
            if (dni_nie == default(string)) return this;
            return new SuperAdminFilter(Id, dni_nie, Password, ReferenceNumber, UserIncluded);
        }
        public SuperAdminFilter AndByDNI_NIE_Password_ReferenceNumber(string dni_nie, string password, string referenceNumber)
        {
            if (dni_nie == default(string) || password == default(string) || referenceNumber == default(string)) return this;
            return new SuperAdminFilter(Id, dni_nie, password, referenceNumber, UserIncluded);
        }
        public SuperAdminFilter AndIncludeUser()
        {
            return new SuperAdminFilter(Id, DNI_NIE, Password, ReferenceNumber, true);
        }

        public bool Equals(SuperAdminFilter other)
        {
            return Id.Equals(other.Id) && (DNI_NIE?.Equals(other.DNI_NIE) ?? (other.DNI_NIE == null))
                && (Password?.Equals(other.Password) ?? (other.Password == null)) && (ReferenceNumber?.Equals(other.ReferenceNumber) ?? (other.ReferenceNumber == null))
                        && UserIncluded == other.UserIncluded;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SuperAdminFilter && Equals((SuperAdminFilter)obj);
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
