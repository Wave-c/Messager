using Messager.Helpers;
using Messager.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Messager.Models.Requests
{
    public class RegisterRequest : IRequest
    {
        public User AddedUser { get; set; }
        public string Message { get; set; }
        public TcpClient Client { get; set; }

        public void Dispose()
        {
            Client.Close();
        }

        public async Task<Response> SendRequest()
        {
            await SendReceiveMessage.SendMessageAsync(Client, Message);
            string strResponse = await SendReceiveMessage.ReceiveMessageAsync(Client);
            var response = JsonSerializer.Deserialize<Response>(strResponse);
            return response;
        }
    }
}
