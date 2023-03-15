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
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Messager.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private IEnumerable<User> _chats;
        private User _currentUser;
        private IEnumerable<User> _searchedUsers;
        private string _searchedString;
        private User _selectedUser;
        private User _selectedChat;
        private string _name;
        private BitmapImage _image;

        public MainWindowViewModel(User user)
        {
            _currentUser = user;
            SearchedStringChanged += SearchedStringChangedHendler;
        }

        private async void SearchedStringChangedHendler()
        {
            await SearchUsersAsync();
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
        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                ShowUserAccauntWindow();
                RaisePropertyChanged();
            }
        }
        public string SearchedString
        {
            get => _searchedString;
            set
            {
                _searchedString = value;
                SearchedStringChanged?.Invoke();
                RaisePropertyChanged();
            }
        }
        public IEnumerable<User> SearchedUsers
        {
            get => _searchedUsers;
            set
            {
                _searchedUsers = value;
                RaisePropertyChanged();
            }
        }
        public IEnumerable<User> Chats
        {
            get => _chats;
            set
            {
                _chats = value;
                RaisePropertyChanged();
            }
        }
        public User SelectedChat 
        { 
            get => _selectedChat;
            set
            {
                _selectedChat = value;
                RaisePropertyChanged();
            }
        }

        public event Action SearchedStringChanged;

        public async Task UpdateChatsAsync()
        {
            using(var request = (GetChatsRequest)await RequestsFactory.CreateRequestAsync<GetChatsRequest, User>(_currentUser))
            {
                try
                {
                    Response response = await request.SendRequestAsync();
                    if (response.ResponseCode == 200)
                    {
                        Chats = JsonSerializer.Deserialize<List<User>>(response.ResponseObj);
                    }
                    if (response.ResponseCode == 406)
                    {
                        Chats = new List<User>().Append(new User() { Name = response.ErrorMessage });
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private async Task SearchUsersAsync()
        {
            var searchedString = new SearchedString()
            {
                SearchedUserString = SearchedString
            };
            using (var request = await RequestsFactory.CreateRequestAsync<SearchRequest, SearchedString>(searchedString))
            {
                Response response = await request.SendRequestAsync();
                if(response.ResponseCode == 200)
                {
                    SearchedUsers = JsonSerializer.Deserialize<List<User>>(response.ResponseObj);
                }
            }
        }
        private void ShowUserAccauntWindow()
        {
            var userAcccauntWindow = new UserAccauntWindow(_currentUser, SelectedUser);
            userAcccauntWindow.Show();
        }
    }
}
