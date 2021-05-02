using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.Configuration;
using System;
using System.Linq;
using System.Windows.Input;

namespace WPF_OV_OnlineVote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        private OvMainDbContext _ovMainDbContext;
        //OvMainDbContext ovMainDbContext
        public MainWindow()
        {
            //_ovMainDbContext = ovMainDbContext ?? throw new ArgumentNullException(nameof(ovMainDbContext));
            InitializeComponent();
            //var test = _ovMainDbContext.AutonomousCommunities.ToList();
        }

        private void MvxWindow_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
