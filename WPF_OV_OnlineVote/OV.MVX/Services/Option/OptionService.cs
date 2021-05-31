using OV.MainDb.Configuration;
using OV.MainDb.Option.Create;
using OV.MainDb.Option.Create.Models.Public;
using OV.MainDb.Option.Delete;
using OV.MainDb.Option.Find;
using OV.MainDb.Option.Find.Models.Public;
using OV.MainDb.Option.Models.Public;
using OV.MainDb.Option.Modify;
using OV.MainDb.Option.Modify.Models.Public;
using OV.MVX.Helpers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.Services.Option
{
    public interface IOptionService
    {
        Task<IEnumerable<OV.Models.MainDb.Option.Option>> FindAsync(OptionFilter filter, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(int optionId, CancellationToken cancellationToken);
        Task<ICreateOptionResponse> CreateAsync(CandidateOption candidate, CancellationToken cancellationToken);
        Task<IModifyOptionResponse> ModifyAsync(ModifyOptionCandidate candidate, CancellationToken cancellationToken);
    }

    public class OptionService : IOptionService
    {
        private IOvMainDbContextFactory _ovMainDbContextFactory;
        private IFindOptionService _findOrganizerService;
        private IDeleteOptionService _deleteOrganizerService;
        private ICreateOptionService _createOrganizerService;
        private IModifyOptionService _modifyOrganizerService;

        public OptionService()
        {
            var dbContextCreator = new CreateDbContext();
            _ovMainDbContextFactory = dbContextCreator.getOvMainDbContextFactory();

            var findOoptionDataService = new FindOptionDataService(_ovMainDbContextFactory);
            _findOrganizerService = new FindOptionService(findOoptionDataService);

            var deleteOptionDataService = new DeleteOptionDataService(_ovMainDbContextFactory);
            _deleteOrganizerService = new DeleteOptionService(deleteOptionDataService);

            var createOptionDataService = new CreateOptionDataService(_ovMainDbContextFactory);
            var createOptionvalidator = new CreateOptionValidator();
            _createOrganizerService = new CreateOptionService(createOptionDataService, createOptionvalidator);

            var modifyOptionDataService = new ModifyOptionDataService(_ovMainDbContextFactory);
            var modifyOptionvalidator = new ModifyOptionValidator();
            _modifyOrganizerService = new ModifyOptionService(modifyOptionDataService, modifyOptionvalidator);
        }

        public async Task<ICreateOptionResponse> CreateAsync(CandidateOption candidate, CancellationToken cancellationToken)
        {
            return await _createOrganizerService.CreateAsync(candidate, cancellationToken);
        }

        public async Task<bool> DeleteAsync(int optionId, CancellationToken cancellationToken)
        {
            return await _deleteOrganizerService.DeleteAsync(optionId, cancellationToken);
        }

        public async Task<IEnumerable<OV.Models.MainDb.Option.Option>> FindAsync(OptionFilter filter, CancellationToken cancellationToken)
        {
            return await _findOrganizerService.FindAsync(filter, cancellationToken);
        }

        public async Task<IModifyOptionResponse> ModifyAsync(ModifyOptionCandidate candidate, CancellationToken cancellationToken)
        {
            return await _modifyOrganizerService.ModifyAsync(candidate, cancellationToken);
        }
    }
}
