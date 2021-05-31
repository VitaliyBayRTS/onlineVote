using FluentValidation;
using OV.MainDb.Election.Modify.Models.Public;

namespace OV.MainDb.Election.Modify.Models
{
    public interface IModifyCandidateValidationBase : IValidator<ModifyElectionCandidate>
    {

    }
    public class ModifyCandidateValidationBase : AbstractValidator<ModifyElectionCandidate>, IModifyCandidateValidationBase
    {
        protected ModifyCandidateValidationBase() { }
    }
}
