using FluentValidation;

namespace OV.MainDb.Option.Create
{
    public interface ICreateOptionValidator : ICandidateOptionValidatorBase
    {

    }
    public class CreateOptionValidator : CandidateOptionValidatorBase, ICreateOptionValidator
    {
        public CreateOptionValidator()
        {
            RuleFor(candidate => candidate.Name)
                   .NotEmpty()
                   .WithErrorCode(OptionFailureReason.NameIsEmpty.ToString());

            RuleFor(candidate => candidate.tblElection_UID)
                .NotEmpty()
                .WithErrorCode(OptionFailureReason.tblElection_UIDIsEmpty.ToString());

            RuleFor(candidate => candidate.Description)
                .NotEmpty()
                .WithErrorCode(OptionFailureReason.DescriptionIsEmpty.ToString());
        }
    }
}
