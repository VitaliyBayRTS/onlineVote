using System;

namespace OV.MainDb.Habitant.Find.Models.Public
{
    public class HabitantFilter
    {
        public int Id { get; }
        public string? DNI_NIE { get; }
        public string? Password { get; }
        public bool UserIncluded { get; } = false;

        public HabitantFilter() { }

        public HabitantFilter(int id, string? dni_nie, string? password,  bool userIncluded)
        {
            Id = id;
            DNI_NIE = dni_nie;
            Password = password;
            UserIncluded = userIncluded;
        }

        // Do not filter
        public static HabitantFilter All = new HabitantFilter();

        public static HabitantFilter ById(int id) => All.AndById(id);
        public static HabitantFilter ByDNI_NIE(string dni_nie) => All.AndByDNI_NIE(dni_nie);
        public static HabitantFilter ByDNI_NIEAndPassword(string dni_nie, string password) => All.AndByDNI_NIEAndPassword(dni_nie, password);
        public static HabitantFilter IncludeUser() => All.AndIncludeUser();


        public HabitantFilter AndById(int id)
        {
            if (id == default(int)) return this;
            return new HabitantFilter(id, DNI_NIE, Password, UserIncluded);
        }
        public HabitantFilter AndByDNI_NIE(string dni_nie)
        {
            if (dni_nie == default(string)) return this;
            return new HabitantFilter(Id, dni_nie, Password, UserIncluded);
        }
        public HabitantFilter AndByDNI_NIEAndPassword(string dni_nie, string password)
        {
            if (dni_nie == default(string) || password == default(string)) return this;
            return new HabitantFilter(Id, dni_nie, password, UserIncluded);
        }
        public HabitantFilter AndIncludeUser()
        {
            return new HabitantFilter(Id, DNI_NIE, Password, true);
        }

        public bool Equals(HabitantFilter other)
        {
            return Id.Equals(other.Id) && (DNI_NIE?.Equals(other.DNI_NIE) ?? (other.DNI_NIE == null)) && (Password ?.Equals(other.Password) ?? (other.Password == null))
                        && UserIncluded == other.UserIncluded;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is HabitantFilter && Equals((HabitantFilter)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCode.Combine(Id.GetHashCode(), DNI_NIE.GetHashCode(), Password.GetHashCode(), UserIncluded.GetHashCode());
            }
        }
    }
}
