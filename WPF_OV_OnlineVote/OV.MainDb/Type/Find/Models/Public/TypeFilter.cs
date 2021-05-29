using System;

namespace OV.MainDb.Type.Find.Models.Public
{
    public class TypeFilter
    {        
        public string Code { get; } 
        public TypeFilter() { }
        public TypeFilter(string code) 
        {
            Code = code;
        }


        // Do not filter
        public static TypeFilter All = new TypeFilter();
        public static TypeFilter ByCode(string code) => All.AndByCode(code);


        public TypeFilter AndByCode(string code)
        {
            return new TypeFilter(code);
        }

        public bool Equals(TypeFilter other)
        {
            return string.Equals(Code, other.Code, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypeFilter && Equals((TypeFilter)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return HashCode.Combine(Code.GetHashCode());
            }
        }
    }
}
