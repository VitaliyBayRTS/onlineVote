using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.Configuration;
using OV.MainDb.Province.Find;
using OV.MainDb.User.Create;
using OV.MainDb.User.Create.Models.Public;
using OV.MainDb.User.Find;
using OV.MainDb.User.Find.Models.Public;
using OV.MainDb.User.Models.Public;
using OV.MVX.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.User
{
    public interface IUserService
    {
        Task<ICreateUserResponse> CreateUserAsync(CandidateUser candidate, CancellationToken cancellation = default);
        Task<IEnumerable<OV.Models.MainDb.User.User>> FindAsync(UserFilter filter, CancellationToken cancellationToken = default);
    }
    public class UserService : IUserService
    {
        private IOvMainDbContext _dbContext;
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private ICreateUserService _createUserService;
        private IFindUserService _findUserService;
        public UserService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();
            _dbContext = dbContextCreator.getOvMainDbContext();

            var findAutonomousCommunityDataService = new FindAutonomousCommunityDataService(_dbContext);
            var findProvinceDataService = new FindProvinceDataService(_dbContext);
            var validator = new CandidateUserValidator(findAutonomousCommunityDataService, findProvinceDataService);

            _createUserService = new CreateUserService(new CreateUserDataService(_ovMainDbContextFactory), validator);
            _findUserService = new FindUserService(new FindUserDataService(_ovMainDbContextFactory));
        }
        public async Task<ICreateUserResponse> CreateUserAsync(CandidateUser candidate, CancellationToken cancellation = default)
        {
            return await _createUserService.CreateAsync(candidate, cancellation);
        }

        public async Task<IEnumerable<OV.Models.MainDb.User.User>> FindAsync(UserFilter filter, CancellationToken cancellationToken = default)
        {
            return await _findUserService.FindAsync(filter, cancellationToken);
        }
    }
}
