using Grpc.Core;
using Test.Common;

namespace Test.Broadcaster
{
    class ListenerSkeleton
    {
        internal ListenerInfo listener;
        internal IServerStreamWriter<Broadcast> responseStream;

        public ListenerSkeleton(ListenerInfo req, IServerStreamWriter<Broadcast> resp)
        {
            listener = req;
            responseStream = resp;
        }
    }
}
