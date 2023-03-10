using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ServerMessager.Helpers;
using ServerMessager.Interfaces;
using ServerMessager.Models;
using ServerMessager.Models.Entitys;

namespace ServerMessager
{
    public class Session : IObserver
    {
        public TcpClient Client { get; set; }

        public async Task UpdateAsync()
        {
            var message = await SendReceiveMessage.ReceiveMessageAsync(Client);
            var command = new Command()
            {
                Action = message.Split("\r\n")[0],
                Entity = JsonSerializer.Deserialize<User>(message.Split("\r\n")[1])
            };

            switch(command.Action)
            {
                case "Register":
                    await RegisterAsync(command);
                    break;
                case "Login":
                    await LoginAsync(command);
                    break;
                case "GetChats":
                    await GetChatsAsync(command);
                    break;
            }
        }

        public async Task RegisterAsync(Command command)
        {
            using(var dbContext = new AppDBContext())
            {
                if (dbContext.Users.Where(x => x.Name == ((User)command.Entity).Name).FirstOrDefault() == null)
                {
                    dbContext.Users.Add((User)command.Entity);
                    dbContext.SaveChanges();
                    Response response = new Response()
                    {
                        ResponseCode = 200
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }
                else
                {
                    Response response = new Response()
                    {
                        ErrorMessage = "Error: Such user already exists",
                        ResponseCode = 401
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }
            }
        }

        public async Task LoginAsync(Command command)
        {
            using(var dbContext = new AppDBContext())
            {
                if(dbContext.Users.Where(x => x.Name == ((User)command.Entity).Name && x.Password == ((User)command.Entity).Password).First() == null)
                {
                    Response response = new Response()
                    {
                        ErrorMessage = "Error: This user does not exist, please register",
                        ResponseCode = 401
                    };
                }
                else
                {
                    Response response = new Response()
                    {
                        ResponseCode = 200
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }
            }
        }

        public async Task GetChatsAsync(Command command)
        {
            List<User> chatsUsers = new List<User>();
            try
            {
                using (var dbContext = new AppDBContext())
                {
                    List<AddedInFriends> addedInFriends = dbContext.AddedInFriends.Where(x => command.Entity.Id == x.User1
                        || command.Entity.Id == x.User2).ToList();
                    if (addedInFriends.Count == 0)
                    {
                        Response response = new Response()
                        {
                            ErrorMessage = "Error: You don't have any chats",
                            ResponseCode = 406
                        };
                        await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                        return;
                    }
                    foreach (var item in addedInFriends)
                    {
                        chatsUsers.Add(dbContext.Users.Where(x => x.Id == item.User1).First());
                        chatsUsers.Add(dbContext.Users.Where(x => x.Id == item.User2).First());
                    }
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(chatsUsers));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
