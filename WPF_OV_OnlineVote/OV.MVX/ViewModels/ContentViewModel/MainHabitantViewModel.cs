using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class MainHabitantViewModel : MvxViewModel
    {
        //!Commands
        public IMvxCommand SeeAllElectionsViewCommand { get; set; }

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
        public SeeAllElectionViewModel SeeAllElectionsVM { get; set; }
        public int Habitant_UID { get; set; }


        public MainHabitantViewModel(int tblHabitant_UID)
        {
            Habitant_UID = tblHabitant_UID;
            SeeAllElectionsVM = new SeeAllElectionViewModel();

            CurrentView = SeeAllElectionsVM;

            SeeAllElectionsViewCommand = new MvxCommand(OpenSeeAllElectionsVM);
            //AddNewOptionViewCommand = new MvxCommand(OpenAddNewOptionVM);
        }


        //!Methods

        private async void OpenSeeAllElectionsVM()
        {
            //await ElectionManagementVM.LoadData();
            CurrentView = SeeAllElectionsVM;
        }
        //private async void OpenAddNewOptionVM()
        //{
        //    await AddNewOptionVC.LoadData();
        //    CurrentView = AddNewOptionVC;
        //}
    }
}
