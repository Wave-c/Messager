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
        public static async Task<IRequest> CreateRequestAsync<TRequest, TEntity>(params TEntity[] entity) where TRequest : IRequest where TEntity : Entity
        {
            var type = typeof(TRequest);
            switch(type.Name)
            {
                case nameof(RegisterRequest):
                    return await CreateRegisterRequestAsync(entity[0] as User);
                case nameof(LoginRequest):
                    return await CreateLoginRequestAsync(entity[0] as User);
                case nameof(GetChatsRequest):
                    return await CreateGetChatsRequestAsync(entity[0] as User);
                case nameof(SearchRequest):
                    return await CreateSearchRequestAsync(entity[0] as SearchedString);
                case nameof(AddInFriendsRequest):
                    return await CreateAddInFriendsRequest(entity[0] as User, entity[1] as User);
                case nameof(CloseRequest):
                    return await CreateCloseRequest(entity[0] as User);
                case nameof(GetUserImageRequest):
                    return await CreateGetUserImageRequest(entity[0] as User);
            }
            throw new FormatException();
        }

        private static async Task<GetUserImageRequest> CreateGetUserImageRequest(User user)
        {
            var newGetUserImageRequest = new GetUserImageRequest()
            {
                Client = new TcpClient(),
                Message = $"GetUserImage\r\n {JsonSerializer.Serialize(user)}"
            };
            await newGetUserImageRequest.Client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);
            return newGetUserImageRequest;
        }
        private static async Task<CloseRequest> CreateCloseRequest(User user)
        {
            var newCloseRequest = new CloseRequest()
            {
                Client = new TcpClient(),
                Message = $"Close\r\n {JsonSerializer.Serialize(user)}"
            };
            await newCloseRequest.Client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);
            return newCloseRequest;
        }
        private static async Task<AddInFriendsRequest> CreateAddInFriendsRequest(User user, User addingUser)
        {
            var newAddInFriendsRequest = new AddInFriendsRequest()
            {
                Client = new TcpClient(),
                Message = $"AddInFriends\r\n {JsonSerializer.Serialize(user)} \r\n {JsonSerializer.Serialize(addingUser)}"
            };
            await newAddInFriendsRequest.Client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);
            return newAddInFriendsRequest;
        }
        private static async Task<SearchRequest> CreateSearchRequestAsync(SearchedString searchedString)
        {
            var newSearchRequest = new SearchRequest()
            {
                Client = new TcpClient(),
                Message = $"Search\r\n {searchedString.SearchedUserString}"
            };
            await newSearchRequest.Client.ConnectAsync(IPAddress.Parse("127.0.0.1"), 8888);
            return newSearchRequest;
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
