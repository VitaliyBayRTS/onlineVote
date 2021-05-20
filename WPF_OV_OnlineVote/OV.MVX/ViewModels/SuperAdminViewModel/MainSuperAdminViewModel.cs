using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace OV.MVX.ViewModels.SuperAdminViewModel
{
    public class MainSuperAdminViewModel : MvxViewModel
    {

        public IMvxCommand ElectionManagementViewCommand { get; set; }
        public IMvxCommand HomeViewCommand { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                RaisePropertyChanged(() => CurrentView);
            }
        }
        public HomeViewModel HomeVM { get; set; }
        public ElectionManagementViewModel ElectionManagementVM { get; set; }


        public int SuperAdmin_UID { get; set; }


        public MainSuperAdminViewModel()
        {
            HomeVM = new HomeViewModel();
            ElectionManagementVM = new ElectionManagementViewModel();

            CurrentView = HomeVM;

            ElectionManagementViewCommand = new MvxCommand(OpenElectionVM);
            HomeViewCommand = new MvxCommand(OpenHomeVM);
        }

        private void OpenHomeVM()
        {
            CurrentView = HomeVM;
        }

        private void OpenElectionVM()
        {
            CurrentView = ElectionManagementVM;
        }
    }
}
