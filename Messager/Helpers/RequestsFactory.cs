using Messager.Models.Entitys;
using Messager.Models.Requests;
using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;

namespace Messager.Helpers
{
    public class RequestsFactory
    {
        public static async Task<IRequest> CreateRequestAsync<TRequest, TEntity>(TEntity entity) where TRequest : IRequest where TEntity : Entity
        {
            var type = typeof(TRequest);
            switch(type.Name)
            {
                case nameof(RegisterRequest):
                    return await CreateRegisterRequestAsync(entity as User);
                case nameof(LoginRequest):
                    return await CreateLoginRequestAsync(entity as User);
                case nameof(GetChatsRequest):
                    return await CreateGetChatsRequestAsync(entity as User);
            }
            throw new FormatException();
        }
        private static async Task<RegisterRequest> CreateRegisterRequestAsync(User user)
        {
            var newRegisterRequest = new RegisterRequest()
            {
                Client = new TcpClient(),
                AddedUser = user,
                Message = $"Register\r\n {JsonSerializer.Serialize(user)}"
            };
            await newRegisterRequest.Client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);
            return newRegisterRequest;
        }
        private static async Task<LoginRequest> CreateLoginRequestAsync(User user)
        {
            var newLoginRequest = new LoginRequest()
            {
                Client = new TcpClient(),
                IncomingUser = user,
                Message = $"Login\r\n {JsonSerializer.Serialize(user)}"
            };
            await newLoginRequest.Client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);
            return newLoginRequest;
        }
        private static async Task<GetChatsRequest> CreateGetChatsRequestAsync(User user)
        {
            var newGetChatsRequest = new GetChatsRequest()
            {
                Client = new TcpClient(),
                Message = $"GetChats\r\n {JsonSerializer.Serialize(user)}"
            };
            await newGetChatsRequest.Client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);
            return newGetChatsRequest;
        }
    }
}
