using OV.MainDb.Result.Create.Models.Public;
using OV.MainDb.User.Find.Models.Public;
using OV.Models.MainDb.Type;
using OV.NotifyService.Models;
using OV.NotifyService.Result;
using OV.NotifyService.Service.Election;
using OV.NotifyService.Service.User;
using OV.Services.Email;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OV.NotifyService.ViewModel
{
    public class NotifyElectionViewModel : INotifyPropertyChanged
    {
        //!Private variables
        private IElectionService _electionService;
        private IUserService _userService;
        private IResultService _resultService;
        private NotifyElectionModel _selectedElection;
        private ObservableCollection<NotifyElectionModel> _elections;

        //!Public variables
        public event PropertyChangedEventHandler PropertyChanged;


        //!Properties
        public NotifyElectionModel SelectedElection
        {
            get { return _selectedElection; }
            set
            {
                _selectedElection = value;
                OnPropertyChanged("SelectedElection");
            }
        }
        public ObservableCollection<NotifyElectionModel> Elections
        {
            get
            {
                return _elections;
            }
            set
            {
                _elections = value;
                OnPropertyChanged("Elections");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null && !string.IsNullOrEmpty(propertyName))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public NotifyElectionViewModel()
        {
            _electionService = new ElectionService();
            _userService = new UserService();
            _resultService = new ResultService();

            new Thread(async () =>
            {
                await LoadData();
            }).Start();

            new Thread(async () =>
            {
                await NotifyUser();
                Thread.Sleep(20000);
            }).Start();
        }

        public async Task LoadData()
        {
            var notifiedElections = await _electionService.GetNotifiedAsync(new CancellationToken());
            List<NotifyElectionModel> notifyElectionModels = new List<NotifyElectionModel>();
            foreach (var election in notifiedElections)
            {
                var notifyElectionModel = new NotifyElectionModel();
                notifyElectionModel.SetDate(election);
            }

            Elections = new ObservableCollection<NotifyElectionModel>(notifyElectionModels);
        }

        public async Task NotifyUser()
        {
            var unnotifiedElections = await _electionService.GetUnnotifiedAsync(new CancellationToken());
            foreach (var election in unnotifiedElections)
            {
                await SendEmailToEachUserRelatedToElection(election);
            }
        }

        private async Task SendEmailToEachUserRelatedToElection(OV.Models.MainDb.Election.Election election)
        {
            IEnumerable<OV.Models.MainDb.User.User> users = null;
            if(election.Type.Code == OV_Types.NL.ToString())
            {
                users = await _userService.FindAsync(UserFilter.All, new CancellationToken());
            }
            if(election.Type.Code == OV_Types.ACL.ToString())
            {
                users = await _userService.FindAsync(UserFilter.ByAc(election.tblAutonomousCommunity_UID.Value), new CancellationToken());
            }
            if(election.Type.Code == OV_Types.PL.ToString())
            {
                users = await _userService.FindAsync(UserFilter.ByProvince(election.tblProvince_UID.Value), new CancellationToken());
            }
            await SendEmail(users, election);
        }

        public async Task SendEmail(IEnumerable<OV.Models.MainDb.User.User> users, OV.Models.MainDb.Election.Election election) 
        {
            if(users.ToList().Count > 0)
            {
                var result = await _resultService.GetResult(election.Id.Value, new CancellationToken());

                if (result.Error) return;

                var winnerResult = result.Options;
                if(winnerResult != null)
                {
                    var totalVotes = 0;
                    foreach (var item in winnerResult)
                    {
                        totalVotes += item.Votes != null ? item.Votes.Value : 0;
                    }
                    var winner = winnerResult.OrderByDescending(o => o.Votes).FirstOrDefault();

                    if (winner == null) return;

                    var text = election.Id + " " + election.Name + " " + election.Type.Name + " "
                        + result.TotalHabitant + " " + result.HabitantCountThatParticipate
                        + " Winner " + winner.Id + " " + winner.Name + " " + winner.Votes;
                    foreach (var user in users)
                    {
                        var message = Mailer.GenerateEmailMessage(user.Email, "Resultados de elección " + election.Name, text);
                        Mailer.SendEmail(message);
                    }

                    var cancellationToken = new CancellationToken();
                    CreateResultRequest request = new CreateResultRequest()
                    {
                        TblElection_UID = election.Id.Value,
                        TotalHabitants = result.TotalHabitant,
                        TotalHabitantsThatParticipate = result.HabitantCountThatParticipate,
                        Winner_UID = winner.Id.Value,
                        TotalVotes = totalVotes
                    };
                    var createResult = await _resultService.CreateAsync(request, cancellationToken);

                    if(createResult)
                    {
                        await _electionService.Notify(election.Id.Value, cancellationToken);
                    }
                }
            }
        }
    }
}
