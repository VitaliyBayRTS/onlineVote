using OV.MainDb.Configuration;
using OV.MainDb.User.Models;
using OV.MainDb.User.Models.Public;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.User.Create
{
    public interface ICreateUserDataService
    {
        Task<PersistedUser> CreateAsync(CandidateUser candidate, CancellationToken cancellationToken);
    }
    public class CreateUserDataService : ICreateUserDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public CreateUserDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<PersistedUser> CreateAsync(CandidateUser candidate, CancellationToken cancellationToken)
        {

            var _ovMainDbContext = _ovMainDbContextFactory.Create();

            PersistedUser user = new PersistedUser()
            {
                FirstName = candidate.FirstName,
                SecondName = candidate.SecondName,
                SurName = candidate.SurName,
                SecondSurName = candidate.SecondSurName,
                Password = candidate.Password,
                DOB = candidate.DOB,
                TblProvince_UID = candidate.TblProvince_UID,
                Email = candidate.Email,
                PhoneNumber = candidate.PhoneNumber,
                DNI_NIE = candidate.DNI_NIE
            };

            var newUser = _ovMainDbContext.Users.Add(user);
            
            await _ovMainDbContext.SaveChangesAsync(cancellationToken);

            return newUser.Entity;

        }
    }
}
