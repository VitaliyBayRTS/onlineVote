using Microsoft.EntityFrameworkCore.Storage;
using OV.MainDb.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MainDb.Option.IncreaseVotes
{
    public interface IIncreaseVotesDataService
    {
        Task<bool> IncreaseVote(int tblOption_UID, CancellationToken cancellationToken);
        IDbContextTransaction BeginTransaction();
        void CommitTransaction(IDbContextTransaction transaction);
        void RollBackTransaction(IDbContextTransaction transaction);
        IOvMainDbContext getOvMainDbContext();
    }
    public class IncreaseVotesDataService : IIncreaseVotesDataService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IOvMainDbContext _ovMainDbContext;
        public IncreaseVotesDataService(IOvMainDbContextFactory ovMainDbContextFactory)
        {
            _ovMainDbContextFactory = ovMainDbContextFactory ?? throw new ArgumentNullException(nameof(ovMainDbContextFactory));
            _ovMainDbContext = _ovMainDbContextFactory.Create();
        }

        public IncreaseVotesDataService(IOvMainDbContext ovMainDbContext)
        {
            _ovMainDbContext = ovMainDbContext ?? throw new ArgumentNullException(nameof(ovMainDbContext));
        }

        public async Task<bool> IncreaseVote(int tblOption_UID, CancellationToken cancellationToken)
        {
            var option = _ovMainDbContext.Options.First(o => o.Id == tblOption_UID);
            var previusCountOfVotes = option.Votes;

            option.Votes = option.Votes == null ? 1 : (previusCountOfVotes + 1);

            await _ovMainDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _ovMainDbContext.Database.BeginTransaction();
        }

        public void CommitTransaction(IDbContextTransaction transaction)
        {
            transaction.Commit();
        }

        public void RollBackTransaction(IDbContextTransaction transaction)
        {
            transaction.Rollback();
        }

        public IOvMainDbContext getOvMainDbContext()
        {
            return _ovMainDbContext;
        }
    }
}
