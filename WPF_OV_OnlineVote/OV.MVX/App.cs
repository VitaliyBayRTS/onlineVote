using MvvmCross.ViewModels;
using OV.MVX.ViewModels;

namespace OV.MVX
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            RegisterAppStart<InitialViewModel>();
        }
    }
}
