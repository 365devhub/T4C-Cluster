using Akka.Actor;
using Akka.Cluster;
using Akka.Cluster.Sharding;
using Akka.IO;
using Akka.Persistence;
using Autofac.Extras.Attributed;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using T4C_Cluster.Lib.Cluster;
using T4C_Cluster.Node.Network.Configuration;

namespace T4C_Cluster.Node.Network
{
    public class NetworkActor : ReceivePersistentActor
    {
        private readonly IOptions<NetworkConfiguration> _networkConfiguration;
        private readonly IActorRef _region;
        private IActorRef SenderUdp;

        public override string PersistenceId => Self.Path.Name;
        public NetworkActor(IOptions<NetworkConfiguration> networkConfiguration,[WithKey("RegionPlayerActor")] IActorRef region)
        {
            _networkConfiguration = networkConfiguration;

            Context.System.Udp().Tell(new Udp.Bind(Self, new IPEndPoint(IPAddress.Parse(networkConfiguration.Value.Address), networkConfiguration.Value.Port)));
            _region = region;

            Command<Udp.Received>(OnUdpReceive);
            Command<ShardedMessageDatagram>(OnWorkerSending);

        }

        private void OnWorkerSending(ShardedMessageDatagram message)
        {
            SenderUdp.Tell(Udp.Send.Create(ByteString.FromBytes(message.Datas), IPEndPoint.Parse(message.EndPoint)));
        }

        private void OnUdpReceive(Udp.Received obj)
        {
           SenderUdp = Sender;
            _region.Tell(new ShardingEnvelope(Convert.ToBase64String(Encoding.UTF8.GetBytes(obj.Sender.ToString())), new ShardedMessageDatagram(obj.Data.ToArray(),obj.Sender.ToString())));
        }
    }
}
