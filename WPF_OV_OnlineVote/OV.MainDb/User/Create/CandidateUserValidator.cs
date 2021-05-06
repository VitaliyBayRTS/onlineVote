using FluentValidation;

namespace OV.MainDb.User.Create
{
    public interface ICandidateUserValidator : ICandidateUserValidatorBase
    {

    }
    public class CandidateUserValidator : CandidateUserValidatorBase, ICandidateUserValidator
    {
        public CandidateUserValidator()
        {
            RuleFor(candodate => candodate.FirstName)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.FirstNameIsEmpty.ToString());

            RuleFor(candodate => candodate.SurName)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.FirstSurNameIsEmpty.ToString());
        }
    }
}
