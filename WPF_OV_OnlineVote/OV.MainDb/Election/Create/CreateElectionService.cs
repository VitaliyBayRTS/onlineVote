using OV.MainDb.Election.Create.Models.Public;
using OV.MainDb.Election.Models.Public;
using OV.Models.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.Create
{
    public interface ICreateElectionService
    {
        Task<ICreateElectionResponse> CreateAsync(CandidateElection candidate, CancellationToken cancellationToken);
    }
    public class CreateElectionService : ICreateElectionService
    {

        private ICreateElectionDataService _createElectionDataService;
        private ICandidateElectionValidator _validator;

        public CreateElectionService(ICreateElectionDataService createElectionDataService, ICandidateElectionValidator validator)
        {
            _createElectionDataService = createElectionDataService ?? throw new ArgumentNullException(nameof(createElectionDataService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<ICreateElectionResponse> CreateAsync(CandidateElection candidate, CancellationToken cancellationToken)
        {
            try
            {
                var validatorResult = await _validator.ValidateAsync(candidate);
                if (!validatorResult.IsValid)
                {
                    return new CreateElectionFailure(validatorResult.Errors.ParseFailures<ElectionFailureReason>());
                }

                var insertedElection = await _createElectionDataService.CreateAsync(candidate, cancellationToken);

                if(insertedElection != null)
                {
                    return new CreateElectionSuccess(insertedElection.ToElection());
                }
                return new CreateElectionFailure(new FailureReason<ElectionFailureReason>(ElectionFailureReason.FailureInsertingIntoDataBase));
            }
            catch (Exception e)
            {
                return new CreateElectionFailure(new FailureReason<ElectionFailureReason>(ElectionFailureReason.FailureInsertingIntoDataBase));
            }
        }
    }
}
