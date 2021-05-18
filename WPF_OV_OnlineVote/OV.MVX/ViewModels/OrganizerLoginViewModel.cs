using GalaSoft.MvvmLight.Messaging;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.Organizer.Find.Models.Public;
using OV.MVX.Helpers;
using OV.MVX.Services.Organizer;
using OV.Services.AES_Operation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Windows;
using static WPF_OV_OnlineVote.Helper.MessageHelper;

namespace OV.MVX.ViewModels
{
    public class OrganizerLoginViewModel : MvxViewModel, INotifyDataErrorInfo
    {
        public IMvxCommand LogInOrganizerCommand { get; set; }

        private string _dni_nie;
        private string _referenceNumber;
        private SecureString _password;
        private IOrganizerService _organizerService;

        public OrganizerLoginViewModel()
        {
            _organizerService = new OrganizerService();
            LogInOrganizerCommand = new MvxCommand(LogInOrganizer);
        }

        private async void LogInOrganizer()
        {
            var errors = ValidateData();
            bool isAnyError = errors.Count > 0 || HasErrors;
            if (isAnyError)
            {
                VisualizeError(errors);
            }
            else
            {
                var encryptedPassword = EncrypedPassword();
                var organizers = await _organizerService.FindAsync(OrganizerFilter.ByDNI_NIE_Password_ReferenceNumber(DNI_NIE, encryptedPassword, ReferenceNumber),
                                                                new CancellationToken());
                if (organizers.Count() > 0)
                {
                    Messenger.Default.Send(new NotificationMessage(MessageTypes.OrganiserLoginSuccess.ToString() + "=>" + organizers.FirstOrDefault().Id));
                }
                else
                {
                    MessageBox.Show("DNI/NIE, Contraseña o Número de referencía es incorecto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

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
        public string ReferenceNumber
        {
            get { return _referenceNumber; }
            set
            {
                SetProperty(ref _referenceNumber, value);
                ClearError(nameof(ReferenceNumber));
                if (string.IsNullOrEmpty(ReferenceNumber))
                {
                    AddError(nameof(ReferenceNumber), "Número de referencía es obligatorio");
                }
                RaisePropertyChanged(() => ReferenceNumber);
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

        private readonly Dictionary<string, List<string>> _propertyError = new Dictionary<string, List<string>>();

        public bool HasErrors => _propertyError.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public System.Collections.IEnumerable GetErrors(string propertyName)
        {
            return _propertyError.GetValueOrDefault(propertyName ?? "", null);
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyError.ContainsKey(propertyName))
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
        private void ClearError(string propertyName)
        {
            if (_propertyError.Remove(propertyName))
            {
                OnErrorChange(propertyName);
            }
        }


        private Dictionary<string, string> ValidateData()
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(DNI_NIE?.Trim()))
            {
                errors.Add("DNI/NIE", "DNI/NIE de usuario es obligatorio");
            }
            if (Password == null)
            {
                errors.Add("Contraseña", "Contraseña de usuario es obligatoria");
            }

            return errors;
        }

        private void VisualizeError(Dictionary<string, string> errors)
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
        }

        private string EncrypedPassword()
        {
            var password = SecureStringToString(Password);
            var aesKey = ConfigurationManager.AppSettings.Get("AesKey");
            return AES.EncryptString(aesKey, password);
        }
    }
}
