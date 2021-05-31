using OV.Models.Response;
using OV.Models.Validation;

namespace OV.MainDb.Election.Modify.Models.Public
{
    public interface IModifyElectionResponse : IResponseObject
    {

    }

    public class ModifyElectionSuccess : ResponseSuccess<OV.Models.MainDb.Election.Election>, IModifyElectionResponse
    {
        public ModifyElectionSuccess(OV.Models.MainDb.Election.Election Election) : base(Election) { }
    }

    public class ModifyElectionFailure : ResponseFailure<ElectionFailureReason>, IModifyElectionResponse
    {
        public ModifyElectionFailure(params FailureReason<ElectionFailureReason>[] failureReasons) : base(failureReasons) { }
    }
}
