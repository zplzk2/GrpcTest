using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Test.Common;

namespace Test.Broadcaster
{
    class ServerImpl : Common.Broadcaster.BroadcasterBase
    {
        public List<ListenerSkeleton> requests = new List<ListenerSkeleton>();

        public override Task Register(ListenerInfo request, IServerStreamWriter<Broadcast> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"Listener named \"{request.Name}\" registered.");

            requests.Add(new ListenerSkeleton(request, responseStream));

            // TODO: I don't like this.
            while (true)
                Thread.Sleep(1000);

            return null;
        }

        internal void SendAll(string msg)
        {
            foreach (var req in requests)
            {
                req.responseStream.WriteAsync(new Broadcast { Message = $"Date:    {DateTime.Now.ToString()}\nTo:      all\nMessage: {msg}" });
            }
        }

        /// <summary>
        /// You can register with duplicated names, but I will only send to the first one.
        /// </summary>
        internal bool Send(string to, string msg)
        {
            ListenerSkeleton req = requests.FirstOrDefault(r => r.listener.Name == to);
            if (req != null)
            {
                req.responseStream.WriteAsync(new Broadcast { Message = $"Date:    {DateTime.Now.ToString()}\nTo:      {req.listener.Name}\nMessage: {msg}" });
                return true;
            }
            else
                return false;
        }
    }
}
