using System;

namespace OV.MainDb.User.Find.Models.Public
{
    public class UserFilter
    {
        public int Id { get; }
        public bool Unautorized { get; } = false;
        public bool Autorized { get; } = false;
        public bool IncludeProvince { get; } = false;
        public bool IncludeAC { get; } = false;

        public UserFilter() { }

        public UserFilter(int id, bool unautorized, bool includeProvince, bool includeAC, bool autorized)
        {
            Id = id;
            Unautorized = unautorized;
            IncludeProvince = includeProvince;
            IncludeAC = includeAC;
            Autorized = autorized;
        }

        // Do not filter
        public static UserFilter All = new UserFilter();

        public static UserFilter ById(int id) => All.AndById(id);
        public static UserFilter ByUnautorized() => All.AndByUnautorized();
        public static UserFilter ByAutorized() => All.AndByAutorized();
        public static UserFilter ByIncludeProvince() => All.AndByIncludeProvince();
        public static UserFilter ByIncludeAC() => All.AndByIncludeAC();

        public UserFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new UserFilter(id, Unautorized, IncludeProvince, IncludeAC, Autorized);
        }

        public UserFilter AndByUnautorized()
        {
            return new UserFilter(Id, true, IncludeProvince, IncludeAC, Autorized);
        }
        public UserFilter AndByIncludeProvince()
        {
            return new UserFilter(Id, Unautorized, true, IncludeAC, Autorized);
        }
        public UserFilter AndByIncludeAC()
        {
            return new UserFilter(Id, Unautorized, IncludeProvince, true, Autorized);
        }
        public UserFilter AndByAutorized()
        {
            return new UserFilter(Id, Unautorized, IncludeProvince, IncludeAC, true);
        }

        public bool Equals(UserFilter other)
        {
            return Id.Equals(other.Id) && Unautorized == other.Unautorized 
                && IncludeProvince == other.IncludeProvince && IncludeAC == other.IncludeAC
                && Autorized == other.Autorized;
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
                return HashCode.Combine(Id.GetHashCode(), Unautorized.GetHashCode(), IncludeProvince.GetHashCode(), IncludeAC.GetHashCode(),
                    Autorized.GetHashCode());
            }
        }
    }
}
