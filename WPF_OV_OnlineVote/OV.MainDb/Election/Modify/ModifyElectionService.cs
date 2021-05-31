using OV.MainDb.Election.Modify.Models.Public;
using OV.Models.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Election.Modify
{
    public interface IModifyElectionService
    {
        Task<IModifyElectionResponse> ModifyAsync(ModifyElectionCandidate candidate, CancellationToken cancellationToken);
    }
    public class ModifyElectionService : IModifyElectionService
    {

        private IModifyElectionDataService _modifyElectionDataService;
        private IModifyElectionValidator _validator;

        public ModifyElectionService(IModifyElectionDataService modifyElectionDataService, IModifyElectionValidator validator)
        {
            _modifyElectionDataService = modifyElectionDataService ?? throw new ArgumentNullException(nameof(modifyElectionDataService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<IModifyElectionResponse> ModifyAsync(ModifyElectionCandidate candidate, CancellationToken cancellationToken)
        {
            try
            {
                var validatorResult = await _validator.ValidateAsync(candidate);
                if (!validatorResult.IsValid)
                {
                    return new ModifyElectionFailure(validatorResult.Errors.ParseFailures<ElectionFailureReason>());
                }

                var modifiedElection = await _modifyElectionDataService.ModifyAsync(candidate, cancellationToken);

                if (modifiedElection != null)
                {
                    return new ModifyElectionSuccess(modifiedElection.ToElection());
                }
                return new ModifyElectionFailure(new FailureReason<ElectionFailureReason>(ElectionFailureReason.FailureInsertingIntoDataBase));
            }
            catch (Exception e)
            {
                return new ModifyElectionFailure(new FailureReason<ElectionFailureReason>(ElectionFailureReason.FailureInsertingIntoDataBase));
            }
        }
    }
}
