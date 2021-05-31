using FluentValidation;
using OV.MainDb.Option.Modify.Models.Public;

namespace OV.MainDb.Option.Modify.Models
{
    public interface IModifyCandidateValidatorBase : IValidator<ModifyOptionCandidate>
    {

    }
    public class ModifyCandidateValidatorBase : AbstractValidator<ModifyOptionCandidate>, IModifyCandidateValidatorBase
    {
        protected ModifyCandidateValidatorBase() { }
    }
}
