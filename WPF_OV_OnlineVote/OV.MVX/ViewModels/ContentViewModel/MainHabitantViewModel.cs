using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class MainHabitantViewModel : MvxViewModel
    {
        //!Commands
        public IMvxCommand SeeAllElectionsViewCommand { get; set; }
        public IMvxCommand RealizVotationViewCommand { get; set; }

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
        public RealizeVotationViewModel RealizeVotationVM { get; set; }
        public int Habitant_UID { get; set; }
        public int User_UID { get; set; }


        public MainHabitantViewModel(int tblHabitant_UID, int user_UID)
        {
            Habitant_UID = tblHabitant_UID;
            User_UID = user_UID;
            SeeAllElectionsVM = new SeeAllElectionViewModel();
            RealizeVotationVM = new RealizeVotationViewModel();

            CurrentView = SeeAllElectionsVM;

            SeeAllElectionsViewCommand = new MvxCommand(OpenSeeAllElectionsVM);
            RealizVotationViewCommand = new MvxCommand(RealizeVotation);
        }


        //!Methods

        private async void OpenSeeAllElectionsVM()
        {
            await SeeAllElectionsVM.LoadData();
            CurrentView = SeeAllElectionsVM;
        }

        private async void RealizeVotation()
        {
            await RealizeVotationVM.LoadData(Habitant_UID, User_UID);
            CurrentView = RealizeVotationVM;
        }
    }
}
