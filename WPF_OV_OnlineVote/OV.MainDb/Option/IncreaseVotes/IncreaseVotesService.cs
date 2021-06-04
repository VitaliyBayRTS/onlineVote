using OV.MainDb.Option.IncreaseVotes.Models.Public;
using OV.MainDb.UserElection.Create;
using OV.MainDb.UserElection.Models.Public;
using OV.Models.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Option.IncreaseVotes
{
    public interface IIncreaseVotesService
    {
        Task<IIncreaseVotesResponse> IncreaseVote(IncreaseVotesRequest request, CancellationToken cancellationToken);
    }
    public class IncreaseVotesService : IIncreaseVotesService
    {
        private readonly IIncreaseVotesDataService _increaseVoteDataService;
        private IIncreaseVotesValidator _validator;

        public IncreaseVotesService(
            IIncreaseVotesDataService increaseVoteDataService,
            IIncreaseVotesValidator validator)
        {
            _increaseVoteDataService = increaseVoteDataService ??
                                                  throw new ArgumentNullException(nameof(increaseVoteDataService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<IIncreaseVotesResponse> IncreaseVote(IncreaseVotesRequest request, CancellationToken cancellationToken)
        {
            var transaction = _increaseVoteDataService.BeginTransaction();
            try
            {
                var validatorResult = await _validator.ValidateAsync(request);
                if (!validatorResult.IsValid)
                {
                    return new IncreaseVotesFailure(validatorResult.Errors.ParseFailures<OptionFailureReason>());
                }

                var createUserElectionDataService = new CreateUserElectionDataService(_increaseVoteDataService.getOvMainDbContext());
                var result = await createUserElectionDataService.CreateAsync(
                    new CandidateUserElection() {
                        TblUser_UID = request.TblUser_UID,
                        TblElection_UID = request.TblElection_UID 
                    },
                    cancellationToken
                );

                var isSuccesfulyIncreased = false;

                if (result != null)
                {
                    isSuccesfulyIncreased = await _increaseVoteDataService.IncreaseVote(request.TblOption_UID, cancellationToken);
                } 
                else
                {
                    _increaseVoteDataService.RollBackTransaction(transaction);
                    return new IncreaseVotesFailure(new FailureReason<OptionFailureReason>(OptionFailureReason.FailureIncreasingVotes));
                }

                _increaseVoteDataService.CommitTransaction(transaction);
                return new IncreaseVotesSuccess(isSuccesfulyIncreased);
            }
            catch (Exception)
            {
                _increaseVoteDataService.RollBackTransaction(transaction);
                return new IncreaseVotesFailure(new FailureReason<OptionFailureReason>(OptionFailureReason.FailureIncreasingVotes));
            }
        }
    }
}
