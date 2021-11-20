using Akka.Actor;
using Akka.Persistence;
using T4C_Cluster.Lib.Cluster;

namespace T4c_Cluster.Node.Worker
{
    public class PlayerActor : ReceivePersistentActor
    {
        private IActorRef _UdpSender;
        public override string PersistenceId => Self.Path.Name;
        public PlayerActor()
        {
            Command<ShardedMessageDatagram>(OnDatagram);
        }

        public void OnDatagram(ShardedMessageDatagram datagram) 
        {
            _UdpSender = Sender;
        }

    }
}