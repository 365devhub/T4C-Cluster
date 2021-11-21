using Akka.Actor;
using Akka.IO;
using Akka.Persistence;
using System.Linq;
using System.Net;
using T4c_Cluster.Node.Worker.Sessions.PlayerActor;
using T4C_Cluster.Lib.Cluster;
using T4C_Cluster.Lib.Network.Datagram;
using T4C_Cluster.Lib.Network.Datagram.Container;

namespace T4c_Cluster.Node.Worker.Actors
{
    public class PlayerActor : ReceivePersistentActor
    {
        private IActorRef _UdpSender;
        public override string PersistenceId => Self.Path.Name;

        private PlayerSession Session = new PlayerSession();


        public PlayerActor()
        {
            Command<ShardedMessageDatagram>(OnDatagram);
        }

        public void OnDatagram(ShardedMessageDatagram message)
        {
            _UdpSender = Sender;


            var datagram = new Datagram(message.Datas.ToArray(), message.Datas.Count());

            if (!datagram.IsValid || datagram.Header.Splited)
                return;

            if (datagram.IsAck)
                RemoveDatagramToRelaunch(datagram.Header);

            if (datagram.IsAck)
                return;

            if (datagram.Header.NeedAck)
                _UdpSender.Tell(new ShardedMessageDatagram(ByteString.CopyFrom(datagram.MakeAck()).ToArray(), message.EndPoint));

            if (!ResgisterDatagramID(datagram.Header.Id))
                return;

            if (datagram.Header.Compressed)
                datagram.Body.DecompressBuffer();
        
        
            Session.LastDatagramElapsedTime.Restart();
            SaveSnapshot(Session);

            var datagramTyped = DatagramReader.Read(datagram.Body);

            if (datagramTyped == null)
                return;

            if (!datagramTyped.IsValid())
                return;

            if (datagramTyped.NeedAuthentification() && !Session.IsAuthenticated)
                return;

            
            Self.Tell(datagramTyped);
        }

        /// <summary>
        /// Retirer des datagram de la liste des datagrammes à relancer
        /// </summary>
        /// <param name="header">Header à valider</param>
        private void RemoveDatagramToRelaunch(DatagramHeader header)
        {
            Session.DatagramsToRelaunch.RemoveAll(x => x.Datagram.Header.Id == header.Id && x.Datagram.Header.PartNumber == header.PartNumber);     
        }

        /// <summary>
        /// Enregistrer un identifiant de datagramme
        /// </summary>
        /// <param name="datagramId">L'identifiant</param>
        /// <returns> true if inserted  false if already in list</returns>
        private bool ResgisterDatagramID(ushort datagramId)
        {
            if (Session.AlreadyTreatedDatagramIndex == PlayerSession.MaximumAlreadyTreatedDatagramIndex)
            {
                Session.AlreadyTreatedDatagramIndex = 0;
            }

            if (Session.AlreadyTreatedDatagram.Any(x => x == datagramId))
                return false;

            Session.AlreadyTreatedDatagram[Session.AlreadyTreatedDatagramIndex] = datagramId;

            Session.AlreadyTreatedDatagramIndex++;

            return true;
        }

    }
}