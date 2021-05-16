using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OV.Models.Validation
{
    public static class ExtensionsForIEnumerableOfValidationFailure
    {

        public static T[] ParseFailureReasons<T>(this IEnumerable<ValidationFailure> validationFailures) where T : struct
        {
            return validationFailures
                .Select(failure => failure.ErrorCode)
                .Select(code => Enum.TryParse(code, true, out T result) ? (T?)result : (T?)null)
                .Where(result => result != null)
                .Select(result => result!.Value)
                .ToArray();
        }

        public static FailureReason<TReason>[] ParseFailures<TReason>(this IEnumerable<ValidationFailure> validationFailures)
            where TReason : struct
        {
            return validationFailures.Select(v => new FailureReason<TReason>(v)).ToArray();
        } 

    }
}
