using OV.DbRemoteConfigurationService.DbService;
using OV.MainDb.Configuration;
using OV.MainDb.Election.Create;
using OV.MainDb.Election.Create.Models.Public;
using OV.MainDb.Election.Delete;
using OV.MainDb.Election.Find;
using OV.MainDb.Election.Find.Models.Public;
using OV.MainDb.Election.GetForVote;
using OV.MainDb.Election.Models.Public;
using OV.MainDb.Election.Modify;
using OV.MainDb.Election.Modify.Models.Public;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.Election
{
    public interface IElectionService
    {
        Task<ICreateElectionResponse> CreateAsync(CandidateElection candidate, CancellationToken cancellationToken);
        Task<IEnumerable<OV.Models.MainDb.Election.Election>> FindAsync(ElectionFilter filter, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int electionId, CancellationToken cancellationToken);
        Task<IModifyElectionResponse> ModifyAsync(ModifyElectionCandidate candidate, CancellationToken cancellationToken);
        Task<IEnumerable<OV.Models.MainDb.Election.Election>> FindForVoteAsync(int tblUser_UID, CancellationToken cancellationToken);
    }
    public class ElectionService : IElectionService
    {
        private IOvMainDbContext _dbContext;
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private ICreateElectionService _createElectionService;
        private IFindElectionService _findElectionService;
        private IDeleteElectionService _deleteElectionService;
        private IModifyElectionService _modifyElectionService;
        private IGetElectionForVoteDataService _getElectionForVoteDataService;
        public ElectionService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();
            _dbContext = dbContextCreator.getOvMainDbContext();

            var createElectionValidator = new CandidateElectionValidator();
            _createElectionService = new CreateElectionService(new CreateElectionDataService(_ovMainDbContextFactory), createElectionValidator);

            _findElectionService = new FindElectionService(new FindElectionDataService(_ovMainDbContextFactory));

            _deleteElectionService = new DeleteElectionService(new DeleteElectionDataService(_ovMainDbContextFactory));

            var modifyElectionValidator = new ModifyElectionValidator();
            _modifyElectionService = new ModifyElectionService(new ModifyElectionDataService(_ovMainDbContextFactory), modifyElectionValidator);

            _getElectionForVoteDataService = new GetElectionForVoteDataService(_ovMainDbContextFactory, new FindElectionDataService(_ovMainDbContextFactory));
        }

        public async Task<ICreateElectionResponse> CreateAsync(CandidateElection candidate, CancellationToken cancellationToken)
        {
            return await _createElectionService.CreateAsync(candidate, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int electionId, CancellationToken cancellationToken)
        {
            return await _deleteElectionService.DeleteAsync(electionId, cancellationToken);
        }

        public async Task<IEnumerable<OV.Models.MainDb.Election.Election>> FindAsync(ElectionFilter filter, CancellationToken cancellationToken)
        {
            return await _findElectionService.FindAsync(filter, cancellationToken);
        }

        public Task<IEnumerable<OV.Models.MainDb.Election.Election>> FindForVoteAsync(int tblUser_UID, CancellationToken cancellationToken)
        {
            return _getElectionForVoteDataService.FindForVoteAsync(tblUser_UID, cancellationToken);
        }

        public async Task<IModifyElectionResponse> ModifyAsync(ModifyElectionCandidate candidate, CancellationToken cancellationToken)
        {
            return await _modifyElectionService.ModifyAsync(candidate, cancellationToken);
        }
    }
}
