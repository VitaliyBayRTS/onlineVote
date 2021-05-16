using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace OV.Models.Validation
{
    public class FailureReason<TCode> where TCode: struct
    {
        public TCode? Code { get; }
        public string? PropertyName { get; }
        
        public FailureReason(TCode code)
        {
            Code = code;
        }

        public FailureReason(string propertyName, TCode code)
        {
            PropertyName = propertyName;
            Code = code;
        }

        public FailureReason(ValidationFailure validationFailure)
        {
            if (validationFailure == null) return;
            PropertyName = validationFailure.PropertyName;
            Code = Enum.TryParse(validationFailure.ErrorCode, true, out TCode code) ? code : default(TCode);
        }

        public bool Equals(FailureReason<TCode> other)
        {
            return string.Equals(PropertyName, other.PropertyName) && Code.Equals(other.Code);
        }
        public bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((FailureReason<TCode>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = PropertyName != null ? PropertyName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ Code.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{PropertyName}: {Code.ToString()}";
        }

    }
}
