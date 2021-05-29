using OV.Models.Response;
using OV.Models.Validation;
namespace OV.MainDb.Election.Create.Models.Public
{
    public interface ICreateElectionResponse : IResponseObject
    {

    }

    public class CreateElectionSuccess : ResponseSuccess<OV.Models.MainDb.Election.Election>, ICreateElectionResponse
    {
        public CreateElectionSuccess(OV.Models.MainDb.Election.Election election) : base(election) { }
    }

    public class CreateElectionFailure : ResponseFailure<ElectionFailureReason>, ICreateElectionResponse
    {
        public CreateElectionFailure(params FailureReason<ElectionFailureReason>[] failureReasons) : base(failureReasons) { }
    }
}
