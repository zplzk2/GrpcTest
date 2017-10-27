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

        public override async Task Register(ListenerInfo request, IServerStreamWriter<Broadcast> responseStream, ServerCallContext context)
        {
            Console.WriteLine($"Listener named \"{request.Name}\" registered.");

            ListenerSkeleton skeleton = new ListenerSkeleton(request, responseStream);
            requests.Add(skeleton);

            await skeleton.DoWork();
        }

        internal void SendAll(string msg)
        {
            foreach (var req in requests)
            {
                req.Send(msg);
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
                req.Send(msg);
                return true;
            }
            else
                return false;
        }
    }
}
