using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Messager.Models.Requests
{
    public interface IRequest : IDisposable
    {
        TcpClient Client { get; set; }
        string Message { get; set; }
        Task<Response> SendRequest();
    }
}
