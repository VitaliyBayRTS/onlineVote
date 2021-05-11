using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.Configuration;
using OV.MainDb.Province.Find;
using OV.MainDb.User.Create;
using OV.MainDb.User.Create.Models.Public;
using OV.MainDb.User.Models.Public;
using OV.MVX.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.User
{
    public interface IUserService
    {
        Task<ICreateUserResponse> CreateUserAsync(CandidateUser candidate, CancellationToken cancellation = default); 
    }
    public class UserService : IUserService
    {
        private IOvMainDbContext dbContext;
        private ICreateUserService _createUserService;
        public UserService()
        {
            dbContext = new CreateDbContext().getOvMainDbContext();

            var findAutonomousCommunityDataService = new FindAutonomousCommunityDataService(dbContext);
            var findProvinceDataService = new FindProvinceDataService(dbContext);
            var validator = new CandidateUserValidator(findAutonomousCommunityDataService, findProvinceDataService);

            _createUserService = new CreateUserService(new CreateUserDataService(dbContext), validator);
        }
        public async Task<ICreateUserResponse> CreateUserAsync(CandidateUser candidate, CancellationToken cancellation = default)
        {
            return await _createUserService.CreateAsync(candidate, cancellation);
        }
    }
}
