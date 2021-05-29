using FluentValidation;

namespace OV.MainDb.Organizer.Create
{
    public interface ICandidateOrganizerValidator : ICandidateOrganizerValidatorBase
    {

    }
    public class CandidateOrganizerValidator : CandidateOrganizerValidatorBase, ICandidateOrganizerValidator
    {
        public CandidateOrganizerValidator()
        {
            RuleFor(candidate => candidate.tblUser_UID)
                   .NotEmpty()
                   .WithErrorCode(OrganizerFailureReason.tblUser_UIDIsEmpty.ToString());

            RuleFor(candidate => candidate.tblElection_UID)
                .NotEmpty()
                .WithErrorCode(OrganizerFailureReason.tblElection_UIDIsEmpty.ToString());

            RuleFor(candidate => candidate.ReferenceNumber)
                .NotEmpty()
                .WithErrorCode(OrganizerFailureReason.ReferenceNumberIsEmpty.ToString());
        }
    }
}
