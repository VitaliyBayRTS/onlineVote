using MvvmCross.Platforms.Wpf.Views;
using System.Windows;

namespace WPF_OV_OnlineVote.Views.ContentView
{
    /// <summary>
    /// Interaction logic for OrganizerEdirElectionView.xaml
    /// </summary>
    public partial class OrganizerEditElectionView : MvxWpfView
    {
        public OrganizerEditElectionView()
        {
            InitializeComponent();
            saveBtn.Visibility = Visibility.Collapsed;
            cancelEditModeBtn.Visibility = Visibility.Collapsed;
            deleteOptionBtn.Visibility = Visibility.Collapsed;
        }

        private void editBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            enableEditMode();
        }

        private void cancelEditModeBtn_Click(object sender, RoutedEventArgs e)
        {
            disableEditMode();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            disableEditMode();
        }

        private void enableEditMode()
        {
            nameLabel.Visibility = Visibility.Hidden;
            descriptionLabel.Visibility = Visibility.Hidden;
            initDateLabel.Visibility = Visibility.Hidden;
            finishDateLabel.Visibility = Visibility.Hidden;

            nameTB.Visibility = Visibility.Visible;
            descriptionTB.Visibility = Visibility.Visible;
            initDateDT.Visibility = Visibility.Visible;
            finishDateDP.Visibility = Visibility.Visible;

            editBtn.Visibility = Visibility.Collapsed;
            saveBtn.Visibility = Visibility.Visible;
            cancelEditModeBtn.Visibility = Visibility.Visible;
            deleteOptionBtn.Visibility = Visibility.Visible;
        }
        private void disableEditMode()
        {
            nameLabel.Visibility = Visibility.Visible;
            descriptionLabel.Visibility = Visibility.Visible;
            initDateLabel.Visibility = Visibility.Visible;
            finishDateLabel.Visibility = Visibility.Visible;

            nameTB.Visibility = Visibility.Hidden;
            descriptionTB.Visibility = Visibility.Hidden;
            initDateDT.Visibility = Visibility.Hidden;
            finishDateDP.Visibility = Visibility.Hidden;

            editBtn.Visibility = Visibility.Visible;
            saveBtn.Visibility = Visibility.Collapsed;
            cancelEditModeBtn.Visibility = Visibility.Collapsed;
            deleteOptionBtn.Visibility = Visibility.Collapsed;
        }
    }
}
