using OV.MainDb.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.User.Delete
{
    public interface IDeleteUserDataService
    {
        Task<bool> DeleteAsync(int userId, CancellationToken cancellationToken);
    }
    public class DeleteUserDataService : IDeleteUserDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        public DeleteUserDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
        }

        public async Task<bool> DeleteAsync(int userId, CancellationToken cancellationToken)
        {

            var ovMainDbContext = _ovMainDbContextFactory.Create();

            var user = ovMainDbContext.Users.First(u => u.Id == userId);

            ovMainDbContext.Users.Remove(user);

            await ovMainDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
