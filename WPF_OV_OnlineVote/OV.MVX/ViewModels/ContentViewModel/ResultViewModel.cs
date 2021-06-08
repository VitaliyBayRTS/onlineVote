using Caliburn.Micro;
using MvvmCross.ViewModels;
using OV.MVX.Models.Organizer;
using OV.MVX.Services.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class ResultViewModel : MvxViewModel
    {
        //!Private variables
        private IResultService _resultService;
        private BindableCollection<OptionModel> _options;
        private OptionModel _selectedOption;
        private BindableCollection<OptionModel> _winner;
        private OptionModel _selectedWinner;

        //!Properties
        public int TotalHabitantCount { get; set; }
        public int TotalHabitantCountThatParticipate { get; set; }
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
        public BindableCollection<OptionModel> Winner
        {
            get
            {
                return _winner;
            }
            set
            {
                SetProperty(ref _winner, value);
                RaisePropertyChanged(() => Winner);
            }
        }
        public OptionModel SelectedWinner
        {
            get
            {
                return _selectedWinner;
            }
            set
            {
                SetProperty(ref _selectedWinner, value);
                RaisePropertyChanged(() => SelectedWinner);
            }
        }
        public int Election_UID { get; set; }

        public ResultViewModel()
        {
            _resultService = new ResultService();
        }

        public async void LoadData(int election_UID)
        {
            Election_UID = election_UID;
            var result = await _resultService.GetResult(election_UID, new CancellationToken());
            if (result.Error) 
            {
                MessageBox.Show("Se ha ocurido error, contacta con el grupo de antención de cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            TotalHabitantCount = result.TotalHabitant;
            await RaisePropertyChanged(() => TotalHabitantCount);
            TotalHabitantCountThatParticipate = result.HabitantCountThatParticipate;
            await RaisePropertyChanged(() => TotalHabitantCountThatParticipate);
            ReloadOptions(result.Options);
        }
        private void ReloadOptions(IEnumerable<OV.Models.MainDb.Option.Option> options)
        {
            List<OptionModel> optionModels = new List<OptionModel>();
            var indexCount = 0;
            foreach (var option in options)
            {
                var optionModel = new OptionModel();
                indexCount++;
                optionModel.SetData(option, indexCount);
                optionModels.Add(optionModel);
            }
            Options = new BindableCollection<OptionModel>(optionModels);
            GetWinner();
        }

        public void GetWinner()
        {
            OptionModel winner = Options.OrderByDescending(x => x.Votes).FirstOrDefault();
            List<OptionModel> TestList = new List<OptionModel>();
            if(winner != null && winner.Votes > 0) 
                TestList.Add(winner);
            Winner = new BindableCollection<OptionModel>(TestList);
        }
    }
}
