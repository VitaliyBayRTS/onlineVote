using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels;
using System.Windows.Controls;

namespace WPF_OV_OnlineVote.Views.Login
{
    /// <summary>
    /// Interaction logic for HabitantLoginForm.xaml
    /// </summary>
    public partial class HabitantLoginForm : MvxWpfView
    {
        public HabitantLoginForm()
        {
            InitializeComponent();
            DataContext = new HabitantLoginViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword; }
        }
    }
}
