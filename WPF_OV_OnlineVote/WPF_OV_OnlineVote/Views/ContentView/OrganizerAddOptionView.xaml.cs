using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace WPF_OV_OnlineVote.Views.ContentView
{
    /// <summary>
    /// Interaction logic for OrganizerAddOptionView.xaml
    /// </summary>
    public partial class OrganizerAddOptionView : MvxWpfView
    {
        public OrganizerAddOptionView()
        {
            InitializeComponent();
            disableEditMode();
        }

        private void editOptionBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            enableEditMode();
        }

        private void cancelEditModeBtn_Click(object sender, RoutedEventArgs e)
        {
            disableEditMode();
        }

        private void enableEditMode()
        {
            editOptionBtn.Visibility = Visibility.Collapsed;
            deleteOptionBtn.Visibility = Visibility.Collapsed;
            modifyOptionBtn.Visibility = Visibility.Visible;
            cancelEditModeBtn.Visibility = Visibility.Visible;

            nameOptionLabel.Visibility = Visibility.Collapsed;
            descriptionOptionLabel.Visibility = Visibility.Collapsed;
            nameOptionTB.Visibility = Visibility.Visible;
            descriptionOptionTB.Visibility = Visibility.Visible;
        }
        private void disableEditMode()
        {
            editOptionBtn.Visibility = Visibility.Visible;
            deleteOptionBtn.Visibility = Visibility.Visible;
            modifyOptionBtn.Visibility = Visibility.Collapsed;
            cancelEditModeBtn.Visibility = Visibility.Collapsed;

            nameOptionLabel.Visibility = Visibility.Visible;
            descriptionOptionLabel.Visibility = Visibility.Visible;
            nameOptionTB.Visibility = Visibility.Collapsed;
            descriptionOptionTB.Visibility = Visibility.Collapsed;
        }
    }
}
