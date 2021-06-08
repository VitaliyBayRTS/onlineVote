using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels.ContentViewModel;
using System.Windows;
using System.Windows.Input;

namespace WPF_OV_OnlineVote.Views.ContentView
{
    /// <summary>
    /// Interaction logic for OrganizerMainViewModel.xaml
    /// </summary>
    public partial class OrganizerMainViewModel : MvxWindow
    {
        private MainOrganizerViewModel _mainOrganizerViewModel;
        public OrganizerMainViewModel()
        {
            InitializeComponent();
        }
        public async void LoadDataContext(int organizer_UID, int election_UID)
        {
            _mainOrganizerViewModel = new MainOrganizerViewModel(organizer_UID, election_UID);
            await _mainOrganizerViewModel.ElectionManagementVM.LoadData();
            DataContext = _mainOrganizerViewModel;
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
