using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using OV.MainDb.AutonomousCommunity.Find;
using OV.MVX.ViewModels;
using System.Windows.Controls;

namespace WPF_OV_OnlineVote.Views.Login
{
    /// <summary>
    /// Interaction logic for SingIn.xaml
    /// </summary>
    public partial class SingIn : MvxWpfView
    {
        private SingupViewModel singupViewModel;
        public SingIn()
        {
            InitializeComponent();
        }

        public void LoadDataContext()
        {
            singupViewModel = new SingupViewModel();
            DataContext = singupViewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword; }
        }

        private void PasswordBox_PasswordChanged_1(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).ConfirmPassword = ((PasswordBox)sender).SecurePassword; }
        }
    }
}
