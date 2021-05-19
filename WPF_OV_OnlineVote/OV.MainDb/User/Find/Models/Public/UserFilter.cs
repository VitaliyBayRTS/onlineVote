using System;

namespace OV.MainDb.User.Find.Models.Public
{
    public class UserFilter
    {
        public int Id { get; }
        public bool Unautorized { get; } = false;

        public UserFilter() { }

        public UserFilter(int id, bool unautorized)
        {
            Id = id;
            Unautorized = unautorized;
        }

        // Do not filter
        public static UserFilter All = new UserFilter();

        public static UserFilter ById(int id) => All.AndById(id);
        public static UserFilter ByUnautorized() => All.AndByUnautorized();

        public UserFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new UserFilter(id, Unautorized);
        }

        public UserFilter AndByUnautorized()
        {
            return new UserFilter(Id, true);
        }

        public bool Equals(UserFilter other)
        {
            return Id.Equals(other.Id) && Unautorized == other.Unautorized;
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
                return HashCode.Combine(Id.GetHashCode());
            }
        }
    }
}
