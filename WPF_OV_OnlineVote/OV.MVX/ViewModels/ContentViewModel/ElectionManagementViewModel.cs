using Caliburn.Micro;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.AutonomousCommunity.Find.Models.Public;
using OV.MainDb.Election.Create.Models.Public;
using OV.MainDb.Election.Models.Public;
using OV.MainDb.Organizer.Find.Models.Public;
using OV.MainDb.Organizer.Models.Public;
using OV.MainDb.Type.Find.Models.Public;
using OV.MainDb.User.Find.Models.Public;
using OV.Models.MainDb.AutonomousCommunity;
using OV.Models.MainDb.Election;
using OV.Models.MainDb.Province;
using OV.Models.MainDb.Type;
using OV.MVX.Models;
using OV.MVX.Services.AutonomousCommunity;
using OV.MVX.Services.Election;
using OV.MVX.Services.Organizer;
using OV.MVX.Services.Type;
using OV.MVX.Services.User;
using OV.Services.DocumentValidator;
using OV.Services.Email;
using OV.Services.ReferenceNumber;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class ElectionManagementViewModel : MvxViewModel, INotifyDataErrorInfo
    {
        //!Commands
        public IMvxCommand OVNacionalLevel { get; set; }
        public IMvxCommand OVACLevel { get; set; }
        public IMvxCommand OVProvinceLevel { get; set; }
        public IMvxCommand SelectUserCommand { get; set; }
        public IMvxCommand RemoveUserCommand { get; set; }
        public IMvxCommand CreateElectionCommand { get; set; }

        //!Services Dependency
        private IAutonomousCommunityService _autonomousCommunityService;
        private IUserService _userService;
        private IElectionService _electionService;
        private ITypeService _typeService;
        private IOrganizerService _organizerService;

        //!Private variables
        private bool _isNotNacionalLevel;
        private bool _provinceEnable;
        private bool _acEnable;
        private string _name;
        private string _description;
        private ObservableCollection<AutonomousCommunity> _allAutonomousCommunities = new ObservableCollection<AutonomousCommunity>();
        private ObservableCollection<Province> _provincesOfCommunity = new ObservableCollection<Province>();
        private DateTime _initDate = DateTime.Today.AddDays(7);
        private DateTime _finishDate = DateTime.Today.AddDays(8);
        private string _initDateString = DateTime.Today.ToString("dd/MM/yyyy");
        private string _finishDateString = DateTime.Today.ToString("dd/MM/yyyy");
        private AutonomousCommunity _autonomousCommunity;
        private Province _province;
        private bool _allowAddOrganizer = true;
        private string _searchByDNI_NIE_Value;
        private BindableCollection<AutorizedUserModel> _selectedUsers;
        private BindableCollection<AutorizedUserModel> _users;
        private AutorizedUserModel _selectedUser;
        private AutorizedUserModel _unselectedUser;
        private readonly Dictionary<string, List<string>> _propertyError = new Dictionary<string, List<string>>();

        //!Properties
        public OV_Types ovType { get; set; }
        public bool IsNotNacionalLevel
        {
            get
            {
                return _isNotNacionalLevel;
            }
            set
            {
                SetProperty(ref _isNotNacionalLevel, value);
                RaisePropertyChanged(() => IsNotNacionalLevel);
            }
        }
        public bool ProvinceEnable
        {
            get
            {
                return _provinceEnable;
            }
            set
            {
                SetProperty(ref _provinceEnable, value);
                RaisePropertyChanged(() => ProvinceEnable);
            }
        }
        public bool ACEnable
        {
            get
            {
                return _acEnable;
            }
            set
            {
                SetProperty(ref _acEnable, value);
                RaisePropertyChanged(() => ACEnable);
            }
        }
        public bool AllowAddOrganizer
        {
            get { return _allowAddOrganizer; }
            set
            {
                SetProperty(ref _allowAddOrganizer, value);
                RaisePropertyChanged(() => AllowAddOrganizer);
            }
        }
        public string SearchByDNI_NIE_Value
        {
            get { return _searchByDNI_NIE_Value; }
            set
            {
                SetProperty(ref _searchByDNI_NIE_Value, value);
                FilterUsers();
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {

                SetProperty(ref _name, value);
                ClearError(nameof(Name));
                if (string.IsNullOrEmpty(_name))
                {
                    AddError(nameof(Name), "Nombre no puede ser vacio");
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(_name, @"^[a-zA-ZñÑ áÁ óÓ éÉ íÍ úÚ]+$"))
                {
                    AddError(nameof(Name), "Nombre puede tener solo caracteres alfabeticos");
                }
                RaisePropertyChanged(() => Name);
            }
        }
        public string Description
        {
            get { return _description; }
            set
            {
                SetProperty(ref _description, value);
                RaisePropertyChanged(() => Description);
            }
        }
        public BindableCollection<AutorizedUserModel> SelectedUsers
        {
            get
            {
                return _selectedUsers;
            }
            set
            {
                SetProperty(ref _selectedUsers, value);
                RaisePropertyChanged(() => SelectedUsers);
            }
        }
        public BindableCollection<AutorizedUserModel> Users
        {
            get
            {
                return _users;
            }
            set
            {
                SetProperty(ref _users, value);
                RaisePropertyChanged(() => Users);
            }
        }
        public BindableCollection<AutorizedUserModel> AllUsers { get; set; }
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
                if (value != null)
                {
                    _provincesOfCommunity = new ObservableCollection<Province>(value?.Provinces.OrderBy(p => p.Name));
                }
                RaisePropertyChanged(() => ProvincesOfCommunity);
                removeFromSelectedUsers();
                FilterUsers();
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
                removeFromSelectedUsers();
                FilterUsers();
            }
        }
        public AutorizedUserModel SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                SetProperty(ref _selectedUser, value);
                RaisePropertyChanged(() => SelectedUser);
            }
        }
        public AutorizedUserModel UnselectedUser
        {
            get
            {
                return _unselectedUser;
            }
            set
            {
                SetProperty(ref _unselectedUser, value);
                RaisePropertyChanged(() => UnselectedUser);
            }
        }
        public bool HasErrors => _propertyError.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public DateTime StartDateTime { get; set; } = DateTime.Today.AddDays(7);
        public DateTime FinishElectionDateTime { get { return _initDate.AddDays(1); } }
        public DateTime InitDate
        {
            get { return _initDate; }
            set
            {
                SetProperty(ref _initDate, value);
                InitDateString = value.ToString("dd/MM/yyyy");
                if (_initDate > FinishDate)
                {
                    FinishDate = _initDate.AddDays(1);
                    FinishDateString = InitDateString;
                }
                RaisePropertyChanged(() => InitDate);
            }
        }
        public string InitDateString
        {
            get { return _initDate.ToString("dd/MM/yyyy"); }
            set
            {
                SetProperty(ref _initDateString, value);
                RaisePropertyChanged(() => InitDateString);
            }
        }
        public DateTime FinishDate
        {
            get { return _finishDate; }
            set
            {
                SetProperty(ref _finishDate, value);
                FinishDateString = value.ToString("dd/MM/yyyy");
                RaisePropertyChanged(() => FinishDate);
            }
        }
        public string FinishDateString
        {
            get { return _finishDate.ToString("dd/MM/yyyy"); }
            set
            {
                SetProperty(ref _finishDateString, value);
                RaisePropertyChanged(() => FinishDateString);
            }
        }


        public ElectionManagementViewModel()
        {
            SelectedUsers = new BindableCollection<AutorizedUserModel>();
            _autonomousCommunityService = new AutonomousCommunityService();
            _userService = new UserService();
            _electionService = new ElectionService();
            _typeService = new TypeService();
            _organizerService = new OrganizerService();
            OVNacionalLevel = new MvxCommand(ovNationalLevel);
            OVACLevel = new MvxCommand(ovacLevel);
            OVProvinceLevel = new MvxCommand(ovProvinceLevel);
            SelectUserCommand = new MvxCommand(SelectUser);
            RemoveUserCommand = new MvxCommand(RemoveUser);
            CreateElectionCommand = new MvxCommand(CreateElection);
            ovType = OV_Types.NL;
            _initDateString = DateTime.Today.ToString("dd/MM/yyyy");
            _finishDateString = DateTime.Today.ToString("dd/MM/yyyy");
        }


        //!Methods
        public void FilterUsers()
        {
            Users = new BindableCollection<AutorizedUserModel>(AllUsers.Where(u => u.DNI_NIE.ToUpper().Contains(_searchByDNI_NIE_Value.ToUpper())).ToList());
            if (AutonomousCommunity != null && Province != null)
            {
                Users = new BindableCollection<AutorizedUserModel>(Users.Where(u => u.Province.Id != Province.Id));
            }
            else if (AutonomousCommunity != null)
            {
                Users = new BindableCollection<AutorizedUserModel>(Users.Where(u => u.Province.AutonomousCommunity.Id != AutonomousCommunity.Id));
            }
            Users = new BindableCollection<AutorizedUserModel>(Users.Where(u => !SelectedUsers.Any(su => su.DNI_NIE == u.DNI_NIE)));
            RaisePropertyChanged(() => Users);
        }
        public async void CreateElection()
        {
            var errors = ValidateData();
            bool isAnyError = errors.Count > 0 || HasErrors;
            if (isAnyError)
            {
                VisualizeError(errors);
            }
            else
            {
                var electionObject = await GetElectionModel();
                var result = await _electionService.CreateAsync(new CandidateElection(electionObject), new CancellationToken());

                if (result is CreateElectionSuccess success)
                {
                    var newOrganizers = GetCandidateOrganizers(success.ResponseObject.Id.Value);
                    var response = await _organizerService.CreateRangeAsync(newOrganizers, new CancellationToken());

                    if (response)
                    {
                        foreach (var newOrganizer in newOrganizers)
                        {
                            var organizerResult = await _organizerService.FindAsync(OrganizerFilter.ByReferenceNumber(newOrganizer.ReferenceNumber).AndIncludeUser(), new CancellationToken());
                            var organizer = organizerResult.FirstOrDefault();
                            SendEmailToOrganizers(organizer.User.Email, success.ResponseObject, organizer.ReferenceNumber, organizer.User.FirstName);
                            resetAllData();
                        }
                        MessageBox.Show("La elección se ha creado corectamente", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                } else if (result is CreateElectionFailure failure)
                {
                    var errorText = "";
                    foreach (var error in failure.FailureReasons)
                    {
                        errorText += "- " + Translation.Translation.ResourceManager.GetString(error.PropertyName.ToString()) + "\r\n\r\n";
                    }
                    MessageBox.Show(errorText, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public void resetAllData()
        {
            Name = "";
            ClearError(nameof(Name));
            Description = "";
            AutonomousCommunity = null;
            Province = null;
            InitDate = DateTime.Today.AddDays(1);
            ClearError(nameof(InitDate));
            FinishDate = DateTime.Today;
            ClearError(nameof(InitDate));
            SearchByDNI_NIE_Value = "";
            foreach (var selectedUser in SelectedUsers.ToList())
            {
                Users.Add(selectedUser);
                SelectedUsers.Remove(selectedUser);
                if (SelectedUsers.Count < 4) AllowAddOrganizer = true;
            }

        }

        public void SendEmailToOrganizers(string userEmail, Election election, string referenceNumber, string userName)
        {
            //TODO: Create proper email text
            var email = Mailer.GenerateEmailMessage(userEmail, "Organizador de " + election.Name + " votación", "Hola " + userName + ". usted es organizador de "
                + election.Name + " votación, su numero de referencia es " + referenceNumber + " (se utiliza para entrar en la aplicación)");
            Mailer.SendEmail(email);
        }

        public List<CandidateOrganizer> GetCandidateOrganizers(int tblElection_UID)
        {
            List<CandidateOrganizer> candidates = new List<CandidateOrganizer>();
            foreach (var selectedUser in SelectedUsers)
            {
                var refNumber = GenerateReferenceNumber.Get();
                var candidateOrganizer = new CandidateOrganizer()
                {
                    tblUser_UID = selectedUser.Id,
                    tblElection_UID = tblElection_UID,
                    ReferenceNumber = refNumber
                };
                candidates.Add(candidateOrganizer);
            }

            return candidates;
        }

        public async Task<Election> GetElectionModel()
        {
            var types = await _typeService.FindAsync(TypeFilter.ByCode(ovType.ToString()), new CancellationToken());
            var tblType_UID = types.FirstOrDefault().Id;
            return new Election()
            {
                Name = Name,
                Description = Description,
                tblAutonomousCommunity_UID = ovType == OV_Types.NL ? null : AutonomousCommunity.Id,
                tblProvince_UID = ovType == OV_Types.NL || ovType == OV_Types.ACL ? null : Province.Id,
                InitDate = InitDate,
                FinalizeDate = FinishDate,
                tblType_UID = tblType_UID.Value
            };
        }

        private Dictionary<string, string> ValidateData()
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(Name?.Trim()))
            {
                errors.Add("Nombre", "Nombre de elección es obligatorio");
            }
            if ((ovType == OV_Types.ACL || ovType == OV_Types.PL) && AutonomousCommunity == null)
            {
                errors.Add("Comunidad autonoma", "Es necesario indicar comunidad autonoma");
            }
            if (ovType == OV_Types.PL && Province == null)
            {
                errors.Add("Provincia", "Es necesario indicar provincia");
            }
            if (InitDate > FinishDate)
            {
                errors.Add("Fecha de inicio", "No puede ser mayor que la fecha de fin de elección");
            }
            if (SelectedUsers.Count < 1)
            {
                errors.Add("Organizador", "Hay que selecionar al menos un organizador");
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
            if(errors.Count == 0 )
            {
                foreach (var error in _propertyError.OrderBy(_ => _.Key))
                {
                    var errorItemText = "";
                    foreach (var errorItem in error.Value)
                    {
                        errorItemText += "- " + errorItem + "\r\n\r\n";
                    }
                    errorText += "- " + errorItemText + "\r\n\r\n";
                }
            }
            MessageBox.Show(errorText, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void SelectUser()
        {
            if (_selectedUser != null)
            {
                SelectedUsers.Add(_selectedUser);
                Users.Remove(_selectedUser);
                if (SelectedUsers.Count >= 4) AllowAddOrganizer = false;
            }
        }
        public void RemoveUser()
        {
            if (_unselectedUser != null)
            {
                Users.Add(_unselectedUser);
                SelectedUsers.Remove(_unselectedUser);
                if (SelectedUsers.Count < 4) AllowAddOrganizer = true;
            }
        }

        public void removeFromSelectedUsers()
        {
            if (AutonomousCommunity != null && Province != null)
            {
                foreach (var selectedUser in SelectedUsers.ToList())
                {
                    if (selectedUser.Province.AutonomousCommunity.Id == AutonomousCommunity.Id && selectedUser.Province.Id == Province.Id)
                    {
                        Users.Add(selectedUser);
                        SelectedUsers.Remove(selectedUser);
                        if (SelectedUsers.Count < 4) AllowAddOrganizer = true;
                    }
                }
            }
            else if (AutonomousCommunity != null)
            {
                foreach (var selectedUser in SelectedUsers.ToList())
                {
                    if (selectedUser.Province.AutonomousCommunity.Id == AutonomousCommunity.Id)
                    {
                        Users.Add(selectedUser);
                        SelectedUsers.Remove(selectedUser);
                        if (SelectedUsers.Count < 4) AllowAddOrganizer = true;
                    }
                }
            }
        }

        private void ChangeOvTypeBoxState(OV_Types type)
        {
            ACEnable = type == OV_Types.ACL || type == OV_Types.PL;
            ProvinceEnable = type == OV_Types.PL;
            IsNotNacionalLevel = type != OV_Types.NL;
            if(!IsNotNacionalLevel)
            {
                Province = null;
                AutonomousCommunity = null;
            }
            if(type == OV_Types.NL)
                Province = null;
            FilterUsers();
        }

        public void ovacLevel()
        {
            ovType = OV_Types.ACL;
            ChangeOvTypeBoxState(ovType);
        }
        public void ovProvinceLevel()
        {
            ovType = OV_Types.PL;
            ChangeOvTypeBoxState(ovType);
        }
        public void ovNationalLevel()
        {
            ovType = OV_Types.NL;
            ChangeOvTypeBoxState(ovType);
        }

        public async Task LoadData()
        {
            var autonomousCommunities = await _autonomousCommunityService.FindAsync(AutonomousCommunityFilter.All.AndIncludeProvince(), new CancellationToken());
            AllAutonomousCommunities = new ObservableCollection<AutonomousCommunity>(autonomousCommunities);
            var result = await _userService.FindAsync(UserFilter.ByAutorized().AndByIncludeProvince().AndByIncludeAC(), new CancellationToken());
            result = result.Where(u => DocumentValidation.GetDocumentType(u.DNI_NIE) == "DNI");
            var resultList = result.ToList();
            List<AutorizedUserModel> users = new List<AutorizedUserModel>();
            foreach (var item in resultList)
            {
                var user = new AutorizedUserModel();
                user.SetData(item);
                users.Add(user);
            }
            resultList.ForEach(u => new AutorizedUserModel().SetData(u));
            AllUsers = new BindableCollection<AutorizedUserModel>(users);
            Users = AllUsers;
        }
        private void ClearError(string propertyName)
        {
            if (_propertyError.Remove(propertyName))
            {
                OnErrorChange(propertyName);
            }
        }
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
    }
}
