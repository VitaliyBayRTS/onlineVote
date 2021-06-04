using OV.MainDb.UserElection.Create.Models.Public;
using OV.MainDb.UserElection.Models.Public;
using OV.Models.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.UserElection.Create
{
    public interface ICreateUserElectionService
    {
        Task<ICreateUserElectionResponse> CreateAsync(CandidateUserElection candidate, CancellationToken cancellationToken);
    }
    public class CreateUserElectionService : ICreateUserElectionService
    {
        private ICreateUserElectionDataService _createUserElectionDataService;
        private ICandidateUserElectionValidator _validator;

        public CreateUserElectionService(ICreateUserElectionDataService createUserElectionDataService, ICandidateUserElectionValidator validator)
        {
            _createUserElectionDataService = createUserElectionDataService ?? throw new ArgumentNullException(nameof(createUserElectionDataService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<ICreateUserElectionResponse> CreateAsync(CandidateUserElection candidate, CancellationToken cancellationToken)
        {
            try
            {
                var validatorResult = await _validator.ValidateAsync(candidate);
                if (!validatorResult.IsValid)
                {
                    return new CreateUserElectionFailure(validatorResult.Errors.ParseFailures<UserElectionFailureReason>());
                }


                var insertedUserElection = await _createUserElectionDataService.CreateAsync(candidate, cancellationToken);

                if (insertedUserElection != null)
                {
                    return new CreateUserElectionSuccess(insertedUserElection.ToUserElection());
                }
                return new CreateUserElectionFailure(new FailureReason<UserElectionFailureReason>(UserElectionFailureReason.FailureInsertingIntoDataBase));
            }
            catch (Exception)
            {
                return new CreateUserElectionFailure(new FailureReason<UserElectionFailureReason>(UserElectionFailureReason.FailureInsertingIntoDataBase));
            }
        }
    }
}
