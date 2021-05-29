using OV.Models.Response;
using OV.Models.Validation;

namespace OV.MainDb.Organizer.Create.Models.Public
{
    public interface ICreateOrganizerResponse : IResponseObject
    {

    }
    public class CreateOrganizerSuccess : ResponseSuccess<OV.Models.MainDb.Organizer.Organizer>, ICreateOrganizerResponse
    {
        public CreateOrganizerSuccess(OV.Models.MainDb.Organizer.Organizer organizer) : base(organizer) { }
    }

    public class CreateOrganizerFailure : ResponseFailure<OrganizerFailureReason>, ICreateOrganizerResponse
    {
        public CreateOrganizerFailure(params FailureReason<OrganizerFailureReason>[] failureReasons) : base(failureReasons) { }
    }
}
