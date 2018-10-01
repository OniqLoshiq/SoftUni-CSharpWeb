﻿using SIS.WebServer.Routing;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SIS.WebServer
{
    public class Server
    {
        private readonly string LocalhostIpAddress = "127.0.0.1";

        private readonly int port;

        private readonly TcpListener listener;

        private readonly ServerRoutingTable serverRoutingTable;

        private bool isRunning;

        public Server(int port, ServerRoutingTable serverRoutingTable)
        {
            this.port = port;
            this.listener = new TcpListener(IPAddress.Parse(LocalhostIpAddress), port);

            this.serverRoutingTable = serverRoutingTable;
        }

        public void Run()
        {
            this.listener.Start();
            this.isRunning = true;

            Console.WriteLine($"Server started at http://{LocalhostIpAddress}:{port}");

            var task = Task.Run(this.ListenLoop);
            task.Wait();
        }

        public async Task ListenLoop()
        {
            while(isRunning)
            {
                var client = await this.listener.AcceptSocketAsync();
                var connectionHandler = new ConnectionHandler(client, this.serverRoutingTable);
                await connectionHandler.ProcessRequestAsync();
            }
        }
    }
}
