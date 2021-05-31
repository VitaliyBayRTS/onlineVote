using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace WPF_OV_OnlineVote.Views.Login
{
    /// <summary>
    /// Interaction logic for OrganizerLoginForm.xaml
    /// </summary>
    public partial class OrganizerLoginForm : MvxWpfView
    {
        public OrganizerLoginForm()
        {
            InitializeComponent();
            DataContext = new OrganizerLoginViewModel(organizerSecureCode);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword; }
        }
    }
}
