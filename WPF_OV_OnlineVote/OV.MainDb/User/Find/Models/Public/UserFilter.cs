using System;

namespace OV.MainDb.User.Find.Models.Public
{
    class UserFilter
    {
        public int Id { get; }

        public UserFilter() { }

        public UserFilter(int id)
        {
            Id = id;
        }

        // Do not filter
        public static UserFilter All = new UserFilter();

        public static UserFilter ById(int id) => All.AndById(id);


        public UserFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new UserFilter(id);
        }

        public bool Equals(UserFilter other)
        {
            return Id.Equals(other.Id);
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
