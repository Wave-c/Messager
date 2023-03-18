using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMessager.Models.Entitys
{
    public class Message : Entity
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public string Information { get; set; }
    }
}
