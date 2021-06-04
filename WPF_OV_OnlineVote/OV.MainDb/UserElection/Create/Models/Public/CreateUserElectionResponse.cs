using OV.Models.Response;
using OV.Models.Validation;

namespace OV.MainDb.UserElection.Create.Models.Public
{
    public interface ICreateUserElectionResponse : IResponseObject
    {

    }
    public class CreateUserElectionSuccess : ResponseSuccess<OV.Models.MainDb.UserElection.UserElection>, ICreateUserElectionResponse
    {
        public CreateUserElectionSuccess(OV.Models.MainDb.UserElection.UserElection userElection) : base(userElection) { }
    }

    public class CreateUserElectionFailure : ResponseFailure<UserElectionFailureReason>, ICreateUserElectionResponse
    {
        public CreateUserElectionFailure(params FailureReason<UserElectionFailureReason>[] failureReasons) : base(failureReasons) { }
    }
}
