using Messager.Helpers;
using Messager.Models;
using Messager.Models.Entitys;
using Messager.Models.Requests;
using Messager.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Messager.ViewModels
{
    class LoginWindowViewModel : BindableBase
    {
        private string _login;
        private string _password;
        private LoginWindow _window;

        public LoginWindowViewModel(LoginWindow window) 
        { 
            _window = window;
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                RaisePropertyChanged();
                LoginCommand.RaiseCanExecuteChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged();
                LoginCommand.RaiseCanExecuteChanged();
                RegisterCommand.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _loginCommand;
        public DelegateCommand LoginCommand => _loginCommand ??= new DelegateCommand(LoginCommand_Execute, LoginAndRegisterCommand_CanExecute);

        private DelegateCommand _registerCommand;
        public DelegateCommand RegisterCommand => _registerCommand ??= new DelegateCommand(RegisterCommand_Execute, LoginAndRegisterCommand_CanExecute);

        private async void RegisterCommand_Execute()
        {
            string encodedPassword = Encryptor.GenerateHash(Password);
            var addedUser = new User()
            {
                Id = Guid.NewGuid(),
                Name = Login,
                Password = encodedPassword
            };

            using (var request = (RegisterRequest)await RequestsFactory.CreateRequestAsync<RegisterRequest, User>(addedUser))
            {
                Response response = await request.SendRequest();

                if (response.ResponseCode == 200)
                {
                    LoginCommand_Execute();
                }
                else
                {
                    MessageBox.Show(response.ToString());
                }
            }
        }
       
        private async void LoginCommand_Execute()
        {
            string encodedPassword = Encryptor.GenerateHash(Password);
            var loginUser = new User()
            {
                Id = Guid.Empty,
                Name = Login,
                Password = encodedPassword
            };

            using (var request = (LoginRequest)await RequestsFactory.CreateRequestAsync<LoginRequest, User>(loginUser))
            {
                Response response = await request.SendRequest();

                if(response.ResponseCode == 200)
                {
                    EnterToSystem(loginUser);
                }
                else
                {
                    MessageBox.Show(response.ToString());
                }
            }
        }
        private bool LoginAndRegisterCommand_CanExecute()
        {
            return !string.IsNullOrEmpty(Login) && !string.IsNullOrEmpty(Password);
        }

        private void EnterToSystem(User user)
        {
            var mainWindow = new MainWindow(user);
            mainWindow.Show();
            _window.Close();
        }
    }
}
