using FluentValidation;
using OV.MainDb.Election.Modify.Models;

namespace OV.MainDb.Election.Modify
{
    public interface IModifyElectionValidator : IModifyCandidateValidationBase
    {

    }
    public class ModifyElectionValidator : ModifyCandidateValidationBase, IModifyElectionValidator
    {
        public ModifyElectionValidator()
        {
            RuleFor(candidate => candidate.Id)
                .NotEmpty()
                .WithErrorCode(ElectionFailureReason.IdIsEmpty.ToString());

            RuleFor(candidate => candidate.Name)
                .NotEmpty()
                .WithErrorCode(ElectionFailureReason.NameIsEmpty.ToString());

            RuleFor(candidate => candidate.Description)
                .NotEmpty()
                .WithErrorCode(ElectionFailureReason.Description.ToString());

            RuleFor(candidate => candidate.InitDateTime)
                .NotEmpty()
                .WithErrorCode(ElectionFailureReason.InitDateIsEmpty.ToString());

            RuleFor(candidate => candidate.FinalizeDateTime)
                .NotEmpty()
                .WithErrorCode(ElectionFailureReason.FinishDateIsEmpty.ToString());
        }
    }
}
