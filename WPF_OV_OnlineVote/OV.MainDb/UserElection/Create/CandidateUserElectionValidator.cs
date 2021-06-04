using FluentValidation;

namespace OV.MainDb.UserElection.Create
{
    public interface ICandidateUserElectionValidator : ICandidateUserElectionValidatorBase
    {

    }
    public class CandidateUserElectionValidator : CandidateUserElectionValidatorBase, ICandidateUserElectionValidator
    {
        public CandidateUserElectionValidator()
        {

            RuleFor(candidate => candidate.TblUser_UID)
                .NotEmpty()
                .WithErrorCode(UserElectionFailureReason.tblUser_UIDIsEmpty.ToString());

            RuleFor(candidate => candidate.TblElection_UID)
                .NotEmpty()
                .WithErrorCode(UserElectionFailureReason.tblElection_UIDIsEmpty.ToString());
        }
    }
}
