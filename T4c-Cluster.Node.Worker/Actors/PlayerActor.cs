using Akka.Actor;
using Akka.IO;
using Akka.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using T4c_Cluster.Node.Worker.Controlers;
using T4c_Cluster.Node.Worker.Controlers.PlayerActor;
using T4c_Cluster.Node.Worker.Sessions.PlayerActor;
using T4C_Cluster.Lib.Cluster;
using T4C_Cluster.Lib.Network.Datagram;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Container;
using T4C_Cluster.Lib.Network.Datagram.Message;

namespace T4c_Cluster.Node.Worker.Actors
{
    public class PlayerActor : ReceivePersistentActor
    {
        public enum ScheduledEvent 
        {
            RelaunchMaintenance,
            LaunchAck,
            ScheduleAck,
            ScheduleAckForce
        }
        public override string PersistenceId => Self.Path.Name;

        private PlayerSession Session = new PlayerSession();
        private AuthentificationController _authController;

        private IActorRef _UdpSender;
        private string _EndPoint;
        public PlayerActor(AuthentificationController authController)
        {
            _authController = authController;
            this.RegisterControlerCommand(authController, Session, () => this.SaveSnapshot(this.Session),Self);

            Context.System.Scheduler.ScheduleTellRepeatedly(new System.TimeSpan(0), new System.TimeSpan(0, 0, 0, 0, 10), Self, ScheduledEvent.RelaunchMaintenance, ActorRefs.NoSender);

            Command<ShardedMessageDatagram>(OnDatagram);
            Command<ScheduledEvent>(OnRelaunchMaintenance, i => i == ScheduledEvent.RelaunchMaintenance);
            Command<ScheduledEvent>(OnLaunchAck, i => i == ScheduledEvent.LaunchAck);
            Command<ScheduledEvent>(ScheduleAck, i => i == ScheduledEvent.ScheduleAck);
            Command<ScheduledEvent>(ScheduleAckForce, i => i == ScheduledEvent.ScheduleAckForce);
            Command<IResponse>(OnIResponse);
            Recover<SnapshotOffer>(reco => OnRecovery((PlayerSession)reco.Snapshot));
        }

        private void OnRecovery(PlayerSession session) {
            
            Session = session;
            if (Session.IsAckScheduled)
            {
                Self.Tell(ScheduledEvent.ScheduleAckForce);
            }
        }
        private void ScheduleAck(ScheduledEvent obj)
        {
            _ScheduleAck();
        }

        private void ScheduleAckForce(ScheduledEvent obj)
        {
            _ScheduleAck(true);
        }

        private void _ScheduleAck(bool force = false)
        {
            if (Session.IsAckScheduled && !force)
                return;

            Session.IsAckScheduled = true;
            Context.System.Scheduler.ScheduleTellRepeatedly(new System.TimeSpan(0), new System.TimeSpan(0,0,2), Self, ScheduledEvent.LaunchAck, ActorRefs.NoSender);
            
            this.SaveSnapshot(this.Session);
        }

        public void OnIResponse(IResponse response)
        {
            var needAck = ((DatagramTypeAttribute)response.GetType().GetCustomAttributes(typeof(DatagramTypeAttribute),true).Single()).NeedAck;
            var datagrams = MessageWriter.ToDatagramBody(response).GenerateDatagrams(false, needAck, GetNextDatagramId, GetNextGroupId);
            
            
            datagrams.ForEach((d) => {
                if(needAck)
                    AddDatagramToRelaunch(d);
                _UdpSender.Tell(new ShardedMessageDatagram(ByteString.CopyFrom(d.GenerateBuffer()).ToArray(), _EndPoint));
            });

        }


        private void OnLaunchAck(ScheduledEvent obj)
        {
            OnIResponse(new ResponseAck());
        }

        private void OnRelaunchMaintenance(ScheduledEvent evnt)
        {
            var datagramToRelaunch = GetDatagramsToRelaunch();
            foreach (var inx in datagramToRelaunch)
            {
                _UdpSender.Tell(new ShardedMessageDatagram(ByteString.CopyFrom(inx.GenerateBuffer()).ToArray(), _EndPoint));
            }
        }

        public void OnDatagram(ShardedMessageDatagram message)
        {
            _UdpSender = Sender;
            _EndPoint = message.EndPoint;

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

        /// <summary>
        /// Add a datagram to the resend queue
        /// </summary>
        /// <param name="datagram">The datagram</param>
        /// <param name="Interval">Resend interval</param>
        /// <param name="RelaunchCountTotal">Total number of time to send</param>
        private void AddDatagramToRelaunch(Datagram datagram, int Interval = 20, int RelaunchCountTotal = 5)
        {
            Session.DatagramsToRelaunch.Add(new PlayerSessionDatagramToRelaunch() { Datagram = datagram, RelaunchInterval = Interval, RelaunchCountTotal = RelaunchCountTotal });
        }

        /// <summary>
        /// Get a list of datagram to resend. if the datagram reached its resend limit it is automatically removed
        /// </summary>
        /// <returns>the list of datagram to resend.</returns>
        private IList<Datagram> GetDatagramsToRelaunch()
        {
            Session.DatagramsToRelaunch.RemoveAll(x => x.RelaunchCount >= x.RelaunchCountTotal);

            var re = Session.DatagramsToRelaunch.Where(x => {
                bool ret = x.ElapsedTime.ElapsedMilliseconds > x.RelaunchInterval;

                x.RelaunchCount += 1;

                x.ElapsedTime.Restart();
                return ret;
            });


            return re.Select(x => x.Datagram).ToList();
        }

        /// <summary>
        /// Obtention de l'identifiant à utilisé pour le prochain datagramme à envoyer
        /// </summary>
        /// <returns></returns>
        private ushort GetNextDatagramId()
        {
            Session.NextDatagramId += 1;
            if (Session.NextDatagramId >= PlayerSession.MaximumDatagramId)
                Session.NextDatagramId = 0;

            return Session.NextDatagramId;
        }


        /// <summary>
        /// Obtention de l'identifiant de groupe à utiliser pour le prochain datagramme à envoyer séparé
        /// </summary>
        /// <returns></returns>
        private byte GetNextGroupId()
        {
            Session.NextGroupeId += 1;
            if (Session.NextGroupeId == PlayerSession.MaximumGroupId)
                Session.NextGroupeId = 0;

            return Session.NextGroupeId;
        }

        

        
    }
}