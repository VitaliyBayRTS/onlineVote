using OV.MainDb.User.Create.Models.Public;
using OV.MainDb.User.Models.Public;
using OV.Models.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.User.Create
{
    public interface ICreateUserService
    {
        Task<ICreateUserResponse> CreateAsync(CandidateUser candidate, CancellationToken cancellationToken);
    }
    public class CreateUserService : ICreateUserService
    {

        private ICreateUserDataService _createUserDataService;
        private ICandidateUserValidator _validator;

        public CreateUserService(ICreateUserDataService createUserDataService, ICandidateUserValidator validator)
        {
            _createUserDataService = createUserDataService ?? throw new ArgumentNullException(nameof(createUserDataService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }


        public async Task<ICreateUserResponse> CreateAsync(CandidateUser candidate, CancellationToken cancellationToken)
        {
            try
            {
                var validatorResult = await _validator.ValidateAsync(candidate);
                if(!validatorResult.IsValid)
                {
                    return new CreateUserFailure(validatorResult.Errors.ParseFailures<UserFailureReason>());
                }


                var insertedUser = await _createUserDataService.CreateAsync(candidate, cancellationToken);

                if(insertedUser != null)
                {
                    return new CreateUserSuccess(insertedUser.ToUser());
                }
                return new CreateUserFailure(new FailureReason<UserFailureReason>(UserFailureReason.FailureInsertingIntoDataBase));
            }
            catch (Exception e)
            {
                return new CreateUserFailure(new FailureReason<UserFailureReason>(UserFailureReason.FailureInsertingIntoDataBase));
            }
        }
    }
}
