using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels.SuperAdminViewModel;

namespace WPF_OV_OnlineVote.Views.SuperAdmin
{
    /// <summary>
    /// Interaction logic for SuperAdminMainViewWindow.xaml
    /// </summary>
    public partial class SuperAdminMainViewWindow : MvxWindow
    {
        private MainSuperAdminViewModel _mainSuperAdminViewModel;
        public SuperAdminMainViewWindow()
        {
            InitializeComponent();
        }
        public void LoadDataContext(int SuperAdmin_UID)
        {
            _mainSuperAdminViewModel = new MainSuperAdminViewModel();
            _mainSuperAdminViewModel.SuperAdmin_UID = SuperAdmin_UID;
            DataContext = _mainSuperAdminViewModel;
        }
    }
}
