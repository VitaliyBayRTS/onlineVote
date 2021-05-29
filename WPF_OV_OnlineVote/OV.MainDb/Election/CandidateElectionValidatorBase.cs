using FluentValidation;
using OV.MainDb.Election.Models.Public;

namespace OV.MainDb.Election
{
    public interface ICandidateElectionValidatorBase : IValidator<CandidateElection>
    {

    }

    public class CandidateElectionValidatorBase : AbstractValidator<CandidateElection>, ICandidateElectionValidatorBase
    {
        protected CandidateElectionValidatorBase() { }
    }
}
