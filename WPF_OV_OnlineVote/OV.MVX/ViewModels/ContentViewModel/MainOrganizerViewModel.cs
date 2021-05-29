using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class MainOrganizerViewModel : MvxViewModel
    { //!Commands
        public IMvxCommand ElectionManagementViewCommand { get; set; }

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
        public int Organizer_UID { get; set; }


        public MainOrganizerViewModel(int tblOrganizer_UID)
        {
            Organizer_UID = tblOrganizer_UID;
            ElectionManagementVM = new EditElectionViewModel(Organizer_UID);

            CurrentView = ElectionManagementVM;

            ElectionManagementViewCommand = new MvxCommand(OpenElectionVM);
        }


        //!Methods

        private void OpenElectionVM()
        {
            CurrentView = ElectionManagementVM;
        }

        //private async void OpenAllElectionVM()
        //{
        //    await AllElectionVM.LoadData();
        //    CurrentView = AllElectionVM;
        //}
    }
}
