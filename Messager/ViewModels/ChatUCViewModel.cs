using Messager.Models.Entitys;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager.ViewModels
{
    public class ChatUCViewModel : BindableBase
    {
        private List<Message> _messages;
        private string _writedText;

        public List<Message> Messages 
        {
            get => _messages;
            set
            {
                _messages = value;
                RaisePropertyChanged();
            }
        }
        public string WritedText
        {
            get => _writedText;
            set
            {
                _writedText = value;
                RaisePropertyChanged();
            }
        }
    }
}
