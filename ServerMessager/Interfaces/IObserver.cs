using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerMessager.Interfaces
{
    public interface IObserver
    {
        Task UpdateAsync();
    }
}
