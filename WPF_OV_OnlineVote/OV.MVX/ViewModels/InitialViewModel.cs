using MvvmCross.ViewModels;
using OV.MVX.Models;
using System.Collections.ObjectModel;

namespace OV.MVX.ViewModels
{
    public class InitialViewModel : MvxViewModel
    {
        private ObservableCollection<InitialModel> _inistials = new ObservableCollection<InitialModel>();

        public ObservableCollection<InitialModel> Initials
        {
            get { return _inistials; }
            set { SetProperty(ref _inistials, value); }
        }

        private string _firstProp;

        public string FirstProp
        {
            get { return _firstProp; }
            set { SetProperty(ref _firstProp, value); }
        }


    }
}
