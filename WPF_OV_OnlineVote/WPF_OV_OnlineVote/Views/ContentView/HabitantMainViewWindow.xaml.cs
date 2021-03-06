using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels.ContentViewModel;
using System.Windows;
using System.Windows.Input;

namespace WPF_OV_OnlineVote.Views.ContentView
{
    /// <summary>
    /// Interaction logic for HabitantMainViewWindow.xaml
    /// </summary>
    public partial class HabitantMainViewWindow : MvxWindow
    {
        private MainHabitantViewModel _mainHabitantViewModel;
        public HabitantMainViewWindow(int tblhabitant_UID)
        {
            _mainHabitantViewModel = new MainHabitantViewModel(tblhabitant_UID);
            //await _mainHabitantViewModel.ElectionManagementVM.LoadData();
            InitializeComponent();
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
            this.Hide();
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Windows[0].Show();
            this.Close();
        }
    }
}
