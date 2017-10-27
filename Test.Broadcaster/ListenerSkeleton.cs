using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Test.Common;

namespace Test.Broadcaster
{
    class ListenerSkeleton
    {
        internal ListenerInfo listener;
        internal IServerStreamWriter<Broadcast> responseStream;

        public Queue<string> SendQueue = new Queue<string>();
        public AutoResetEvent SendThreshold = new AutoResetEvent(false);

        public ListenerSkeleton(ListenerInfo req, IServerStreamWriter<Broadcast> resp)
        {
            listener = req;
            responseStream = resp;
        }

        internal async Task DoWork()
        {
            while (true)
            {
                if (SendQueue.Count > 0)
                {
                    string toSend = SendQueue.Dequeue();
                    await responseStream.WriteAsync(new Broadcast { Message = toSend });
                }
                else
                    SendThreshold.WaitOne();
            }
        }

        internal void Send(string msg)
        {
            SendQueue.Enqueue(msg);
            SendThreshold.Set();
        }
    }
}
