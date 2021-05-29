using Caliburn.Micro;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.Election.Find.Models.Public;
using OV.MainDb.Organizer.Find.Models.Public;
using OV.Models.MainDb.Organizer;
using OV.MVX.Models.Organizer;
using OV.MVX.Services.Election;
using OV.MVX.Services.Organizer;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class EditElectionViewModel : MvxViewModel
    { //!Cpmmands
        public IMvxCommand EditElectionDataCommand { get; set; }

        //!Private variables
        private IElectionService _electionService;
        private IOrganizerService _organizerService;
        private ElectionModel _election;
        private BindableCollection<Organizer> _organizers;
        private BindableCollection<Organizer> _selectedOrganizer;

        //!Properties
        public ElectionModel Election
        {
            get { return _election; }
            set
            {
                SetProperty(ref _election, value);
                new Task(async () =>
                {
                    if (_election != null)
                    {
                        var organizers = await _organizerService.FindAsync(OrganizerFilter.ByElectionId(_election.Id.Value).AndIncludeUser(), new CancellationToken());
                        Organizers = new BindableCollection<OV.Models.MainDb.Organizer.Organizer>(organizers);
                    }
                }).Start();
                RaisePropertyChanged(() => Election);
                RaisePropertyChanged(() => Organizers);
            }
        }
        public BindableCollection<OV.Models.MainDb.Organizer.Organizer> Organizers
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
        public BindableCollection<OV.Models.MainDb.Organizer.Organizer> SelectedOrganizer
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
        public int TblOrganizer_UID { get; set; }


        public EditElectionViewModel(int tblOrganizer_UID)
        {
            TblOrganizer_UID = tblOrganizer_UID;
            _electionService = new ElectionService();
            _organizerService = new OrganizerService();
            EditElectionDataCommand = new MvxCommand(EditElection);
        }

        //!Methods
        public async void EditElection()
        {
            //if (DateTime.Now >= _selectedElection.InitDate)
            //{
            //    MessageBox.Show("Se pueden borrar solamante elecciones que estan en estado 'Pendiente'", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            //}
            //else
            //{
            //    var result = MessageBox.Show("¿Estas seguro que quieres BORRAR esta elección?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        var response = await _electionService.DeleteAsync(_selectedElection.Id.Value, new CancellationToken());
            //        if (response)
            //        {
            //            await LoadData();
            //        }
            //    }
            //}

        }

        public async Task LoadData()
        {
            var organizer = await _organizerService.FindAsync(OrganizerFilter.ById(TblOrganizer_UID), new CancellationToken());
            if (organizer == null || organizer.ToList().Count == 0)
                throw new ArgumentNullException("Organizer does not exist");

            var singleOrganizer = organizer.First();
            var result = await _electionService.FindAsync(ElectionFilter.ById(singleOrganizer.tblElection_UID).AndTypeIncluded().AndACIncluded().AndProvinceIncluded(),
                new CancellationToken());
            var singleElection = result.First();
            var electionModel = new ElectionModel();
            electionModel.SetData(singleElection);
            Election = electionModel;
        }
    }
}
