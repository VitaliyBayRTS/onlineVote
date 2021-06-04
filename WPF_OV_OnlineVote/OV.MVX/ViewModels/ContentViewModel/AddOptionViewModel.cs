using Caliburn.Micro;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.Election.Find.Models.Public;
using OV.MainDb.Option.Create.Models.Public;
using OV.MainDb.Option.Find.Models.Public;
using OV.MainDb.Option.Models.Public;
using OV.MainDb.Option.Modify.Models.Public;
using OV.MVX.Models;
using OV.MVX.Models.Organizer;
using OV.MVX.Services.Election;
using OV.MVX.Services.Option;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class AddOptionViewModel : MvxViewModel, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        //!Cpmmands
        public IMvxCommand CreateOptionDataCommand { get; set; }
        public IMvxCommand DeleteOptionCommand { get; set; }
        public IMvxCommand CancelEditDataCommand { get; set; }
        public IMvxCommand ModifyOptionDataCommand { get; set; }


        //!Private variables
        private IOptionService _optionService;
        private IElectionService _electionService;
        private string _name;
        private string _selectedName;
        private string _description;
        private string _selecteddescription;
        private BindableCollection<OptionModel> _options;
        private OptionModel _selectedOption;
        private bool _isOptionSelected;
        private readonly Dictionary<string, List<string>> _propertyError = new Dictionary<string, List<string>>();
        private bool _isInPendingState;
        public bool HasErrors => _propertyError.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


        //!Properties
        public int Organizer_UID { get; set; }
        public int Election_UID { get; set; }
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
                IsOptionSelected = true && IsInPendingState;
                ResetSelectedNameAndDesc();
                RaisePropertyChanged(() => SelectedOption);
            }
        }
        public bool IsOptionSelected { 
            get 
            {
                return _isOptionSelected;
            }
            set 
            {
                SetProperty(ref _isOptionSelected, value);
                RaisePropertyChanged(() => IsOptionSelected);
            }
        }
        public string SelectedName 
        {
            get 
            {
                return _selectedName;
            }
            set 
            {
                SetProperty(ref _selectedName, value);
                ClearError(nameof(SelectedName));
                if (string.IsNullOrEmpty(_selectedName))
                {
                    AddError(nameof(SelectedName), "Nombre no puede ser vacio");
                }
                else if (!System.Text.RegularExpressions.Regex.IsMatch(_selectedName, @"^[a-zA-ZñÑ áÁ óÓ éÉ íÍ úÚ]+$"))
                {
                    AddError(nameof(SelectedName), "Solo caracteres alfabeticos");
                }
                RaisePropertyChanged(() => SelectedName);
            }
        }
        public string SelectedDescription 
        {
            get 
            {
                return _selecteddescription;
            }
            set 
            {
                SetProperty(ref _selecteddescription, value);
                ClearError(nameof(SelectedDescription));
                if (string.IsNullOrEmpty(_selecteddescription))
                {
                    AddError(nameof(SelectedDescription), "Descripción no puede ser vacio");
                }
                RaisePropertyChanged(() => SelectedDescription);
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




        public AddOptionViewModel(int organizer_UID, int election_UID)
        {
            Organizer_UID = organizer_UID;
            Election_UID = election_UID;
            _optionService = new OptionService();
            _electionService = new ElectionService();
            CreateOptionDataCommand = new MvxCommand(CreateOption);
            CancelEditDataCommand = new MvxCommand(CancelEditElection);
            DeleteOptionCommand = new MvxCommand(DeleteOption);
            ModifyOptionDataCommand = new MvxCommand(EditElection);
        }

        public async void CreateOption()
        {
            var errors = ValidateData();
            bool isAnyError = errors.Count > 0 || HasErrors;
            if (isAnyError)
            {
                VisualizeError(errors);
            }
            else
            {
                await CreateOptionData();
            }
        }

        private async Task CreateOptionData()
        {
            CandidateOption candidate = new CandidateOption()
            {
                Name = Name,
                Description = Description,
                tblElection_UID = Election_UID
            }; 
            var result = await _optionService.CreateAsync(candidate, new CancellationToken());

            if (result is CreateOptionSuccess success)
            {
                MessageBox.Show("La opción se ha creado corectamente", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                ResetData();
                ReloadOptions();
            }
            else if (result is CreateOptionFailure failure)
            {
                var errorText = "";
                foreach (var error in failure.FailureReasons)
                {
                    errorText += "- " + Translation.Translation.ResourceManager.GetString(error.Code.ToString()) + "\r\n\r\n";
                }
                MessageBox.Show(errorText, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public async void DeleteOption()
        {
            var result = MessageBox.Show("¿Estas seguro que quieres BORRAR la opción " + SelectedOption.Index + " ?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var response = await _optionService.DeleteAsync(SelectedOption.Id.Value, new CancellationToken());
                if (response)
                {
                    ReloadOptions();
                } 
                else
                {
                    MessageBox.Show("Error, no se ha podido borrar", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        public async void CancelEditElection()
        {
            ResetSelectedNameAndDesc();
        }

        public async void EditElection()
        {
            var errors = ValidateOptionData();
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
            var response = MessageBox.Show("¿Estas seguro que quieres MODIFICAR los datos de esta opción?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (response == MessageBoxResult.Yes)
            {
                await ModifyOptionData();
            }
        }

        public async Task ModifyOptionData()
        {
            ModifyOptionCandidate candidate = new ModifyOptionCandidate()
            {
                Id = SelectedOption.Id.Value,
                Name = SelectedName,
                Description = SelectedDescription
            }; 
            var result = await _optionService.ModifyAsync(candidate, new CancellationToken());

            if (result is ModifyOptionSuccess success)
            {
                MessageBox.Show("La opción se ha modificado corectamente", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                ReloadOptions();
                ResetSelectedNameAndDesc();
            }
            else if (result is ModifyOptionFailure failure)
            {
                var errorText = "";
                foreach (var error in failure.FailureReasons)
                {
                    errorText += "- " + Translation.Translation.ResourceManager.GetString(error.PropertyName.ToString()) + "\r\n\r\n";
                }
                MessageBox.Show(errorText, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        public async Task LoadData()
        {
            await ReloadElectionDate(Election_UID);
            ReloadOptions();
        }
        private void ResetSelectedNameAndDesc()
        {
            SelectedName = _selectedOption?.Name;
            ClearError(nameof(SelectedName));
            SelectedDescription = _selectedOption?.Description;
            ClearError(nameof(SelectedDescription));
        }


        private async void ReloadOptions()
        {
            var options = await _optionService.FindAsync(OptionFilter.ByElectionId(Election_UID), new CancellationToken());
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


        private async Task<bool> ReloadElectionDate(int tblElection_UID)
        {
            var result = await _electionService.FindAsync(ElectionFilter.ById(tblElection_UID).AndTypeIncluded().AndACIncluded().AndProvinceIncluded(),
                  new CancellationToken());
            var singleElection = result.First();
            var electionModel = new ElectionModel();
            electionModel.SetData(singleElection);
            IsInPendingState = electionModel.CurrentState == State.Pendiente.ToString();
            return true;
        }


        private void ResetData()
        {
            Name = "";
            ClearError(nameof(Name));
            Description = "";
            ClearError(nameof(Description));
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

            return errors;
        }

        private Dictionary<string, string> ValidateOptionData()
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (HasErrors) return errors;

            if (string.IsNullOrEmpty(SelectedName?.Trim()))
            {
                errors.Add("Nombre", "Nombre de elección es obligatorio");
            }
            if (string.IsNullOrEmpty(SelectedDescription?.Trim()))
            {
                errors.Add("Descripción", "Descripción de elección es obligatorio");
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
