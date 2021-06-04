using FluentValidation;
using OV.MainDb.UserElection.Models.Public;

namespace OV.MainDb.UserElection
{
    public interface ICandidateUserElectionValidatorBase : IValidator<CandidateUserElection>
    {

    }
    public class CandidateUserElectionValidatorBase : AbstractValidator<CandidateUserElection>, ICandidateUserElectionValidatorBase
    {
        protected CandidateUserElectionValidatorBase() { }
    }
}
