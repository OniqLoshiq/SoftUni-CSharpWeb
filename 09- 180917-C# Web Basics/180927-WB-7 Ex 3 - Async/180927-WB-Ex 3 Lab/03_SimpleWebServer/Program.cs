using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _03_SimpleWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            int port = 80;

            TcpListener listener = new TcpListener(ip, port);

            listener.Start();
            Console.WriteLine("Server started.");
            Console.WriteLine($"Listening to TCP clients at 127.0.0.1:{port}");

            var task = Task.Run(() => Connect(listener));
            task.Wait();
        }

        private static async Task Connect(TcpListener listener)
        {
            while(true)
            {
                Console.WriteLine("Waiting for client...");
                var client = await listener.AcceptTcpClientAsync();

                Console.WriteLine("Client connected.");

                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer);

                var request = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(request);

                byte[] data = Encoding.UTF8.GetBytes("Hello from the other side!");
                client.GetStream().Write(data);

                Console.WriteLine("Closing connection.");
                client.GetStream().Dispose();
            }
        }
    }
}
