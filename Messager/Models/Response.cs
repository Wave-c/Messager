using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messager.Models
{
    public class Response
    {
        public int ResponseCode { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return $"{ResponseCode}\n{ErrorMessage}";
        }
    }
}
