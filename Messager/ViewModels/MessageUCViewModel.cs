using Messager.Models.Entitys;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager.ViewModels
{
    public class MessageUCViewModel : BindableBase
    {
        private string _text;
        private string _name;

        public MessageUCViewModel(Message message)
        {
            Text = message.Information;
            Name = message.FromName;
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
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                RaisePropertyChanged();
            }
        }
    }
}
