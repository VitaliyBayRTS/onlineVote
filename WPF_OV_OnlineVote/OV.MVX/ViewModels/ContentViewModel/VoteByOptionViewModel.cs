using Caliburn.Micro;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using OV.MainDb.Option.Find.Models.Public;
using OV.MainDb.Option.IncreaseVotes.Models.Public;
using OV.MVX.Models.Organizer;
using OV.MVX.Services.Option;
using OV.MVX.Services.UserElection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class VoteByOptionViewModel : MvxViewModel
    {
        //!Cpmmands
        public IMvxCommand VoteCommand { get; set; }

        //!Private variables
        private IOptionService _optionService;
        private IUserElectionService _userElectionService;

        private BindableCollection<OptionModel> _options;
        private OptionModel _selectedOption;
        private static string _alreadyMade = "Usted ya ha votado";
        private static string _canVote = "Votar por esta opción";
        private string _btnText = _canVote;
        private bool _isBtnEnable = true;


        //!Properties
        public int User_UID { get; set; }
        public int Election_UID { get; set; }


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
        public string BtnText 
        {
            get 
            {
                return _btnText;
            }
            set 
            {
                SetProperty(ref _btnText, value);
                RaisePropertyChanged(() => BtnText);
            }
        }
        public bool IsBtnEnable 
        {
            get 
            {
                return _isBtnEnable;
            }
            set 
            {
                SetProperty(ref _isBtnEnable, value);
                RaisePropertyChanged(() => IsBtnEnable);
            }
        }

        public VoteByOptionViewModel()
        {
            _optionService = new OptionService();
            _userElectionService = new UserElectionService();

            VoteCommand = new MvxCommand(MakeVote);
        }

        private async void MakeVote()
        {
            if (SelectedOption == null)
            {
                MessageBox.Show("Hay que selecionar opción", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var reponse = MessageBox.Show("¿Estas seguro que quieres VOTAR por la opción " + SelectedOption.Index + " ?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (reponse == MessageBoxResult.Yes)
            {
                IncreaseVotesRequest request = new IncreaseVotesRequest()
                {
                    TblElection_UID = Election_UID,
                    TblUser_UID = User_UID,
                    TblOption_UID = SelectedOption.Id.Value
                };
                var result = await _optionService.IncreaseVote(request, new CancellationToken());
                if (result is IncreaseVotesSuccess success)
                {
                    MessageBox.Show("La operación se ha realizado correctamente", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadData(User_UID, Election_UID);
                }
                else
                {
                    MessageBox.Show("Se ha ocurido error, contacta con el grupo de antención de cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public async void LoadData(int election_UID, int userId)
        {
            IsBtnEnable = true;
            BtnText = _canVote;
            User_UID = userId;
            Election_UID = election_UID;
            var result = await _userElectionService.FindAsync(userId, election_UID, new CancellationToken());
            var resultList = result.ToList();
            if(resultList.Count != 0)
            {
                IsBtnEnable = false;
                BtnText = _alreadyMade;
            }
            ReloadOptions();
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
    }
}
