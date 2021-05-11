using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.AutonomousCommunity.Find;
using OV.MainDb.AutonomousCommunity.Find.Models.Public;
using OV.MainDb.User.Create.Models.Public;
using OV.MainDb.User.Models.Public;
using OV.Models.MainDb.AutonomousCommunity;
using OV.Models.MainDb.Province;
using OV.MVX.Helpers;
using OV.MVX.Services.AutonomousCommunity;
using OV.MVX.Services.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows;

namespace OV.MVX.ViewModels
{
    public class SingupViewModel : MvxViewModel, INotifyDataErrorInfo
    {
        public IMvxCommand CreateUserCommand { get; set; }

        private string _firstName;
        private string _secondName;
        private string _firstSurName;
        private string _secondSurName;
        private DateTime _dateOfBirth = DateTime.Today;
        private string _dateOfBirthString = DateTime.Today.ToString("dd/MM/yyyy");
        private ObservableCollection<AutonomousCommunity> _allAutonomousCommunities = new ObservableCollection<AutonomousCommunity>();
        private AutonomousCommunity _autonomousCommunity;
        private ObservableCollection<Province> _provincesOfCommunity = new ObservableCollection<Province>();
        private Province _province;
        private string _email;
        private string _phoneNumber;
        private string _dni_nie;
        private SecureString _password;
        private SecureString _confirmPassword;

        public string FirstName
        {
           
            get { return _firstName; }
            set 
            { 
                SetProperty(ref _firstName, value);
                ClearError(nameof(FirstName));
                if(string.IsNullOrEmpty(_firstName))
                {
                    AddError(nameof(FirstName), "Primer Nombre no puede ser vacio");
                }
                if(!System.Text.RegularExpressions.Regex.IsMatch(_firstName, @"^[a-zA-ZñÑ áÁ óÓ éÉ íÍ úÚ]+$"))
                {
                    AddError(nameof(FirstName), "Solo caracteres alfabeticos");
                }
                RaisePropertyChanged(() => FirstName);
            }
        }
        public string SecondName
        {
           
            get { return _secondName; }
            set 
            { 
                SetProperty(ref _secondName, value);
                ClearError(nameof(SecondName));
                if(!System.Text.RegularExpressions.Regex.IsMatch(_secondName, @"^[a-zA-ZñÑ áÁ óÓ éÉ íÍ úÚ]+$"))
                {
                    AddError(nameof(SecondName), "Solo caracteres alfabeticos");
                }
                RaisePropertyChanged(() => SecondName);
            }
        }

        private void ClearError(string propertyName)
        {
            if (_propertyError.Remove(propertyName))
            {
                OnErrorChange(propertyName);
            }
        }

