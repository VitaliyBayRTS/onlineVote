using Caliburn.Micro;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.Election.Find.Models.Public;
using OV.MainDb.Election.Modify.Models.Public;
using OV.MainDb.Option.Find.Models.Public;
using OV.MainDb.Organizer.Find.Models.Public;
using OV.MVX.Models;
using OV.MVX.Models.Organizer;
using OV.MVX.Services.Election;
using OV.MVX.Services.Option;
using OV.MVX.Services.Organizer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class EditElectionViewModel : MvxViewModel, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        //!Cpmmands
        public IMvxCommand EditElectionDataCommand { get; set; }
        public IMvxCommand CancelEditDataCommand { get; set; }
        public IMvxCommand DeleteOptionCommand { get; set; }

        //!Private variables
        private IElectionService _electionService;
        private IOrganizerService _organizerService;
        private IOptionService _optionService;
        private ElectionModel _election;
        private BindableCollection<OptionModel> _options;
        private OptionModel _selectedOption;
        private bool _isInPendingState;
        private string _name;
        private string _description;
        private DateTime _initDate = DateTime.Today.AddDays(1);
        private DateTime _finishDate = DateTime.Today.AddDays(1);
        private string _initDateString = DateTime.Today.ToString("dd/MM/yyyy");
        private string _finishDateString = DateTime.Today.ToString("dd/MM/yyyy");
        private readonly Dictionary<string, List<string>> _propertyError = new Dictionary<string, List<string>>();

        public bool HasErrors => _propertyError.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        //!Properties
        public ElectionModel Election
        {
            get { return _election; }
            set
            {
                SetProperty(ref _election, value);
                RaisePropertyChanged(() => Election);
            }
        }
        public BindableCollection<OptionModel> Options
        {
            get
            {
                return _options;
            }
            set
            {
                SetProperty(ref _options, value);
                RaisePropertyChanged(() => Options);
            }
        }
        public OptionModel SelectedOption
        {
            get
            {
                return _selectedOption;
            }
            set
            {
                SetProperty(ref _selectedOption, value);
                RaisePropertyChanged(() => SelectedOption);
            }
        }
        public int TblOrganizer_UID { get; set; }
        public int TblElection_UID { get; set; }
        public DateTime StartDateTime { get; set; } = DateTime.Today.AddDays(1);
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
                    AddError(nameof(Name), "Solo caracteres alfabeticos");
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
                ClearError(nameof(Description));
                if (string.IsNullOrEmpty(_description))
                {
                    AddError(nameof(Description), "Descripción no puede ser vacio");
                }
                RaisePropertyChanged(() => Description);
            }
        }
        public DateTime InitDate
        {
            get { return _initDate; }
            set
            {
                SetProperty(ref _initDate, value);
                InitDateString = value.ToString("dd/MM/yyyy");
                if (_initDate > FinishDate)
                {
                    FinishDate = _initDate;
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
        public bool IsInPendingState 
        {
            get 
            {
                return _isInPendingState;
            }
            set 
            {
                SetProperty(ref _isInPendingState, value);
                RaisePropertyChanged(() => IsInPendingState);
            }
        }


        public EditElectionViewModel(int tblOrganizer_UID, int tblElection_UID)
        {
            TblOrganizer_UID = tblOrganizer_UID;
            TblElection_UID = tblElection_UID;
            _electionService = new ElectionService();
            _organizerService = new OrganizerService();
            _optionService = new OptionService();
            EditElectionDataCommand = new MvxCommand(EditElection);
            CancelEditDataCommand = new MvxCommand(CancelEditElection);
            DeleteOptionCommand = new MvxCommand(DeleteOption);
        }

        //!Methods
        public async void EditElection()
        {
            var errors = ValidateData();
            bool isAnyError = errors.Count > 0 || HasErrors;
            if (isAnyError)
            {
                VisualizeError(errors);
            }
            else
            {
                await ModifyElectionData();
            }
        }

        private async Task ModifyElectionData()
        {
            var response = MessageBox.Show("¿Estas seguro que quieres MODIFICAR los datos de esta votación?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (response == MessageBoxResult.Yes)
            {
                ModifyElectionCandidate candidate = new ModifyElectionCandidate()
                {
                    Id = Election.Id.Value,
                    Name = Name,
                    Description = Description,
                    InitDateTime = InitDate,
                    FinalizeDateTime = FinishDate,
                };
                var result = await _electionService.ModifyAsync(candidate, new CancellationToken());

                if (result is ModifyElectionSuccess success)
                {
                    MessageBox.Show("La elección se ha modificado corectamente", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    await ReloadElectionDate(Election.Id.Value);
                    UpdatePropertiesValue();
                }
                else if (result is ModifyElectionFailure failure)
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

        public async void CancelEditElection()
        {
            UpdatePropertiesValue();
        }

        public async void DeleteOption()
        {
            if(SelectedOption != null)
            {
                var result = MessageBox.Show("¿Estas seguro que quieres BORRAR la opción " + SelectedOption.Index + " ?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    var response = await _optionService.DeleteAsync(SelectedOption.Id.Value, new CancellationToken());
                    if (response)
                    {
                        ReloadOptions();
                    }
                }
            }
        }

        public async Task LoadData()
        {
            //var organizer = await _organizerService.FindAsync(OrganizerFilter.ById(TblOrganizer_UID), new CancellationToken());
            //if (organizer == null || organizer.ToList().Count == 0)
            //    throw new ArgumentNullException("Organizer does not exist");

            //var singleOrganizer = organizer.First();

            await ReloadElectionDate(TblElection_UID);

            ReloadOptions();

            UpdatePropertiesValue();
        }

        private void UpdatePropertiesValue()
        {
            Name = Election.Name;
            Description = Election.Description;
            InitDate = Election.InitDate;
            InitDateString = Election.InitDateString;
            FinishDate = Election.FinilizeDate;
            FinishDateString = Election.FinalizeDateString;
        }

        private async Task<bool> ReloadElectionDate(int tblElection_UID)
        {
            var result = await _electionService.FindAsync(ElectionFilter.ById(tblElection_UID).AndTypeIncluded().AndACIncluded().AndProvinceIncluded(),
                  new CancellationToken());
            var singleElection = result.First();
            var electionModel = new ElectionModel();
            electionModel.SetData(singleElection);
            Election = electionModel;
            IsInPendingState = Election.CurrentState == State.Pendiente.ToString();
            return true;
        }

        private async void ReloadOptions()
        {
            var options = await _optionService.FindAsync(OptionFilter.ByElectionId(Election.Id.Value), new CancellationToken());
            var optionsList = options.ToList();
            List<OptionModel> optionModels = new List<OptionModel>();
            var indexCount = 0;
            foreach (var option in optionsList)
            {
                var optionModel = new OptionModel();
                indexCount++;
                optionModel.SetData(option, indexCount);
                optionModels.Add(optionModel);
            }
            Options = new BindableCollection<OptionModel>(optionModels);
        }


        //!Error handler

        private Dictionary<string, string> ValidateData()
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (HasErrors) return errors;

            if (string.IsNullOrEmpty(Name?.Trim()))
            {
                errors.Add("Nombre", "Nombre de elección es obligatorio");
            }
            if (string.IsNullOrEmpty(Description?.Trim()))
            {
                errors.Add("Descripción", "Descripción de elección es obligatorio");
            }
            if (InitDate > FinishDate)
            {
                errors.Add("Fecha de inicio", "No puede ser mayor que la fecha de fin de elección");
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

        public void ClearError(string propertyName)
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
