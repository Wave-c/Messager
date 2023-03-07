using Messager.Models;
using Messager.Models.Entitys;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messager.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private IEnumerable<User> _chats;
        private User _currentUser;

        public MainWindowViewModel(User user)
        {
            _currentUser = user;
            
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
    }
}
