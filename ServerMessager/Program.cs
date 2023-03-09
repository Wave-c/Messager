using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text;
using ServerMessager.Interfaces;

namespace ServerMessager
{
#pragma warning disable CS4014 // Так как этот вызов не ожидается, выполнение существующего метода продолжается до тех пор, пока вызов не будет завершен
    internal class Program : IObservable
    {
        static List<Session> sessions = new List<Session>();

        static async Task Main(string[] args)
        {
            await new Program().ServerMethod();

            Console.ReadKey();
        }

        async Task ServerMethod()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
            tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            Task.Run(async() =>
            {
                while (true)
                {
                    await AcceptClientAsync(tcpListener);
                    await Task.Delay(10);
                }
            });
            Task.Run(async () =>
            {
                while (true)
                {
                    NotifyObserversAsync();
                    await Task.Delay(10);
                }
            });
        }

        async Task AcceptClientAsync(TcpListener tcpListener)
        {
            var client = await tcpListener.AcceptTcpClientAsync();
            AddObserver(new Session() { Client = client });
            Console.WriteLine($"Новое подключение {client.Client.RemoteEndPoint}");
        }

        public void AddObserver(IObserver o)
        {
            sessions.Add((Session)o);
        }

        public void RemoveObserver(IObserver o)
        {
            sessions.Remove((Session)o);
        }

        public async Task NotifyObserversAsync()
        {
            foreach(var session in sessions)
            {
                await session.UpdateAsync();
                RemoveObserver(session);
            }
        }
    }
}