using OV.MainDb.Option.Modify.Models.Public;
using OV.Models.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Option.Modify
{
    public interface IModifyOptionService
    {
        Task<IModifyOptionResponse> ModifyAsync(ModifyOptionCandidate candidate, CancellationToken cancellationToken);
    }
    public class ModifyOptionService : IModifyOptionService
    {

        private IModifyOptionDataService _modifyOptionDataService;
        private IModifyOptionValidator _validator;

        public ModifyOptionService(IModifyOptionDataService modifyOptionDataService, IModifyOptionValidator validator)
        {
            _modifyOptionDataService = modifyOptionDataService ?? throw new ArgumentNullException(nameof(modifyOptionDataService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<IModifyOptionResponse> ModifyAsync(ModifyOptionCandidate candidate, CancellationToken cancellationToken)
        {
            try
            {
                var validatorResult = await _validator.ValidateAsync(candidate);
                if (!validatorResult.IsValid)
                {
                    return new ModifyOptionFailure(validatorResult.Errors.ParseFailures<OptionFailureReason>());
                }

                var modifiedOption = await _modifyOptionDataService.ModifyAsync(candidate, cancellationToken);

                if (modifiedOption != null)
                {
                    return new ModifyOptionSuccess(modifiedOption.ToOption());
                }
                return new ModifyOptionFailure(new FailureReason<OptionFailureReason>(OptionFailureReason.FailureInsertingIntoDataBase));
            }
            catch (Exception e)
            {
                return new ModifyOptionFailure(new FailureReason<OptionFailureReason>(OptionFailureReason.FailureInsertingIntoDataBase));
            }
        }
    }
}
