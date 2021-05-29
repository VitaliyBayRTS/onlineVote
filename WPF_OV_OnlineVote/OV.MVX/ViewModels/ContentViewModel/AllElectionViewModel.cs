using Caliburn.Micro;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.Election.Find.Models.Public;
using OV.MainDb.Organizer.Find.Models.Public;
using OV.MVX.Models;
using OV.MVX.Services.Election;
using OV.MVX.Services.Organizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class AllElectionViewModel : MvxViewModel
    {
        //!Cpmmands
        public IMvxCommand DeleteElectionDataCommand { get; set; }

        //!Private variables
        private IElectionService _electionService;
        private IOrganizerService _organizerService;
        private SuperAdminAllElectionModel _selectedElection;
        private BindableCollection<SuperAdminAllElectionModel> _elections;
        private BindableCollection<OV.Models.MainDb.Organizer.Organizer> _organizers;
        private BindableCollection<OV.Models.MainDb.Organizer.Organizer> _selectedOrganizer;

        //!Properties
        public SuperAdminAllElectionModel SelectedElection
        {
            get { return _selectedElection; }
            set
            {
                SetProperty(ref _selectedElection, value);
                new Task(async () =>
                {
                    if(_selectedElection != null)
                    {
                        var organizers = await _organizerService.FindAsync(OrganizerFilter.ByElectionId(_selectedElection.Id.Value).AndIncludeUser(), new CancellationToken());
                        Organizers = new BindableCollection<OV.Models.MainDb.Organizer.Organizer>(organizers);
                    }
                }).Start();
                RaisePropertyChanged(() => SelectedElection);
                RaisePropertyChanged(() => Organizers);
            }
        }
        public BindableCollection<SuperAdminAllElectionModel> Elections
        {
            get
            {
                return _elections;
            }
            set
            {
                SetProperty(ref _elections, value);
                RaisePropertyChanged(() => Elections);
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


        public AllElectionViewModel()
        {
            _electionService = new ElectionService();
            _organizerService = new OrganizerService();
            DeleteElectionDataCommand = new MvxCommand(DeleteElection);
        }

        //!Methods
        public async void DeleteElection()
        {
            if(DateTime.Now >= _selectedElection.InitDate)
            {
                MessageBox.Show("Se pueden borrar solamante elecciones que estan en estado 'Pendiente'", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            } else
            {
                var result = MessageBox.Show("¿Estas seguro que quieres BORRAR esta elección?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var response = await _electionService.DeleteAsync(_selectedElection.Id.Value, new CancellationToken());
                    if (response)
                    {
                        await LoadData();
                    }
                }
            }
            
        }

        public async Task LoadData()
        {
            var result = await _electionService.FindAsync(ElectionFilter.IncludType().AndACIncluded().AndProvinceIncluded().AndOrganizersIncluded(), 
                new CancellationToken());
            var resultList = result.ToList();
            List<SuperAdminAllElectionModel> elections = new List<SuperAdminAllElectionModel>();
            foreach (var item in resultList)
            {
                var election = new SuperAdminAllElectionModel();
                election.SetData(item);
                elections.Add(election);
            }
            resultList.ForEach(u => new SuperAdminAllElectionModel().SetData(u));
            Elections = new BindableCollection<SuperAdminAllElectionModel>(elections);
        }
    }
}
