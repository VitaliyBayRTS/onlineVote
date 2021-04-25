using MvvmCross.Platforms.Wpf.Views;
using System.Windows;
using System.Windows.Media;
using WPF_OV_OnlineVote.Views.Login;

namespace WPF_OV_OnlineVote.Views
{
    /// <summary>
    /// Interaction logic for InitialView.xaml
    /// </summary>
    public partial class InitialView : MvxWpfView
    {
        public InitialView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            habitantOption.Background = Brushes.White;
            organizerOption.Background = Brushes.Transparent;
            superAdminOption.Background = Brushes.Transparent;
            newHabitantOption.Background = Brushes.Transparent;
            habitantOption.Opacity = 0.4;
            organizerOption.Opacity = 1;
            superAdminOption.Opacity = 1;
            newHabitantOption.Opacity = 1;
            habitantOption.Foreground = Brushes.DeepSkyBlue;
            organizerOption.Foreground = Brushes.White;
            superAdminOption.Foreground = Brushes.White;
            newHabitantOption.Foreground = Brushes.White;
            initialViewContent.Children.Clear();
            initialViewContent.Children.Add(new HabitantLoginForm("Test"));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            habitantOption.Background = Brushes.Transparent;
            organizerOption.Background = Brushes.White;
            superAdminOption.Background = Brushes.Transparent;
            newHabitantOption.Background = Brushes.Transparent;
            habitantOption.Opacity = 1;
            organizerOption.Opacity = 0.4;
            superAdminOption.Opacity = 1;
            newHabitantOption.Opacity = 1;
            habitantOption.Foreground = Brushes.White;
            organizerOption.Foreground = Brushes.DeepSkyBlue;
            superAdminOption.Foreground = Brushes.White;
            newHabitantOption.Foreground = Brushes.White;
            initialViewContent.Children.Clear();
            initialViewContent.Children.Add(new OrganizerLoginForm());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            habitantOption.Background = Brushes.Transparent;
            organizerOption.Background = Brushes.Transparent;
            superAdminOption.Background = Brushes.White;
            newHabitantOption.Background = Brushes.Transparent;
            habitantOption.Opacity = 1;
            organizerOption.Opacity = 1;
            superAdminOption.Opacity = 0.4;
            newHabitantOption.Opacity = 1;
            habitantOption.Foreground = Brushes.White;
            organizerOption.Foreground = Brushes.White;
            superAdminOption.Foreground = Brushes.DeepSkyBlue;
            newHabitantOption.Foreground = Brushes.White;
            initialViewContent.Children.Clear();
            initialViewContent.Children.Add(new SuperAdminLoginForm());
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void newHabitantOption_Click(object sender, RoutedEventArgs e)
        {
            habitantOption.Background = Brushes.Transparent;
            organizerOption.Background = Brushes.Transparent;
            superAdminOption.Background = Brushes.Transparent;
            newHabitantOption.Background = Brushes.White;
            habitantOption.Opacity = 1;
            organizerOption.Opacity = 1;
            superAdminOption.Opacity = 1;
            newHabitantOption.Opacity = 0.4;
            habitantOption.Foreground = Brushes.White;
            organizerOption.Foreground = Brushes.White;
            superAdminOption.Foreground = Brushes.White;
            newHabitantOption.Foreground = Brushes.DeepSkyBlue;
            initialViewContent.Children.Clear();
            initialViewContent.Children.Add(new SingIn());
        }
    }
}
