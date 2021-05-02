using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.AutonomousCommunity.Find;
using OV.Models.MainDb.AutonomousCommunity;
using OV.Models.MainDb.Province;
using OV.MVX.Helpers;
using OV.MVX.Services.AutonomousCommunity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows;

namespace OV.MVX.ViewModels
{
    public class SingupViewModel : MvxViewModel, INotifyDataErrorInfo
    {

        public IMvxCommand CreateUserCommand { get; set; }

        private string _name = "TestName";
        private string _firstSurName = "TestFirstSurname";
        private string _secondSurName = "TestSecondSurname";
        private DateTime _dateOfBirth = DateTime.Today;
        private string _dateOfBirthString = DateTime.Today.ToString("dd/MM/yyyy");
        private ObservableCollection<AutonomousCommunity> _allAutonomousCommunities = new ObservableCollection<AutonomousCommunity>();
        private AutonomousCommunity _autonomousCommunity;
        private ObservableCollection<Province> _allProvinces = new ObservableCollection<Province>();
        private ObservableCollection<Province> _provincesOfCommunity = new ObservableCollection<Province>();
        private Province _province;
        private string _email = "TestEmail";
        private string _phoneNumber = "TestPhoneNumber";
        private string _dni_nie = "TestDNI_NIE";
        private SecureString _password;
        private SecureString _confirmPassword;

        public string Name
        {
           
            get { return _name; }
            set 
            { 
                SetProperty(ref _name, value);
                if(string.IsNullOrEmpty(_name))
                {

                }
            }
        }
        public string FirstSurName
        {
            get { return _firstSurName; }
            set { SetProperty(ref _firstSurName, value); }
        }
        public string SecondSurName
        {
            get { return _secondSurName; }
            set { SetProperty(ref _secondSurName, value); }
        }
        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set 
            { 
                SetProperty(ref _dateOfBirth, value);
                _dateOfBirthString = value.ToString("dd/MM/yyyy");
            }
        }
        public string DateOfBirthString
        {
            get { return _dateOfBirthString; }
            set 
            { 
                SetProperty(ref _dateOfBirthString, value);
            }
        }
        public ObservableCollection<AutonomousCommunity> AllAutonomousCommunities
        {
            get { return _allAutonomousCommunities; }
            set { SetProperty(ref _allAutonomousCommunities, value); }
        }
        public AutonomousCommunity AutonomousCommunity
        {
            get { return _autonomousCommunity; }
            set
            {
                SetProperty(ref _autonomousCommunity, value);
                RaisePropertyChanged(() => AutonomousCommunity);
                if(value != null)
                {
                    _provincesOfCommunity = new ObservableCollection<Province>(value?.Provinces.OrderBy(p => p.Name));
                }
                RaisePropertyChanged(() => ProvincesOfCommunity);
            }
        }
        public ObservableCollection<Province> ProvincesOfCommunity
        {
            get { return _provincesOfCommunity; }
            set
            {
                SetProperty(ref _provincesOfCommunity, value);
            }
        }
        public Province Province
        {
            get { return _province; }
            set
            {
                SetProperty(ref _province, value);
                RaisePropertyChanged(() => Province);
            }
        }
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }
        public string DNI_NIE
        {
            get { return _dni_nie; }
            set { SetProperty(ref _dni_nie, value); }
        }
        public SecureString Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }
        public SecureString ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }


        private IAutonomousCommunityService autonomousCommunityService;


        public SingupViewModel()
        {
            autonomousCommunityService = new AutonomousCommunityService();
            var autonomousCommunities = autonomousCommunityService.Find();
            _allAutonomousCommunities = new ObservableCollection<AutonomousCommunity>(autonomousCommunities);

            CreateUserCommand = new MvxCommand(createUser);
        }

        public async void createUser()
        {
            //var name = Name;
            //var firstSurName = FirstSurName;
            //var secondSurName = SecondSurName;
            //var dateOfBirth = DateOfBirth;
            //var autonomousCommunity = AutonomousCommunity;
            //var province = Province;
            //var email = Email;
            //var phoneNumber = PhoneNumber;
            //var DNI = DNI_NIE;
            //var password = SecureStringToString(Password);
            //var confirmPassword = SecureStringToString(ConfirmPassword);

            var errors = ValidateData();
            if (errors.Count > 0)
            {
                var errorText = "";
                foreach (var error in errors.OrderBy(_ => _.Key))
                {
                    errorText += error.Key + " : " + error.Value + "\r\n";
                }
                MessageBox.Show(errorText, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private Dictionary<string, string> ValidateData()
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if(Name.Trim().Length <= 0)
            {
                errors.Add("Nombre", "Nombre de usuario es obligatorio");
            }
            if(FirstSurName.Trim().Length <= 0)
            {
                errors.Add("Primer Apellido", "Primer Apellido de usuario es obligatorio");
            }
            if(DateOfBirth == null)
            {
                errors.Add("Fecha De nacimiento", "Fecha De nacimiento de usuario es obligatoria");
            }
            if(AutonomousCommunity == null)
            {
                errors.Add("Comunidad autonoma", "Comunidad autonoma de usuario es obligatoria");
            }
            if(Province == null)
            {
                errors.Add("Provincia", "Provincia de usuario es obligatoria");
            }
            if(Email.Trim().Length <= 0)
            {
                errors.Add("Correo electronico", "Correo electronico de usuario es obligatorio");
            }
            if(PhoneNumber.Trim().Length <= 0)
            {
                errors.Add("Numero de telefono", "Numero de telefono de usuario es obligatorio");
            }
            if(DNI_NIE.Trim().Length <= 0)
            {
                errors.Add("DNI/NIE", "DNI/NIE de usuario es obligatorio");
            }
            if(Password == null)
            {
                errors.Add("Contraseña", "Contraseña de usuario es obligatoria");
            } else if(ConfirmPassword == null || !SecureStringToString(Password).Trim().Equals(SecureStringToString(ConfirmPassword).Trim()))
            {
                errors.Add("Confirmar contraseña", "La contraseña no coincide");
            }

            return errors;
        }

        private readonly Dictionary<string, List<string>> _propertyError = new Dictionary<string, List<string>>(); 

        static String SecureStringToString(SecureString value)
        {
            IntPtr bstr = Marshal.SecureStringToBSTR(value);

            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }

        public bool HasErrors => _propertyError.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return _propertyError.GetValueOrDefault(propertyName, null);
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if(!_propertyError.ContainsKey(propertyName))
            {
                _propertyError.Add(propertyName, new List<string>());
            }

            _propertyError[propertyName].Add(errorMessage);
            OnErrorChange(propertyName);
        }

        private void OnErrorChange(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
