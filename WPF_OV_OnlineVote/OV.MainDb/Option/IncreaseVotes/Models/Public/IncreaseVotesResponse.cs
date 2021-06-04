using OV.Models.Response;
using OV.Models.Validation;

namespace OV.MainDb.Option.IncreaseVotes.Models.Public
{
    public interface IIncreaseVotesResponse : IResponseObject
    {

    }

    public class IncreaseVotesSuccess : ResponseSuccess<bool>, IIncreaseVotesResponse
    {
        public IncreaseVotesSuccess(bool success) : base(success) { }
    }

    public class IncreaseVotesFailure : ResponseFailure<OptionFailureReason>, IIncreaseVotesResponse
    {
        public IncreaseVotesFailure(params FailureReason<OptionFailureReason>[] failureReasons) : base(failureReasons) { }
    }
}
