using ServerMessager.Helpers;
using ServerMessager.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerMessager.Models.Requests
{
    public class LoginRequest : IRequest
    {
        public User IncomingUser { get; set; }
        public string Message { get; set; }
        public TcpClient Client { get; set; }

        public void Dispose()
        {
            Client.Close();
        }

        public async Task<Response> SendRequest()
        {
            await SendReceiveMessage.SendMessageAsync(Client, Message);
            return new Response() { ResponseCode = -1 };
        }
    }
}
