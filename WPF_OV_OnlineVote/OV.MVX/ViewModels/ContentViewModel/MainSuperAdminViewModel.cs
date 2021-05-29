using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class MainSuperAdminViewModel : MvxViewModel
    {
        //!Commands
        public IMvxCommand ElectionManagementViewCommand { get; set; }
        public IMvxCommand HomeViewCommand { get; set; }
        public IMvxCommand AllElectionViewCommand { get; set; }

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
        public UnautorizedUsersViewModel UnautorizedUsersVM { get; set; }
        public ElectionManagementViewModel ElectionManagementVM { get; set; }
        public AllElectionViewModel AllElectionVM { get; set; }
        public int SuperAdmin_UID { get; set; }


        public MainSuperAdminViewModel()
        {
            UnautorizedUsersVM = new UnautorizedUsersViewModel();
            ElectionManagementVM = new ElectionManagementViewModel();
            AllElectionVM = new AllElectionViewModel();

            CurrentView = UnautorizedUsersVM;

            ElectionManagementViewCommand = new MvxCommand(OpenElectionVM);
            HomeViewCommand = new MvxCommand(OpenHomeVM);
            AllElectionViewCommand = new MvxCommand(OpenAllElectionVM);
        }


        //!Methods
        private async void OpenHomeVM()
        {
            //await UnautorizedUsersVM.LoadData();
            CurrentView = UnautorizedUsersVM;
        }

        private void OpenElectionVM()
        {
            CurrentView = ElectionManagementVM;
        }

        private async void OpenAllElectionVM()
        {
            await AllElectionVM.LoadData();
            CurrentView = AllElectionVM;
        }
    }
}
