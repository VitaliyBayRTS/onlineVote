using FluentValidation;
using OV.MainDb.Option.Models.Public;

namespace OV.MainDb.Option
{
    public interface ICandidateOptionValidatorBase : IValidator<CandidateOption>
    {

    }
    public class CandidateOptionValidatorBase : AbstractValidator<CandidateOption>, ICandidateOptionValidatorBase
    {
        protected CandidateOptionValidatorBase() { }
    }
}
