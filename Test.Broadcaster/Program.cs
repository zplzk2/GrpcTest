using System;
using Grpc.Core;

namespace Test.Broadcaster
{
    class Program
    {
        static void Main(string[] args)
        {
            int Port = 38846;

            ServerImpl impl = new ServerImpl();
            Server server = new Server
            {
                Services = { Common.Broadcaster.BindService(impl) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine($"Listening on port {Port}.");
            Console.WriteLine("Type 'all {msg}' to broadcast.");
            Console.WriteLine("Type '{name} {msg}' to send to user named {name}.");

            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                    break;

                string[] split = input.Split(' ');
                if (split.Length >= 2)
                {
                    if (split[0] == "all" && input.Length > 4)
                    {
                        string msg = input.Substring(4);
                        impl.SendAll(msg);
                    }
                    else
                    {
                        int len = split[0].Length;
                        string msg = input.Substring(len + 1);
                        impl.Send(split[0], msg);
                    }
                }
            }

            server.ShutdownAsync().Wait();
        }
    }
}
