using OV.Models.Response;
using OV.Models.Validation;

namespace OV.MainDb.Option.Create.Models.Public
{
    public interface ICreateOptionResponse : IResponseObject
    {
    }
    public class CreateOptionSuccess : ResponseSuccess<OV.Models.MainDb.Option.Option>, ICreateOptionResponse
    {
        public CreateOptionSuccess(OV.Models.MainDb.Option.Option Option) : base(Option) { }
    }

    public class CreateOptionFailure : ResponseFailure<OptionFailureReason>, ICreateOptionResponse
    {
        public CreateOptionFailure(params FailureReason<OptionFailureReason>[] failureReasons) : base(failureReasons) { }
    }
}
