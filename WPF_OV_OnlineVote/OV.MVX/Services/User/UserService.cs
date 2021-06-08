using OV.DbRemoteConfigurationService.DbService;
using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.Configuration;
using OV.MainDb.Province.Find;
using OV.MainDb.User.Autorize;
using OV.MainDb.User.Create;
using OV.MainDb.User.Create.Models.Public;
using OV.MainDb.User.Delete;
using OV.MainDb.User.Find;
using OV.MainDb.User.Find.Models.Public;
using OV.MainDb.User.Models.Public;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.User
{
    public interface IUserService
    {
        Task<ICreateUserResponse> CreateUserAsync(CandidateUser candidate, CancellationToken cancellation = default);
        Task<IEnumerable<OV.Models.MainDb.User.User>> FindAsync(UserFilter filter, CancellationToken cancellationToken = default);
        Task<bool> AutorizeAsync(int userId, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int userId, CancellationToken cancellationToken = default);
    }
    public class UserService : IUserService
    {
        private IOvMainDbContext _dbContext;
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private ICreateUserService _createUserService;
        private IFindUserService _findUserService;
        private IAutorizeUserService _autorizeUserService;
        private IDeleteUserService _deleteUserService;
        public UserService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();
            _dbContext = dbContextCreator.getOvMainDbContext();

            var findAutonomousCommunityDataService = new FindAutonomousCommunityDataService(_dbContext);
            var findProvinceDataService = new FindProvinceDataService(_dbContext);
            var validator = new CandidateUserValidator(findAutonomousCommunityDataService, 
                findProvinceDataService, 
                new FindUserDataService(_ovMainDbContextFactory));

            _createUserService = new CreateUserService(new CreateUserDataService(_ovMainDbContextFactory), validator);
            _findUserService = new FindUserService(new FindUserDataService(_ovMainDbContextFactory));
            _autorizeUserService = new AutorizeUserService(new AutorizeUserDataService(_ovMainDbContextFactory));
            _deleteUserService = new DeleteUserService(new DeleteUserDataService(_ovMainDbContextFactory));
        }

        public Task<bool> AutorizeAsync(int userId, CancellationToken cancellationToken = default)
        {
            return _autorizeUserService.AutorizeAsync(userId, cancellationToken);
        }

        public async Task<ICreateUserResponse> CreateUserAsync(CandidateUser candidate, CancellationToken cancellation = default)
        {
            return await _createUserService.CreateAsync(candidate, cancellation);
        }

        public Task<bool> DeleteAsync(int userId, CancellationToken cancellationToken = default)
        {
            return _deleteUserService.DeleteAsync(userId, cancellationToken);
        }

        public async Task<IEnumerable<OV.Models.MainDb.User.User>> FindAsync(UserFilter filter, CancellationToken cancellationToken = default)
        {
            return await _findUserService.FindAsync(filter, cancellationToken);
        }
    }
}
