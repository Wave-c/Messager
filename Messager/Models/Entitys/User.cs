using Messager.ViewModels;
using Messager.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Messager.Models.Entitys
{
    public class User : Entity
    {
        public User()
        {
            ChatUC ??= new ChatUC();
        }
        public string Name { get; set; }
        public string Password { get; set; }
        public Status Status { get; set; }
        public byte[] Image { get; set; }
        public string Email { get; set; }

        [NotMapped, JsonIgnore]
        public ChatUC ChatUC { get; set; }
    }
    public enum Status
    {
        OFFLINE, ONLINE, WRITING
    }
}
