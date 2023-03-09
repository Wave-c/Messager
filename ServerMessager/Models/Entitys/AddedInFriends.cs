using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMessager.Models.Entitys
{
    public class AddedInFriends : Entity
    {
        public Guid User1 { get; set; }
        public Guid User2 { get; set; }
    }
}
