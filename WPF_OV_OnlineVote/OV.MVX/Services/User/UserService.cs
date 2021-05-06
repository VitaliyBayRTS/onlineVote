using OV.MainDb.Configuration;
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
            _createUserService = new CreateUserService(new CreateUserDataService(dbContext), new CandidateUserValidator());
        }
        public async Task<ICreateUserResponse> CreateUserAsync(CandidateUser candidate, CancellationToken cancellation = default)
        {
            return await _createUserService.CreateAsync(candidate, cancellation);
        }
    }
}
