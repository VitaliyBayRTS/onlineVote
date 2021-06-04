using Caliburn.Micro;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.AutonomousCommunity.Find.Models.Public;
using OV.MainDb.Election.Find.Models.Public;
using OV.Models.MainDb.AutonomousCommunity;
using OV.Models.MainDb.Province;
using OV.MVX.Models;
using OV.MVX.Services.AutonomousCommunity;
using OV.MVX.Services.Election;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class SeeAllElectionViewModel : MvxViewModel
    {
        //!Commands
        public IMvxCommand VisualizeElectionDataViewCommand { get; set; }
        public IMvxCommand VisualizeOptionDataViewCommand { get; set; }

        //!Services
        private IElectionService _electionService;
        private IAutonomousCommunityService _autonomousCommunityService;

        //!Private variables
        private bool _isElectionDataBtnIsActive = false;
        private object _currentDetailsView;
        private ObservableCollection<AutonomousCommunity> _allAutonomousCommunities = new ObservableCollection<AutonomousCommunity>();
        private AutonomousCommunity _autonomousCommunity;
        private ObservableCollection<string> _states = new ObservableCollection<string>();
        private string _selectedState;
        private ObservableCollection<Province> _provincesOfCommunity = new ObservableCollection<Province>();
        private Province _province;
        private SuperAdminAllElectionModel _selectedElection;
        private BindableCollection<SuperAdminAllElectionModel> _elections;
        private BindableCollection<OV.Models.MainDb.Organizer.Organizer> _organizers;
        private BindableCollection<OV.Models.MainDb.Organizer.Organizer> _selectedOrganizer;
        private List<SuperAdminAllElectionModel> _allElections;

        //!Properties
        public bool IsElectionDataBtnIsActive 
        {
            get 
            {
                return _isElectionDataBtnIsActive;
            } 
            set
            {
                SetProperty(ref _isElectionDataBtnIsActive, value);
                RaisePropertyChanged(() => IsElectionDataBtnIsActive);
            }
        }
        public SuperAdminAllElectionModel SelectedElection
        {
            get { return _selectedElection; }
            set
            {
                SetProperty(ref _selectedElection, value);
                IsElectionDataBtnIsActive = true;
                OpenElectionDataDetailsView();
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

        public ObservableCollection<string> States
        {
            get { return _states; }
            set { SetProperty(ref _states, value); }
        }
        public string SelectedState
        {
            get { return _selectedState; }
            set
            {
                SetProperty(ref _selectedState, value);
                RaisePropertyChanged(() => SelectedState);
                if (value != null)
                {
                    Elections = new BindableCollection<SuperAdminAllElectionModel>(_allElections
                        .Where(ae => ae.CurrentState == value && ae.AutonomousCommunityId == AutonomousCommunity?.Id
                        && ae.ProvinceId == Province?.Id));
                }
                RaisePropertyChanged(() => Elections);
            }
        }
        public ObservableCollection<AutonomousCommunity> AllAutonomousCommunities
        {
            get { return _allAutonomousCommunities; }
            set { SetProperty(ref _allAutonomousCommunities, value); }
        }
        public AutonomousCommunity AutonomousCommunity
        {
            get { return _autonomousCommunity; }
            set
            {
                SetProperty(ref _autonomousCommunity, value);
                RaisePropertyChanged(() => AutonomousCommunity);
                if (value != null)
                {
                    _provincesOfCommunity = new ObservableCollection<Province>(value?.Provinces.OrderBy(p => p.Name));
                    Elections = new BindableCollection<SuperAdminAllElectionModel>(_allElections
                        .Where(ae => ae.AutonomousCommunityId == value.Id && ae.CurrentState == SelectedState));
                }
                RaisePropertyChanged(() => ProvincesOfCommunity);
                RaisePropertyChanged(() => Elections);
            }
        }
        public ObservableCollection<Province> ProvincesOfCommunity
        {
            get { return _provincesOfCommunity; }
            set
            {
                SetProperty(ref _provincesOfCommunity, value);
            }
        }
        public Province Province
        {
            get { return _province; }
            set
            {
                SetProperty(ref _province, value);
                if(value != null)
                {
                    Elections = new BindableCollection<SuperAdminAllElectionModel>(_allElections.Where(ae => ae.ProvinceId == value.Id && ae.CurrentState == SelectedState));
                }
                RaisePropertyChanged(() => Province);
                RaisePropertyChanged(() => Elections);
            }
        }
        public object CurrentDetailsView 
        {
            get 
            {
                return _currentDetailsView;
            }
            set 
            {
                SetProperty(ref _currentDetailsView, value);
                RaisePropertyChanged(() => CurrentDetailsView);
            }
        }
        public VisualizeElectionDataViewModel VisualizeElectionDataVM { get; set; }
        public VisualizeOptionDataViewModel VisualizeOptionDataVM { get; set; }

        public SeeAllElectionViewModel()
        {
            _electionService = new ElectionService();
            _autonomousCommunityService = new AutonomousCommunityService();
            _allAutonomousCommunities = new ObservableCollection<AutonomousCommunity>();
            VisualizeElectionDataVM = new VisualizeElectionDataViewModel();
            VisualizeOptionDataVM = new VisualizeOptionDataViewModel();


            VisualizeElectionDataViewCommand = new MvxCommand(OpenElectionDataDetailsView);
            VisualizeOptionDataViewCommand = new MvxCommand(OpenOptionDataDetailsView);

            List<string> states = new List<string>();
            states.Add(State.EnCurso.ToString());
            states.Add(State.Pendiente.ToString());
            states.Add(State.Terminado.ToString());

            States = new ObservableCollection<string>(states);
        }

        //!Methods
        public async Task LoadData()
        {
            var result = await _electionService.FindAsync(ElectionFilter.IncludType().AndACIncluded().AndProvinceIncluded().AndOrganizersIncluded(),
                new CancellationToken());
            var resultList = result.ToList();
            _allElections = new List<SuperAdminAllElectionModel>();
            foreach (var item in resultList)
            {
                var election = new SuperAdminAllElectionModel();
                election.SetData(item);
                _allElections.Add(election);
            }
            resultList.ForEach(u => new SuperAdminAllElectionModel().SetData(u));
            var autonomousCommunities = await _autonomousCommunityService.FindAsync(AutonomousCommunityFilter.All.AndIncludeProvince(), new CancellationToken());
            AllAutonomousCommunities = new ObservableCollection<AutonomousCommunity>(autonomousCommunities);
            Elections = new BindableCollection<SuperAdminAllElectionModel>(_allElections);
        }

        private async void OpenElectionDataDetailsView()
        {
            if(SelectedElection != null)
                await VisualizeElectionDataVM.LoadData(SelectedElection);
            CurrentDetailsView = VisualizeElectionDataVM;
        }
        private void OpenOptionDataDetailsView()
        {
            if(SelectedElection != null)
                VisualizeOptionDataVM.LoadData(SelectedElection.Id.Value);
            CurrentDetailsView = VisualizeOptionDataVM;
        }
    }
}
