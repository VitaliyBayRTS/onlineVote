using System;

namespace OV.MainDb.User.Find.Models.Public
{
    public class UserFilter
    {
        public int Id { get; }
        public int Ac { get; }
        public int Province { get; }
        public bool Unautorized { get; } = false;
        public bool Autorized { get; } = false;
        public bool IncludeProvince { get; } = false;
        public bool IncludeAC { get; } = false;
        public string DNI_NIE { get; }
        public string? Password { get; }

        public UserFilter() { }

        public UserFilter(int id, int ac, int province, bool unautorized, bool includeProvince, bool includeAC, bool autorized, string dni_nie, string password)
        {
            Id = id;
            Ac = ac;
            Province = province;
            Unautorized = unautorized;
            IncludeProvince = includeProvince;
            IncludeAC = includeAC;
            Autorized = autorized;
            DNI_NIE = dni_nie;
            Password = password;
        }

        // Do not filter
        public static UserFilter All = new UserFilter();

        public static UserFilter ById(int id) => All.AndById(id);
        public static UserFilter ByAc(int tblAc_UID) => All.AndByAc(tblAc_UID);
        public static UserFilter ByProvince(int tblProvince_UID) => All.AndByProvince(tblProvince_UID);
        public static UserFilter ByUnautorized() => All.AndByUnautorized();
        public static UserFilter ByAutorized() => All.AndByAutorized();
        public static UserFilter ByIncludeProvince() => All.AndByIncludeProvince();
        public static UserFilter ByIncludeAC() => All.AndByIncludeAC();
        public static UserFilter ByDNI_NIE(string dni_nie) => All.AndByDNI_NIE(dni_nie);
        public static UserFilter ByDNI_NIEAndPassword(string dni_nie, string password) => All.AndByDNI_NIEAndPassword(dni_nie, password);

        public UserFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new UserFilter(id, Ac, Province, Unautorized, IncludeProvince, IncludeAC, Autorized, DNI_NIE, Password);
        }
        public UserFilter AndByAc(int tblAc_UID)
        {
            if (tblAc_UID == default(int)) return this;
            return new UserFilter(Id, tblAc_UID, Province, Unautorized, IncludeProvince, IncludeAC, Autorized, DNI_NIE, Password);
        }
        public UserFilter AndByProvince(int tblProvince_UID)
        {
            if (tblProvince_UID == default(int)) return this;
            return new UserFilter(Id, Ac, tblProvince_UID, Unautorized, IncludeProvince, IncludeAC, Autorized, DNI_NIE, Password);
        }

        public UserFilter AndByUnautorized()
        {
            return new UserFilter(Id, Ac, Province, true, IncludeProvince, IncludeAC, Autorized, DNI_NIE, Password);
        }
        public UserFilter AndByIncludeProvince()
        {
            return new UserFilter(Id, Ac, Province, Unautorized, true, IncludeAC, Autorized, DNI_NIE, Password);
        }
        public UserFilter AndByIncludeAC()
        {
            return new UserFilter(Id, Ac, Province, Unautorized, IncludeProvince, true, Autorized, DNI_NIE, Password);
        }
        public UserFilter AndByAutorized()
        {
            return new UserFilter(Id, Ac, Province, Unautorized, IncludeProvince, IncludeAC, true, DNI_NIE, Password);
        }
        public UserFilter AndByDNI_NIE(string dni_nie)
        {
            return new UserFilter(Id, Ac, Province, Unautorized, IncludeProvince, IncludeAC, Autorized, dni_nie, Password);
        }
        public UserFilter AndByDNI_NIEAndPassword(string dni_nie, string password)
        {
            if (dni_nie == default(string) || password == default(string)) return this;
            return new UserFilter(Id, Ac, Province, Unautorized, IncludeProvince, IncludeAC, Autorized, dni_nie, password);
        }

        public bool Equals(UserFilter other)
        {
            return Id.Equals(other.Id) && Ac.Equals(other.Ac) && Province.Equals(other.Province) && Unautorized == other.Unautorized 
                && IncludeProvince == other.IncludeProvince && IncludeAC == other.IncludeAC
                && Autorized == other.Autorized && string.Equals(DNI_NIE, other.DNI_NIE) && string.Equals(Password, other.Password);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UserFilter && Equals((UserFilter)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCode.Combine(Id.GetHashCode(), Ac.GetHashCode(), Province.GetHashCode(), Unautorized.GetHashCode(), IncludeProvince.GetHashCode(), IncludeAC.GetHashCode(),
                    Autorized.GetHashCode(), DNI_NIE.GetHashCode());
            }
        }
    }
}
