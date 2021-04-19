using MvvmCross.Platforms.Wpf.Views;

namespace WPF_OV_OnlineVote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        //private OvMainDbContext _ovMainDbContext;
        //OvMainDbContext ovMainDbContext
        public MainWindow()
        {
            //_ovMainDbContext = ovMainDbContext ?? throw new ArgumentNullException(nameof(ovMainDbContext));
            InitializeComponent();

            //var test = _ovMainDbContext.Provinces.ToList();
            //test = test.Where(p => p.Name == "Malaga").ToList();
        }
    }
}
