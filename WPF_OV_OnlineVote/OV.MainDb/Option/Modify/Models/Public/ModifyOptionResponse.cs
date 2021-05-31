using OV.Models.Response;
using OV.Models.Validation;

namespace OV.MainDb.Option.Modify.Models.Public
{
    public interface IModifyOptionResponse : IResponseObject
    {
    }

    public class ModifyOptionSuccess : ResponseSuccess<OV.Models.MainDb.Option.Option>, IModifyOptionResponse
    {
        public ModifyOptionSuccess(OV.Models.MainDb.Option.Option option) : base(option) { }
    }

    public class ModifyOptionFailure : ResponseFailure<OptionFailureReason>, IModifyOptionResponse
    {
        public ModifyOptionFailure(params FailureReason<OptionFailureReason>[] failureReasons) : base(failureReasons) { }
    }
}
