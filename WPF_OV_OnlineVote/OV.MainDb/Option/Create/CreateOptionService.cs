using OV.MainDb.Option.Create.Models.Public;
using OV.MainDb.Option.Models.Public;
using OV.Models.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Option.Create
{
    public interface ICreateOptionService
    {
        Task<ICreateOptionResponse> CreateAsync(CandidateOption candidate, CancellationToken cancellationToken);
    }
    public class CreateOptionService : ICreateOptionService
    {
        private ICreateOptionDataService _createOptionDataService;
        private ICreateOptionValidator _validator;

        public CreateOptionService(ICreateOptionDataService createOptionDataService, ICreateOptionValidator validator)
        {
            _createOptionDataService = createOptionDataService ?? throw new ArgumentNullException(nameof(createOptionDataService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task<ICreateOptionResponse> CreateAsync(CandidateOption candidate, CancellationToken cancellationToken)
        {
            try
            {
                var validatorResult = await _validator.ValidateAsync(candidate);
                if (!validatorResult.IsValid)
                {
                    return new CreateOptionFailure(validatorResult.Errors.ParseFailures<OptionFailureReason>());
                }

                var insertedOption = await _createOptionDataService.CreateAsync(candidate, cancellationToken);

                if (insertedOption != null)
                {
                    return new CreateOptionSuccess(insertedOption.ToOption());
                }
                return new CreateOptionFailure(new FailureReason<OptionFailureReason>(OptionFailureReason.FailureInsertingIntoDataBase));
            }
            catch (Exception e)
            {
                return new CreateOptionFailure(new FailureReason<OptionFailureReason>(OptionFailureReason.FailureInsertingIntoDataBase));
            }
        }
    }
}
