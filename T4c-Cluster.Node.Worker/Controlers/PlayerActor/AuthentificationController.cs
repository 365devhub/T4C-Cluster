using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4c_Cluster.Node.Worker.Attributes;
using T4c_Cluster.Node.Worker.Sessions.PlayerActor;
using T4C_Cluster.Lib;
using T4C_Cluster.Lib.Network.Datagram.Message;
using static T4c_Cluster.Node.Worker.Actors.PlayerActor;

namespace T4c_Cluster.Node.Worker.Controlers.PlayerActor
{
    public class AuthentificationController : IControlerAction<RequestAuthenticateServerVersion, PlayerSession>/*,
                                              IControlerAction<RequestMessageOfTheDay, PlayerSession>,
                                              IControlerAction<RequestPatchServerInfoNew, PlayerSession>,
                                              IControlerAction<RequestRegisterAccount, PlayerSession>,
                                              IControlerAction<RequestExitGame, PlayerSession>,
                                              IControlerAction<RequestAck, PlayerSession>*/


    {

        [ValidatePlayerAuthenticated]
        [ValidatePlayerNotInGame]
        public void Action(RequestAuthenticateServerVersion data, PlayerSession session, IActorRef actor)
        {
            if(data.Version == Constants.SERVER_VERSION)
                actor.Tell(new ResponseAuthenticateServerVersion() { ServerVersion=1 });
            else
                actor.Tell(new ResponseAuthenticateServerVersion() { ServerVersion=0 });
        }

       /* [ValidatePlayerAuthenticated]
        [ValidatePlayerNotInGame]
        public void Action(RequestAck data, PlayerSession session, IActorRef actor)
        {
            actor.Tell(ScheduledEvent.ScheduleAck);
        }

        [ValidatePlayerAuthenticated]
        [ValidatePlayerNotInGame]
        public void Action(RequestExitGame data, PlayerSession session, IActorRef actor)
        {
            
        }

        [SaveSnapshot]
        [ValidatePlayerNotAuthenticated]
        [ValidatePlayerNotInGame]
        public void Action(RequestRegisterAccount data, PlayerSession session, IActorRef actor)
        {
            actor.Tell(new ResponseRegisterAccount() { Code=0, Message = "haaaaa", UniqueKey = 1010101 } );
            session.IsAuthenticated = true;
        }

        [ValidatePlayerNotAuthenticated]
        [ValidatePlayerNotInGame]
        public void Action(RequestPatchServerInfoNew data, PlayerSession session, IActorRef actor)
        {
            actor.Tell(new ResponsePatchServerInfoNew() { 
                 ImagePath = null,
                 Lang=null,
                 Password = null,
                 ServerVersion = Constants.SERVER_VERSION,
                 Username=null,
                 WebPatchIP = null,
            });

        }

        [ValidatePlayerNotAuthenticated]
        public void Action(RequestMessageOfTheDay data, PlayerSession session, IActorRef actor)
        {
            actor.Tell(new ResponseMessageOfTheDay() { Message = "aaaaa" });
        }*/
    }
}
