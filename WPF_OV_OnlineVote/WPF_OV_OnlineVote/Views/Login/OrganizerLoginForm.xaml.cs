﻿using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels;
using OV.MVX.ViewModels.OrganizerViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            DataContext = new OrganizerLoginViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((dynamic)this.DataContext).Password = ((PasswordBox)sender).SecurePassword; }
        }
    }
}
