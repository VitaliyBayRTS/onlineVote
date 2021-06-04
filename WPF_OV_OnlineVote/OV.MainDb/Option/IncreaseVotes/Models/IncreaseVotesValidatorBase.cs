using FluentValidation;
using OV.MainDb.Option.IncreaseVotes.Models.Public;

namespace OV.MainDb.Option.IncreaseVotes.Models
{
    public interface IIncreaseVotesValidatorBase : IValidator<IncreaseVotesRequest>
    {

    }
    public class IncreaseVotesValidatorBase : AbstractValidator<IncreaseVotesRequest>, IIncreaseVotesValidatorBase
    {
        protected IncreaseVotesValidatorBase() { }
    }
}
