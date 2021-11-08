using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using Fleck;

namespace WebSocketTest
{
    class Program
    {

        static void Main(string[] args)
        {
            var server = new WebSocketServer("ws://127.0.0.1:8001");
            List<User> connections = new List<User>();
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    User user = new User(socket);
                    connections.Add(user);
                    Console.WriteLine("Connection opened " + user.Username + " in room " + user.RoomNumber);
                };
                socket.OnMessage = message =>
                {
                    User sendUser = connections.Find(u => u.Id == socket.ConnectionInfo.Id);
                    List<User> roomMembers = connections.FindAll(u => u.RoomNumber == sendUser.RoomNumber);

                    foreach (var user in roomMembers)
                    {
                        user.SocketConnection.Send(sendUser.Username + " " + message);
                    }
                };

                socket.OnClose = () =>
                {
                    User user = connections.Find(u => u.Id == socket.ConnectionInfo.Id);
                    connections.Remove(user);
                    Console.WriteLine(user.Username + " Disconnected");
                };
            });
            
            Console.ReadKey(true);
        }
    }
}