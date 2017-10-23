using System;
using System.Threading;
using CommandLine;
using Grpc.Core;
using Test.Common;

namespace Test.Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            CmdOptions options = new CmdOptions();
            Parser parser = new Parser(with => with.HelpWriter = Console.Error);
            if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(-2)))
            {
                Channel channel = new Channel("127.0.0.1:38846", ChannelCredentials.Insecure);

                Broadcaster.BroadcasterClient client = new Broadcaster.BroadcasterClient(channel);
                string user = options.Name;
                Console.WriteLine($"Use name \"{user}\".");

                var reply = client.Register(new ListenerInfo { Name = user });
                var reader = reply.ResponseStream;
                new Program().DoWork(reader);

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                channel.ShutdownAsync().Wait();
            }
        }

        private async void DoWork(IAsyncStreamReader<Broadcast> reader)
        {
            // TODO: Handle exception here.
            while (await reader.MoveNext(CancellationToken.None))
            {
                string s = reader.Current.Message;
                Console.WriteLine("----------Message received----------");
                Console.WriteLine(s);
            }
        }
    }
}
