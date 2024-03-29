﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerMessager.Helpers
{
    public static class SendReceiveMessage
    {
        public static async Task SendMessageAsync(TcpClient tcpClient, string message)
        {
                if (string.IsNullOrEmpty(message))
                    throw new ArgumentNullException("message");

                var stream = tcpClient.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(buffer, 0, buffer.Length);
        }

        public static async Task<string> ReceiveMessageAsync(TcpClient tcpClient)
        {
            var stream = tcpClient.GetStream();
            var builder = new StringBuilder();
            byte[] buffer = new byte[1024];

            do
            {
                //TODO: исправить вылет на этом моменте

                int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                string addingText = Encoding.UTF8.GetString(buffer, 0, bytes);
                builder.Append(addingText);
            } while (stream.DataAvailable);
   
            return builder.ToString();
        }
    }
}
