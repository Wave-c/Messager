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
                Action = message.Split("\r\n")[0]
            };
            try
            {

                switch (command.Action)
                {
                    case "Register":
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[1]));
                        await RegisterAsync(command);
                        break;
                    case "Login":
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[1]));
                        await LoginAsync(command);
                        break;
                    case "GetChats":
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[1]));
                        await GetChatsAsync(command);
                        break;
                    case "Search":
                        SearchedString searchedUserString = new SearchedString()
                        {
                            SearchedUserString = message.Split("\r\n ")[1]
                        };
                        await SearchAsync(searchedUserString);
                        break;
                    case "Close":
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[1]));
                        await CloseAsync(command);
                        break;
                    case "AddInFriends":
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[1]));
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[2]));
                        await AddInFriendsAsync(command);
                        break;
                    case "GetUserImage":
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[1]));
                        await GetUserImageAsync(command);
                        break;
                    case "SetUserImage":
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[1]));
                        await SetUserImageAsync(command);
                        break;
                    case "DeleteFromFriends":
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[1]));
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[2]));
                        await DeleteFromFriendsAsync(command);
                        break;
                    case "SendMessage":
                        command.Entitys.Add(JsonSerializer.Deserialize<Message>(message.Split("\r\n")[1]));
                        await SendMessageAsync(command);
                        break;
                    case "ReceiveMessage":
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[1]));
                        command.Entitys.Add(JsonSerializer.Deserialize<User>(message.Split("\r\n")[2]));
                        await ReceiveMessageAsync(command);
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task ReceiveMessageAsync(Command command)
        {
            using (var dbContext = new AppDBContext())
            {
                List<Message> messages = new List<Message>();
                messages.AddRange(dbContext.Messages.Where(x => x.From == command.Entitys[0].Id && x.To == command.Entitys[1].Id));
                messages.AddRange(dbContext.Messages.Where(x => x.From == command.Entitys[1].Id && x.To == command.Entitys[0].Id));

                if(messages.Count == 0)
                {
                    var response = new Response()
                    {
                        ResponseCode = 406,
                        ErrorMessage = "You dont have messages with this user"
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }
                else
                {
                    var response = new Response()
                    {
                        ResponseCode = 200,
                        ResponseObj = JsonSerializer.Serialize(messages)
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }
                
            }
        }

        public async Task SendMessageAsync(Command command)
        {
            try
            {

                using (var dbContext = new AppDBContext())
                {
                    await dbContext.Messages.AddAsync((Message)command.Entitys[0]);
                    await dbContext.SaveChangesAsync();
                    var response = new Response()
                    {
                        ResponseCode = 200
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task DeleteFromFriendsAsync(Command command)
        {
            using (var dbContext = new AppDBContext())
            {
                List<AddedInFriends> addedInFriends = dbContext.AddedInFriends.Where(x => command.Entitys[0].Id == x.User1
                    || command.Entitys[0].Id == x.User2).ToList();
                dbContext.AddedInFriends.RemoveRange(addedInFriends);
                await dbContext.SaveChangesAsync();
                var response = new Response()
                {
                    ResponseCode = 200
                };
                await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
            }
        }

        public async Task SetUserImageAsync(Command command)
        {
            using (var dbContext = new AppDBContext())
            {
                dbContext.Users.Where(x => x.Id == command.Entitys[0].Id).First().Image = ((User)command.Entitys[0]).Image;
                await dbContext.SaveChangesAsync();
                var response = new Response()
                {
                    ResponseCode = 200
                };
                await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
            }
        }

        public async Task GetUserImageAsync(Command command)
        {
            using (var dbContext = new AppDBContext())
            {
                byte[] image = dbContext.Users.Where(x => x.Id == command.Entitys[0].Id).First().Image;
                if (image.Length != 0)
                {
                    var response = new Response()
                    {
                        ResponseCode = 200,
                        ResponseObj = Convert.ToBase64String(image)
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }
                else
                {
                    var response = new Response()
                    {
                        ResponseCode = 404,
                        ErrorMessage = "This user dont have photo"
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }
            }
        }

        public async Task AddInFriendsAsync(Command command)
        {
            using (var dbContext = new AppDBContext())
            {
                if(dbContext.AddedInFriends.Where(x=>x.User1 == command.Entitys[0].Id).FirstOrDefault() != null || dbContext.AddedInFriends.Where(x => x.User2 == command.Entitys[0].Id).FirstOrDefault() != null && dbContext.AddedInFriends.Where(x => x.User1 == command.Entitys[1].Id).FirstOrDefault() != null || dbContext.AddedInFriends.Where(x => x.User2 == command.Entitys[1].Id).FirstOrDefault() != null)
                {
                    var response = new Response()
                    {
                        ResponseCode = 412,
                        ErrorMessage = "You have already added this user as a friend"
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }
                else
                {
                    await dbContext.AddedInFriends.AddAsync(new AddedInFriends() { Id = Guid.NewGuid(), User1 = command.Entitys[0].Id, User2 = command.Entitys[1].Id });
                    await dbContext.SaveChangesAsync();
                    var response = new Response()
                    {
                        ResponseCode = 200
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }
            }
        }

        public async Task CloseAsync(Command command)
        {
            try
            {
                using (var dbContext = new AppDBContext())
                {
                    dbContext.Users.Where(x => x.Id == ((User)command.Entitys[0]).Id).First().Status = Status.OFFLINE;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task RegisterAsync(Command command)
        {
            using(var dbContext = new AppDBContext())
            {
                try
                {
                    if (dbContext.Users.Where(x => x.Name == ((User)command.Entitys[0]).Name).FirstOrDefault() == null)
                    {

                        User addedUser = (User)command.Entitys[0];
                        addedUser.Email = "";
                        addedUser.Image = Encoding.UTF8.GetBytes("");

                        dbContext.Users.Add(addedUser);
                        await dbContext.SaveChangesAsync();
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task LoginAsync(Command command)
        {
            using(var dbContext = new AppDBContext())
            {
                var loginUser = dbContext.Users.Where(x => x.Name == ((User)command.Entitys[0]).Name && x.Password == ((User)command.Entitys[0]).Password).FirstOrDefault();
                if (loginUser == null)
                {
                    Response response = new Response()
                    {
                        ErrorMessage = "Error: This user does not exist, please register",
                        ResponseCode = 401
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                }
                else
                {
                    loginUser.Status = Status.ONLINE;
                    await dbContext.SaveChangesAsync();
                    Response response = new Response()
                    {
                        ResponseCode = 200,
                        ResponseObj = loginUser.Id.ToString()
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
                    List<AddedInFriends> addedInFriends = dbContext.AddedInFriends.Where(x => command.Entitys[0].Id == x.User1
                        || command.Entitys[0].Id == x.User2).ToList();
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
                    else
                    {
                        foreach (var item in addedInFriends)
                        {
                            chatsUsers.Add(dbContext.Users.Where(x => x.Id == item.User1).First());
                            chatsUsers.Add(dbContext.Users.Where(x => x.Id == item.User2).First());
                            if (command.Entitys[0].Id == item.User1 || command.Entitys[0].Id == item.User2)
                            {
                                chatsUsers.Remove(chatsUsers.Where(x => x.Id == command.Entitys[0].Id).First());
                            }
                        }
                        var response = new Response()
                        {
                            ResponseCode = 200,
                            ResponseObj = JsonSerializer.Serialize(chatsUsers)
                        };
                        await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task SearchAsync(SearchedString searchedString)
        {
            using (var dbContext = new AppDBContext())
            {
                List<User> searchedUsers = dbContext.Users.Where(x => x.Name == searchedString.SearchedUserString).ToList();
                if(searchedUsers.Count != 0)
                {
                    Response response = new Response()
                    {
                        ResponseCode = 200,
                        ResponseObj = JsonSerializer.Serialize(searchedUsers)
                    };
                    await SendReceiveMessage.SendMessageAsync(Client, JsonSerializer.Serialize(response));
                    return;
                }
            }
        }
    }
}
