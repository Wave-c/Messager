using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager.Models.Entitys
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Status Status { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
    }
    public enum Status
    {
        OFFLINE, ONLINE, WRITING
    }
}
