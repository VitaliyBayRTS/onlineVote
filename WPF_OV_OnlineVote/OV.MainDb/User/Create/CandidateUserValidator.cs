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
            RuleFor(candidate => candidate.FirstName)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.FirstNameIsEmpty.ToString());

            RuleFor(candidate => candidate.SurName)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.FirstSurNameIsEmpty.ToString());

            RuleFor(candidate => candidate.Password)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.PasswordIsEmpty.ToString());

            RuleFor(candidate => candidate.DOB)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.DateOfBirthIsEmpty.ToString());

            RuleFor(candidate => candidate.TblAutonomousCommunities_UID)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.AutonomousCommunityIsEmpty.ToString());

            RuleFor(candidate => candidate.TblProvince_UID)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.ProvinceIsEmpty.ToString());

            RuleFor(candidate => candidate.Email)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.EmailIsEmpty.ToString());

            RuleFor(candidate => candidate.PhoneNumber)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.PhoneNumberIsEmpty.ToString());

            //TODO: Check if TblAutonomousCommunity_UID exist in DB
            //TODO: Check if TblProvince_UID exist in DB
        }
    }
}
