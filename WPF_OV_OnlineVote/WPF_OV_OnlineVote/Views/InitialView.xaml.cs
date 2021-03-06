using GalaSoft.MvvmLight.Messaging;
using MvvmCross.Platforms.Wpf.Views;
using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.Configuration;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPF_OV_OnlineVote.Views.Login;
using WPF_OV_OnlineVote.Views.ContentView;
using static WPF_OV_OnlineVote.Helper.MessageHelper;

namespace WPF_OV_OnlineVote.Views
{
    /// <summary>
    /// Interaction logic for InitialView.xaml
    /// </summary>
    public partial class InitialView : MvxWpfView
    {
        private OvMainDbContext _ovMainDbContext;
        private IFindAutonomousCommunityDataService _findAutonomousCommunityService;
        //OvMainDbContext ovMainDbContext
        public InitialView()
        {
            //_findAutonomousCommunityService = findAutonomousCommunityService ?? throw new ArgumentNullException(nameof(findAutonomousCommunityService));
            //var test = _findAutonomousCommunityService.Find();
            //var test = _ovMainDbContext.AutonomousCommunities.ToList();
            InitializeComponent();
            Messenger.Default.Register<NotificationMessage>(this, NotificationMessageReceived);
        }

        private void NotificationMessageReceived(NotificationMessage obj)
        {
            if (obj.Notification == MessageTypes.SingUpSuccess.ToString())
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
                initialViewContent.Children.Add(new HabitantLoginForm());
            }
            else if(obj.Notification.Contains(MessageTypes.HabitantLoginSuccess.ToString()))
            {
                var splitedNotification = obj.Notification.Split("=>");
                var habitant_UID = splitedNotification[1];

                HabitantMainViewWindow habitantMainView = new HabitantMainViewWindow(Int32.Parse(habitant_UID));
                //habitantMainView.LoadDataContext(Int32.Parse(habitant_UID));
                Application.Current.Windows[0].Hide();
                habitantMainView.ShowDialog();
            }
            else if(obj.Notification.Contains(MessageTypes.OrganiserLoginSuccess.ToString()))
            {
                var splitedNotification = obj.Notification.Split("=>");
                var message = splitedNotification[1];
                var organizer_UID = message.Split("|")[0];
                var election_UID = message.Split("|")[1];

                OrganizerMainViewModel organizedMainView = new OrganizerMainViewModel();
                organizedMainView.LoadDataContext(Int32.Parse(organizer_UID), Int32.Parse(election_UID));
                Application.Current.Windows[0].Hide();
                organizedMainView.ShowDialog();
            }
            else if(obj.Notification.Contains(MessageTypes.SuperAdminLoginSuccess.ToString()))
            {
                var splitedNotification = obj.Notification.Split("=>");
                var superAdmin_UID = splitedNotification[1];

                SuperAdminMainViewWindow superadminMainView = new SuperAdminMainViewWindow();
                superadminMainView.LoadDataContext(Int32.Parse(superAdmin_UID));
                Application.Current.Windows[0].Hide();
                superadminMainView.ShowDialog();
            }
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
            initialViewContent.Children.Add(new HabitantLoginForm());
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
            var loadingText = new TextBlock();
            loadingText.Text = "Loading";
            initialViewContent.Children.Add(loadingText);
            var singInGrid = new SingIn();
            singInGrid.LoadDataContext();
            initialViewContent.Children.Clear();
            initialViewContent.Children.Add(singInGrid);
        }
    }
}
