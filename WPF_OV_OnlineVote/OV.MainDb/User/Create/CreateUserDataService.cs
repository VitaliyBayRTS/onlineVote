using OV.MainDb.Configuration;
using OV.MainDb.User.Create.Models.Public;
using OV.MainDb.User.Models;
using OV.MainDb.User.Models.Public;
using System;
using System.Collections.Generic;
using System.Text;
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
        private IOvMainDbContext _ovMainDbContext;
        public CreateUserDataService(IOvMainDbContext ovMainDbContext)
        {
            _ovMainDbContext = ovMainDbContext ?? throw new ArgumentNullException(nameof(ovMainDbContext));
        }

        public async Task<PersistedUser> CreateAsync(CandidateUser candidate, CancellationToken cancellationToken)
        {
            PersistedUser user = new PersistedUser()
            {
                FirstName = candidate.FirstName,
                SecondName = candidate.SecondName,
                SurName = candidate.SurName,
                SecondSurName = candidate.SecondSurName,
                Password = candidate.Password,
                DOB = candidate.DOB,
                TblAutonomousCommunities_UID = candidate.TblAutonomousCommunities_UID,
                TblProvince_UID = candidate.TblProvince_UID,
                Email = candidate.Email,
                PhoneNumber = candidate.PhoneNumber,
            };

            var newUser = await _ovMainDbContext.Users.AddAsync(user);

            //TODO: Add new user to UnauthorithedUsers table

            await _ovMainDbContext.SaveChangesAsync(cancellationToken);

            return newUser.Entity;

        }
    }
}
