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
        public HabitantLoginForm(string text)
        {
            InitializeComponent();
            DataContext = new HabitantLoginViewModel(text);
        }
    }
}
