using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace WPF_OV_OnlineVote.Views.Login
{
    /// <summary>
    /// Interaction logic for SuperAdminLoginForm.xaml
    /// </summary>
    public partial class SuperAdminLoginForm : MvxWpfView
    {
        public SuperAdminLoginForm()
        {
            InitializeComponent();
            DataContext = new SuperAdminLoginViewModel(password);
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword; }
        }
    }
}
