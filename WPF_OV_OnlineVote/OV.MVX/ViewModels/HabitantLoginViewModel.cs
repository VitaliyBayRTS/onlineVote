using MvvmCross.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace OV.MVX.ViewModels
{
    public class HabitantLoginViewModel : MvxViewModel
    {
        private string _dni = "dniTest";
        private string _password = "PasswordTest"; 
        private ObservableCollection<string> _comunities = new ObservableCollection<string>(); 
        private string _selectedComunity; 
        private ObservableCollection<string> _allProvinces = new ObservableCollection<string>(); 
        private ObservableCollection<string> _provinces = new ObservableCollection<string>(); 
        private string _selectedProvince; 

        public HabitantLoginViewModel(string text)
        {
            _dni += text;
            _comunities.Add("A");
            _comunities.Add("B");
            _comunities.Add("C");
            _comunities.Add("D");
            _allProvinces.Add("A1");
            _allProvinces.Add("A2");
            _allProvinces.Add("B1");
            _allProvinces.Add("B2");
            _allProvinces.Add("C1");
            _allProvinces.Add("C2");
            _allProvinces.Add("D1");
            _allProvinces.Add("C2");
        }

        public string DNI
        {
            get { return _dni; }
            set { SetProperty(ref _dni, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public ObservableCollection<string> Comunities {
            get { return _comunities; }
            set
            {
                SetProperty(ref _comunities, value);
            }
        }

        public string SelectedComunity 
        {
            get { return _selectedComunity; } 
            set
            {
                SetProperty(ref _selectedComunity, value);
                RaisePropertyChanged(() => SelectedComunity);
                _provinces = new ObservableCollection<string>(_allProvinces.Where(p => p.StartsWith(value ?? "_")));
                RaisePropertyChanged(() => Provinces);
            } 
        }
        public ObservableCollection<string> Provinces {
            get { return _provinces; }
            set
            {
                SetProperty(ref _provinces, value);
            }
        }

        public string SelectedProvince 
        {
            get { return _selectedProvince; } 
            set
            {
                SetProperty(ref _selectedProvince, value);
                RaisePropertyChanged(() => SelectedProvince);
            } 
        }

    }
}
