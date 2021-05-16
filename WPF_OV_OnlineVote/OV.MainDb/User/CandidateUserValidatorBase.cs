using FluentValidation;
using OV.MainDb.User.Models.Public;

namespace OV.MainDb.User
{
    public interface ICandidateUserValidatorBase : IValidator<CandidateUser>
    {

    }

    public class CandidateUserValidatorBase : AbstractValidator<CandidateUser>, ICandidateUserValidatorBase
    {
        protected CandidateUserValidatorBase() { }
    }
}
