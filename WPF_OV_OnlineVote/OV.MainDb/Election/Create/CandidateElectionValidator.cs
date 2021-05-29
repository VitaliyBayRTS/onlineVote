using FluentValidation;

namespace OV.MainDb.Election.Create
{
    public interface ICandidateElectionValidator : ICandidateElectionValidatorBase
    {
    }

    public class CandidateElectionValidator : CandidateElectionValidatorBase, ICandidateElectionValidator
    {
        public CandidateElectionValidator()
        {
            RuleFor(candidate => candidate.Name)
                .NotEmpty()
                .WithErrorCode(ElectionFailureReason.NameIsEmpty.ToString());

            RuleFor(candidate => candidate.Description)
                .NotEmpty()
                .WithErrorCode(ElectionFailureReason.Description.ToString());

            RuleFor(candidate => candidate.InitDate)
                .NotEmpty()
                .WithErrorCode(ElectionFailureReason.InitDateIsEmpty.ToString());

            RuleFor(candidate => candidate.FinalizeDate)
                .NotEmpty()
                .WithErrorCode(ElectionFailureReason.FinishDateIsEmpty.ToString());

            RuleFor(candidate => candidate.tblType_UID)
                .NotEmpty()
                .WithErrorCode(ElectionFailureReason.TypeIsEmpty.ToString());

        }
    }
}
