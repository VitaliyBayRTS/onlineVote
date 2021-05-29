using OV.Models.Response;
using OV.Models.Validation;

namespace OV.MainDb.User.Create.Models.Public
{
    public interface ICreateUserResponse : IResponseObject
    {

    }

    public class CreateUserSuccess : ResponseSuccess<OV.Models.MainDb.User.User>, ICreateUserResponse
    {
        public CreateUserSuccess(OV.Models.MainDb.User.User user) : base(user) { }
    }

    public class CreateUserFailure : ResponseFailure<UserFailureReason>, ICreateUserResponse
    {
        public CreateUserFailure(params FailureReason<UserFailureReason>[] failureReasons) : base(failureReasons) { }
    }
}
