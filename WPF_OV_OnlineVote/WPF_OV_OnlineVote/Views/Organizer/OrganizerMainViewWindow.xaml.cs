using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels.OrganizerViewModel;

namespace WPF_OV_OnlineVote.Views.Organizer
{
    /// <summary>
    /// Interaction logic for OrganizerMainViewWindow.xaml
    /// </summary>
    public partial class OrganizerMainViewWindow : MvxWindow
    {
        private MainOrganizerViewModel _mainOrganizerViewModel;
        public OrganizerMainViewWindow()
        {
            InitializeComponent();
        }
        public void LoadDataContext(int Habitant_UID)
        {
            _mainOrganizerViewModel = new MainOrganizerViewModel();
            _mainOrganizerViewModel.Organizer_UID = Habitant_UID;
            DataContext = _mainOrganizerViewModel;
        }
    }
}
