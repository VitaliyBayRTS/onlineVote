using Caliburn.Micro;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.Election.Find.Models.Public;
using OV.MVX.Models;
using OV.MVX.Services.Election;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class RealizeVotationViewModel : MvxViewModel
    { //!Commands
        public IMvxCommand VisualizeElectionDataViewCommand { get; set; }
        public IMvxCommand VoteByOptionViewCommand { get; set; }

        //!Services
        private IElectionService _electionService;

        //!Private variables
        private bool _isElectionDataBtnIsActive = false;
        private object _currentDetailsView;
        private SuperAdminAllElectionModel _selectedElection;
        private BindableCollection<SuperAdminAllElectionModel> _elections;
        private BindableCollection<OV.Models.MainDb.Organizer.Organizer> _organizers;
        private BindableCollection<OV.Models.MainDb.Organizer.Organizer> _selectedOrganizer;

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
        public VoteByOptionViewModel VoteByOptionVM { get; set; }
        public int User_UID { get; set; }

        public RealizeVotationViewModel()
        {
            _electionService = new ElectionService();
            VisualizeElectionDataVM = new VisualizeElectionDataViewModel();
            VoteByOptionVM = new VoteByOptionViewModel();


            VisualizeElectionDataViewCommand = new MvxCommand(OpenElectionDataDetailsView);
            VoteByOptionViewCommand = new MvxCommand(OpenOptionDataDetailsView);
        }

        //!Methods
        public async Task LoadData(int habitant_UID, int user_UID)
        {
            User_UID = user_UID;
            var result = await _electionService.FindForVoteAsync(user_UID,
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

        private async void OpenElectionDataDetailsView()
        {
            if (SelectedElection != null)
                await VisualizeElectionDataVM.LoadData(SelectedElection);
            CurrentDetailsView = VisualizeElectionDataVM;
        }
        private void OpenOptionDataDetailsView()
        {
            if (SelectedElection != null)
                VoteByOptionVM.LoadData(SelectedElection.Id.Value, SelectedElection.CurrentState,  User_UID);
            CurrentDetailsView = VoteByOptionVM;
        }
    }
}
