using ServerMessager.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMessager.Models
{
    public class Command
    {
        public string Action { get; set; }
        public Entity Entity { get; set; }
    }
}
