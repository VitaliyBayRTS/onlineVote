using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OV.MainDb.Configuration;
using System.Windows;

namespace WPF_OV_OnlineVote
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        //private ServiceProvider serviceProvider;

        //public App()
        //{
        //    ServiceCollection services = new ServiceCollection();
        //    ConfigureServices(services);
        //    serviceProvider = services.BuildServiceProvider();
        //}

        //private void ConfigureServices(ServiceCollection services)
        //{
        //    services.AddDbContext<OvMainDbContext>(options =>
        //    {
        //        options.UseSqlServer("Data Source=DESKTOP-O2K81VH;Initial Catalog=OV_MainDb;Integrated Security=True");
        //    });

        //    services.AddSingleton<MainWindow>();
        //}

        //private void OnStartup(object sender, StartupEventArgs e)
        //{
        //    var mainWindow = serviceProvider.GetService<MainWindow>();
        //    mainWindow.Show();
        //}

        protected override void RegisterSetup()
        {
            this.RegisterSetupType<MvxWpfSetup<OV.MVX.App>>();
        }
    }
}
