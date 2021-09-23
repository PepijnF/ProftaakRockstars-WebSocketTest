using System;
using WebSocketSharp;

namespace WebSocketClient2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ws = new WebSocket ("ws://127.0.0.1:8001/quiz?name=persoontje&room=123")) {
                ws.OnMessage += (sender, e) =>
                    Console.WriteLine (e.Data);
                ws.OnError += (sender, eventArgs) =>
                    ws.Close();
                
                ws.Connect();
                while (true)
                {
                    ws.Send(Console.ReadLine());
                }
            }
        }
    }
}