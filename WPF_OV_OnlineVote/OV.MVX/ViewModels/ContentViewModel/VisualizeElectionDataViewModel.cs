using Caliburn.Micro;
using MvvmCross.ViewModels;
using OV.MainDb.Organizer.Find.Models.Public;
using OV.MVX.Models;
using OV.MVX.Models.Habitant;
using OV.MVX.Services.Organizer;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class VisualizeElectionDataViewModel : MvxViewModel
    {
        //!Private variables
        private IOrganizerService _organizerService;
        private SuperAdminAllElectionModel _selectedElection;
        private BindableCollection<ShortOrganizerInfo> _organizers;
        private ShortOrganizerInfo _selectedOrganizer;

        //!Properties
        public SuperAdminAllElectionModel SelectedElection
        {
            get { return _selectedElection; }
            set
            {
                SetProperty(ref _selectedElection, value);
                RaisePropertyChanged(() => SelectedElection);
                RaisePropertyChanged(() => Organizers);
            }
        }
        public BindableCollection<ShortOrganizerInfo> Organizers
        {
            get
            {
                return _organizers;
            }
            set
            {
                SetProperty(ref _organizers, value);
                RaisePropertyChanged(() => Organizers);
            }
        }
        public ShortOrganizerInfo SelectedOrganizer
        {
            get
            {
                return _selectedOrganizer;
            }
            set
            {
                SetProperty(ref _selectedOrganizer, value);
                RaisePropertyChanged(() => SelectedOrganizer);
            }
        }


        public VisualizeElectionDataViewModel()
        {
            _organizerService = new OrganizerService();
        }

        //!Methods
        public async Task LoadData(SuperAdminAllElectionModel selectedElection)
        {
            if(selectedElection != null)
            {
                var organizers = await _organizerService.FindAsync(OrganizerFilter.ByElectionId(selectedElection.Id.Value).AndIncludeUser(), new CancellationToken());
                var listOfOrganizers = organizers.ToList();
                List<ShortOrganizerInfo> ShortOrganizerInformations = new List<ShortOrganizerInfo>();
                foreach (var organizer in listOfOrganizers)
                {
                    var model = new ShortOrganizerInfo();
                    model.SetData(organizer);
                    ShortOrganizerInformations.Add(model);
                }
                Organizers = new BindableCollection<ShortOrganizerInfo>(ShortOrganizerInformations);
                SelectedElection = selectedElection;
            }
        }
    }
}
