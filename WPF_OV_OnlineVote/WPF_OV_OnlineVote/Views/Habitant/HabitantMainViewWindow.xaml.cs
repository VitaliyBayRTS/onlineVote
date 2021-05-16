using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels.HabitantViewMode;
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
using System.Windows.Shapes;

namespace WPF_OV_OnlineVote.Views.Habitant
{
    /// <summary>
    /// Interaction logic for HabitantMainViewWindow.xaml
    /// </summary>
    public partial class HabitantMainViewWindow : MvxWindow
    {
        private MainHabitantViewModel mainHabitantViewModel;
        public HabitantMainViewWindow()
        {
            InitializeComponent();
        }

        public void LoadDataContext(int Habitant_UID)
        {
            mainHabitantViewModel = new MainHabitantViewModel();
            mainHabitantViewModel.Habitant_UID = Habitant_UID;
            DataContext = mainHabitantViewModel;
        }
    }
}
