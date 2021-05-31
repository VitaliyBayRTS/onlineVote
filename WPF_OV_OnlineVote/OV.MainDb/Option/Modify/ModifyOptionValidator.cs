using FluentValidation;
using OV.MainDb.Option.Modify.Models;

namespace OV.MainDb.Option.Modify
{
    public interface IModifyOptionValidator : IModifyCandidateValidatorBase
    {

    }
    public class ModifyOptionValidator : ModifyCandidateValidatorBase, IModifyOptionValidator
    {
        public ModifyOptionValidator()
        {
            RuleFor(candidate => candidate.Id)
                .NotEmpty()
                .WithErrorCode(OptionFailureReason.IdIsEmpty.ToString());

            RuleFor(candidate => candidate.Name)
                .NotEmpty()
                .WithErrorCode(OptionFailureReason.NameIsEmpty.ToString());

            RuleFor(candidate => candidate.Description)
                .NotEmpty()
                .WithErrorCode(OptionFailureReason.DescriptionIsEmpty.ToString());
        }
    }
}
