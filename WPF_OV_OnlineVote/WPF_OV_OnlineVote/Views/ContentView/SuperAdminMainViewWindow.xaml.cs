using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels.ContentViewModel;
using System.Windows;
using System.Windows.Input;

namespace WPF_OV_OnlineVote.Views.ContentView
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
        public async void LoadDataContext(int SuperAdmin_UID)
        {
            _mainSuperAdminViewModel = new MainSuperAdminViewModel();
            _mainSuperAdminViewModel.SuperAdmin_UID = SuperAdmin_UID;
            await _mainSuperAdminViewModel.UnautorizedUsersVM.LoadData();
            await _mainSuperAdminViewModel.ElectionManagementVM.LoadData();
            DataContext = _mainSuperAdminViewModel;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows[0].Show();
            this.Close();
        }
    }
}
