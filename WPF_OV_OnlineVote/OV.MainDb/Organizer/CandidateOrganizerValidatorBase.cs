using FluentValidation;
using OV.MainDb.Organizer.Models.Public;

namespace OV.MainDb.Organizer
{
    public interface ICandidateOrganizerValidatorBase : IValidator<CandidateOrganizer>
    {

    }

    public class CandidateOrganizerValidatorBase : AbstractValidator<CandidateOrganizer>, ICandidateOrganizerValidatorBase
    {
        protected CandidateOrganizerValidatorBase() { }
    }
}
