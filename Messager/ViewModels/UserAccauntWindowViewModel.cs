using Messager.Helpers;
using Messager.Models.Entitys;
using Messager.Models.Requests;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Messager.ViewModels
{
    public class UserAccauntWindowViewModel : BindableBase
    {
        private string _name;
        private User _currentUser;
        private User _addingUser;
        private BitmapImage _image;

        public UserAccauntWindowViewModel(params User[] user)
        {
            _currentUser = user[0];
            Name = user[1].Name;
            _addingUser = user[1];
        }

        public BitmapImage Image
        {
            get => _image;
            set
            {
                _image = value;
                RaisePropertyChanged();
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        private DelegateCommand _addInFriendsButtonCommand;
        public DelegateCommand AddInFriendsButtonCommand => _addInFriendsButtonCommand ??= new DelegateCommand(AddInFriendsButtonCommand_Execute, AddInFriendsButtonCommand_CanExecute);
        private DelegateCommand _deleteFromFriendsButtonCommand;
        public DelegateCommand DeleteFromFriendsButtonCommand => _deleteFromFriendsButtonCommand ??= new DelegateCommand(DeleteFromFriendsButtonCommand_Execute);

        private async void DeleteFromFriendsButtonCommand_Execute()
        {
            using(var deleteFromFriendsRequest = await RequestsFactory.CreateRequestAsync<DeleteFromFriendsRequest, User>(_currentUser, _addingUser))
            {
                var response = await deleteFromFriendsRequest.SendRequestAsync();
                if(response.ResponseCode != 200)
                {
                    MessageBox.Show(response.ToString());
                }
            }
        }

        private async void AddInFriendsButtonCommand_Execute()
        {
            using (var addInFriendsRequest = await RequestsFactory.CreateRequestAsync<AddInFriendsRequest, User>(_currentUser, _addingUser))
            {
                var response = await addInFriendsRequest.SendRequestAsync();
                if(response.ResponseCode != 200)
                {
                    MessageBox.Show(response.ToString());
                }
            }
        }
        private bool AddInFriendsButtonCommand_CanExecute()
        {
            return true;
        }
    }
}