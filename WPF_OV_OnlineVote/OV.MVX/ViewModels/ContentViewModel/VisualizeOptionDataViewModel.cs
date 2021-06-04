using Caliburn.Micro;
using MvvmCross.ViewModels;
using OV.MainDb.Option.Find.Models.Public;
using OV.MVX.Models.Organizer;
using OV.MVX.Services.Option;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class VisualizeOptionDataViewModel : MvxViewModel
    {
        //!Private variables
        private IOptionService _optionService;

        private BindableCollection<OptionModel> _options;
        private OptionModel _selectedOption;


        //!Properties
        public int Organizer_UID { get; set; }
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




        public VisualizeOptionDataViewModel()
        {
            _optionService = new OptionService();
        }


        public void LoadData(int election_UID)
        {
            Election_UID = election_UID;
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
