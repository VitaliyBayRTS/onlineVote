using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class MainOrganizerViewModel : MvxViewModel
    { 
        //!Commands
        public IMvxCommand ElectionManagementViewCommand { get; set; }
        public IMvxCommand AddNewOptionViewCommand { get; set; }

        //!Private variables
        private object _currentView;

        //!Properties
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                RaisePropertyChanged(() => CurrentView);
            }
        }
        public EditElectionViewModel ElectionManagementVM { get; set; }
        public AddOptionViewModel AddNewOptionVC { get; set; }
        public int Organizer_UID { get; set; }
        public int Election_UID { get; set; }


        public MainOrganizerViewModel(int tblOrganizer_UID, int tblElection_UID)
        {
            Organizer_UID = tblOrganizer_UID;
            Election_UID = tblElection_UID;
            ElectionManagementVM = new EditElectionViewModel(Organizer_UID, Election_UID);
            AddNewOptionVC = new AddOptionViewModel(Organizer_UID, Election_UID);

            CurrentView = ElectionManagementVM;

            ElectionManagementViewCommand = new MvxCommand(OpenElectionVM);
            AddNewOptionViewCommand = new MvxCommand(OpenAddNewOptionVM);
        }


        //!Methods

        private async void OpenElectionVM()
        {
            await ElectionManagementVM.LoadData();
            CurrentView = ElectionManagementVM;
        }
        private async void OpenAddNewOptionVM()
        {
            await AddNewOptionVC.LoadData();
            CurrentView = AddNewOptionVC;
        }
    }
}
