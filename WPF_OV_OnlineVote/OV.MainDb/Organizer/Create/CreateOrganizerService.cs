using OV.MainDb.Organizer.Create.Models.Public;
using OV.MainDb.Organizer.Models.Public;
using OV.Models.Validation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Organizer.Create
{
    public interface ICreateOrganizerService
    {
        Task<ICreateOrganizerResponse> CreateAsync(CandidateOrganizer candidate, CancellationToken cancellationToken);
        Task<bool> CreateRangeAsync(List<CandidateOrganizer> candidates, CancellationToken cancellationToken);
    }
    public class CreateOrganizerService : ICreateOrganizerService
    {

        private ICreateOrganizerDataService _createOrganizerDataService;
        private ICandidateOrganizerValidator _validator;

        public CreateOrganizerService(ICreateOrganizerDataService createOrganizerDataService, ICandidateOrganizerValidator validator)
        {
            _createOrganizerDataService = createOrganizerDataService ?? throw new ArgumentNullException(nameof(createOrganizerDataService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<ICreateOrganizerResponse> CreateAsync(CandidateOrganizer candidate, CancellationToken cancellationToken)
        {
            try
            {
                var validatorResult = await _validator.ValidateAsync(candidate);
                if (!validatorResult.IsValid)
                {
                    return new CreateOrganizerFailure(validatorResult.Errors.ParseFailures<OrganizerFailureReason>());
                }

                var insertedorganizer = await _createOrganizerDataService.CreateAsync(candidate, cancellationToken);

                if (insertedorganizer != null)
                {
                    return new CreateOrganizerSuccess(insertedorganizer.ToOrganizer());
                }
                return new CreateOrganizerFailure(new FailureReason<OrganizerFailureReason>(OrganizerFailureReason.FailureInsertingIntoDataBase));
            }
            catch (Exception e)
            {
                return new CreateOrganizerFailure(new FailureReason<OrganizerFailureReason>(OrganizerFailureReason.FailureInsertingIntoDataBase));
            }
        }

        public async Task<bool> CreateRangeAsync(List<CandidateOrganizer> candidates, CancellationToken cancellationToken)
        {
            try
            {
                return await _createOrganizerDataService.CreateRangeAsync(candidates, cancellationToken);
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
