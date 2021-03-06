using FluentValidation;
using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.Province.Find;
using OV.MainDb.Province.Find.Models.Public;
using System;
using System.Linq;

namespace OV.MainDb.User.Create
{
    public interface ICandidateUserValidator : ICandidateUserValidatorBase
    {

    }
    public class CandidateUserValidator : CandidateUserValidatorBase, ICandidateUserValidator
    {
        public CandidateUserValidator(IFindAutonomousCommunityDataService findAutonomousCommunityDataService,
                                    IFindProvinceDataService findProvinceDataService)
        {
            if (findAutonomousCommunityDataService == null) throw new ArgumentNullException(nameof(findAutonomousCommunityDataService));
            if (findProvinceDataService == null) throw new ArgumentNullException(nameof(findProvinceDataService));

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

            RuleFor(candidate => candidate.TblProvince_UID)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.ProvinceIsEmpty.ToString());

            RuleFor(candidate => candidate.TblProvince_UID)
                .MustAsync(async (candidate, tblProvince_UID, cancellationToken) =>
                {
                    var existingProvince = await findProvinceDataService
                                    .FindAsync(ProvinceFilter.ById(candidate.TblProvince_UID), cancellationToken);
                    return existingProvince.Any();
                })
                .WithErrorCode(UserFailureReason.ProvinceDoesNotExist.ToString());

            RuleFor(candidate => candidate.Email)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.EmailIsEmpty.ToString());

            RuleFor(candidate => candidate.PhoneNumber)
                .NotEmpty()
                .WithErrorCode(UserFailureReason.PhoneNumberIsEmpty.ToString());
        }
    }
}