        public string FirstSurName
        {
            get { return _firstSurName; }
            set 
            { 
                SetProperty(ref _firstSurName, value);
                if (string.IsNullOrEmpty(_firstSurName))
                {
                    AddError(nameof(FirstSurName), "Primer Apellido no puede ser vacio");
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(_firstSurName, @"^[a-zA-ZñÑ áÁ óÓ éÉ íÍ úÚ]+$"))
                {
                    AddError(nameof(FirstSurName), "Solo caracteres alfabeticos");
                }
                RaisePropertyChanged(() => FirstSurName);
            }
        }
        public string SecondSurName
        {
            get { return _secondSurName; }
            set
            {
                SetProperty(ref _secondSurName, value);
                if (!System.Text.RegularExpressions.Regex.IsMatch(_secondSurName, @"^[a-zA-ZñÑ áÁ óÓ éÉ íÍ úÚ]+$"))
                {
                    AddError(nameof(SecondSurName), "Solo caracteres alfabeticos");
                }
                RaisePropertyChanged(() => SecondSurName);
            }
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
            set 
            {
                SetProperty(ref _email, value);
                ClearError(nameof(Email));
                if (!IsValidEmail(_email))
                {
                    AddError(nameof(Email), "Correo incorrecto");
                }
                RaisePropertyChanged(() => Email);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set 
            { 
                SetProperty(ref _phoneNumber, value);
                ClearError(nameof(PhoneNumber));
                if (!System.Text.RegularExpressions.Regex.IsMatch(_phoneNumber, @"^[0-9]+$"))
                {
                    AddError(nameof(PhoneNumber), "Incorrect number");
                }
                RaisePropertyChanged(() => PhoneNumber);
            }
        }
        public string DNI_NIE
        {
            get { return _dni_nie; }
            set 
            { 
                SetProperty(ref _dni_nie, value);
                ClearError(nameof(DNI_NIE));
                if (!DocumentValidation.isValidDocument(_dni_nie))
                {
                    AddError(nameof(DNI_NIE), "Incorrect DNI/NIE");
                }
                RaisePropertyChanged(() => DNI_NIE);
            }
        }
        public SecureString Password
        {
            get { return _password; }
            set
            {
                SetProperty(ref _password, value);
                ClearError(nameof(Password));
                if (Password.Length < 9)
                {
                    AddError(nameof(Password), "La longitud minima de la contraseña es: 9 caracteres");
                }
                RaisePropertyChanged(() => Password);
            }
        }
        public SecureString ConfirmPassword
        {
            get { return _confirmPassword; }
            set { SetProperty(ref _confirmPassword, value); }
        }


        private IAutonomousCommunityService _autonomousCommunityService;
        private IUserService _userService;


        public SingupViewModel()
        {
            _autonomousCommunityService = new AutonomousCommunityService();
            _allAutonomousCommunities = new ObservableCollection<AutonomousCommunity>();
            LoadData();
            _userService = new UserService();
            CreateUserCommand = new MvxCommand(createUser);
        }

        public async void LoadData()
        {
            var autonomousCommunities = await _autonomousCommunityService.FindAsync(AutonomousCommunityFilter.All.AndIncludeProvince(), new CancellationToken());
            AllAutonomousCommunities = new ObservableCollection<AutonomousCommunity>(autonomousCommunities);
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
            //if (errors.Count > 0 || HasErrors)
            if (false)
            {
                var errorText = "";
                foreach (var error in errors.OrderBy(_ => _.Key))
                {
                    errorText += "- " + error.Key + " : " + error.Value + "\r\n\r\n";
                } 

                foreach (var error in _propertyError.OrderBy(_ => _.Key))
                {
                    var errorItemText = "";
                    foreach (var errorItem in error.Value)
                    {
                        errorItemText += "- " + errorItem + "\r\n\r\n";
                    }
                    errorText += "- " + error.Key + " : " + errorItemText + "\r\n\r\n";
                }
                MessageBox.Show(errorText, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            } else
            {
                CandidateUser candidate = new CandidateUser()
                {
                    FirstName = FirstName + "asd",
                    SecondName = SecondName + "asd",
                    SurName = FirstSurName + "asd",
                    SecondSurName = SecondSurName + "asd",
                    //Password = SecureStringToString(Password),
                    Password = "asd",
                    DOB = DateOfBirth,
                    TblAutonomousCommunities_UID = 1,
                    TblProvince_UID = 26,
                    Email = Email + "asd",
                    PhoneNumber = PhoneNumber + "asd"
                };
                var response = await _userService.CreateUserAsync(candidate, new CancellationToken());
                if (response is CreateUserFailure failure)
                {
                    var failureText = "";
                    foreach (var failureReason in failure.FailureReasons)
                    {
                        failureText += failureReason.PropertyName + "\r\n";
                    }
                    MessageBox.Show(failureText, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                MessageBox.Show("Ok", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
           

        }

        private Dictionary<string, string> ValidateData()
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(FirstName?.Trim()))
            {
                errors.Add("Nombre", "Nombre de usuario es obligatorio");
            }
            if (string.IsNullOrEmpty(FirstSurName?.Trim()))
            {
                errors.Add("Primer Apellido", "Primer Apellido de usuario es obligatorio");
            }
            if(DateOfBirth == null)
            {
                errors.Add("Fecha De nacimiento", "Fecha De nacimiento de usuario es obligatoria");
            }
            if (DateOfBirth > DateTime.Now)
            {
                errors.Add("Fecha De nacimiento", "Fecha De nacimiento tiene que ser hasta " + DateTime.Today);
            }
            if (AutonomousCommunity == null)
            {
                errors.Add("Comunidad autonoma", "Comunidad autonoma de usuario es obligatoria");
            }
            if(Province == null)
            {
                errors.Add("Provincia", "Provincia de usuario es obligatoria");
            }
            if(string.IsNullOrEmpty(Email?.Trim()))
            {
                errors.Add("Correo electronico", "Correo electronico de usuario es obligatorio");
            }
            if(string.IsNullOrEmpty(PhoneNumber?.Trim()))
            {
                errors.Add("Numero de telefono", "Numero de telefono de usuario es obligatorio");
            }
            if(string.IsNullOrEmpty(DNI_NIE?.Trim()))
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
            return _propertyError.GetValueOrDefault(propertyName ?? "", null);
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
