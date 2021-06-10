using Caliburn.Micro;
using OV.MainDb.User.Find.Models.Public;
using OV.MVX.Services.User;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using OV.MVX.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Windows;
using OV.Services.Email;
using OV.Services.Email.EmailTemplates;

namespace OV.MVX.ViewModels.ContentViewModel
{
    public class UnautorizedUsersViewModel : MvxViewModel, IMvxNotifyPropertyChanged
    {
        //!Commands
        public IMvxCommand ApproveUserDataCommand { get; set; }
        public IMvxCommand DeleteUserDataCommand { get; set; }

        //!Private variables
        private UserService userService;
        private UnautorizedUserModel _selectedUnautorizedUser;
        private BindableCollection<UnautorizedUserModel> _users;

        //!Properties
        public UnautorizedUserModel SelectedUnautorizedUser
        {
            get { return _selectedUnautorizedUser; }
            set
            {
                SetProperty(ref _selectedUnautorizedUser, value);
                RaisePropertyChanged(() => SelectedUnautorizedUser);
            }
        }
        public BindableCollection<UnautorizedUserModel> Users { 
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


        public UnautorizedUsersViewModel()
        {
            userService = new UserService();
            ApproveUserDataCommand = new MvxCommand(ApproveData);
            DeleteUserDataCommand = new MvxCommand(DeleteUser);
        }


        //!Methods
        public async void ApproveData()
        {

            var result = MessageBox.Show("¿Estas seguro que quieres aprobar los datos de este usuario?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                var response = await userService.AutorizeAsync(SelectedUnautorizedUser.Id, new CancellationToken());
                if(response)
                {
                    var emailTemplate = GenerateTemplate.GenerateEmailTemplate("Usuario autorizado", "Hola " + SelectedUnautorizedUser.FirstName + ". Su usuario ha sido autorizado");
                    var email = Mailer.GenerateEmailMessage(SelectedUnautorizedUser.Email, "Usuario autorizado", emailTemplate);
                    Mailer.SendEmail(email);
                    await LoadData();
                }
            }
        }
        public async void DeleteUser()
        {

            var result = MessageBox.Show("¿Estas seguro que quieres BORRAR los datos de este usuario?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                var response = await userService.DeleteAsync(SelectedUnautorizedUser.Id, new CancellationToken());
                if(response)
                {
                    var emailTemplate = GenerateTemplate.GenerateEmailTemplate("Usuario NO autorizado", "Hola " + SelectedUnautorizedUser.FirstName + ". Su usuario NO ha sido autorizado");
                    var email = Mailer.GenerateEmailMessage(SelectedUnautorizedUser.Email, "Usuario NO autorizado", emailTemplate);
                    Mailer.SendEmail(email);
                    await LoadData();
                }
            }
        }
        public async Task LoadData()
        {
            var result = await userService.FindAsync(UserFilter.ByUnautorized(), new CancellationToken());
            var resultList = result.ToList();
            List<UnautorizedUserModel> users = new List<UnautorizedUserModel>();
            foreach (var item in resultList)
            {
                var user = new UnautorizedUserModel();
                user.SetData(item);
                users.Add(user);
            }
            resultList.ForEach(u => new UnautorizedUserModel().SetData(u));
            Users = new BindableCollection<UnautorizedUserModel>(users);
        }
    }
}
