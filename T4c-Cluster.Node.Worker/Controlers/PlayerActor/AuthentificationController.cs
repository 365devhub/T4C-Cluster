using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4c_Cluster.Node.Worker.Attributes;
using T4c_Cluster.Node.Worker.Sessions.PlayerActor;
using T4C_Cluster.Lib.Network.Datagram.Message;

namespace T4c_Cluster.Node.Worker.Controlers.PlayerActor
{
    public class AuthentificationController : IControlerAction<RequestAuthenticateServerVersion, PlayerSession,Actors.PlayerActor>,
                                              IControlerAction<RequestMessageOfTheDay, PlayerSession, Actors.PlayerActor>,
                                              IControlerAction<RequestPatchServerInfoNew, PlayerSession, Actors.PlayerActor>,
                                              IControlerAction<RequestRegisterAccount, PlayerSession, Actors.PlayerActor>,
                                              IControlerAction<RequestExitGame, PlayerSession, Actors.PlayerActor>,
                                              IControlerAction<RequestAck, PlayerSession, Actors.PlayerActor>
    {

        public void Action(RequestAuthenticateServerVersion data, PlayerSession session, Actors.PlayerActor actor)
        {

        }

        public void Action(RequestAck data, PlayerSession session, Actors.PlayerActor actor)
        {

        }

        public void Action(RequestExitGame data, PlayerSession session, Actors.PlayerActor actor)
        {
            
        }

        [ValidatePlayerNotAuthenticated]
        public void Action(RequestRegisterAccount data, PlayerSession session, Actors.PlayerActor actor)
        {
            actor.SendToClient(new ResponseRegisterAccount() { Code=0, Message = "haaaaa", UniqueKey = 1010101 } );

        }

        [ValidatePlayerNotAuthenticated]
        public void Action(RequestPatchServerInfoNew data, PlayerSession session, Actors.PlayerActor actor)
        {
            actor.SendToClient(new ResponsePatchServerInfoNew() { 
                 ImagePath = null,
                 Lang=null,
                 Password = null,
                 ServerVersion=1720,
                 Username=null,
                 WebPatchIP = null,
            });

        }

        [ValidatePlayerNotAuthenticated]
        public void Action(RequestMessageOfTheDay data, PlayerSession session, Actors.PlayerActor actor)
        {
            actor.SendToClient(new ResponseMessageOfTheDay() { Message = "aaaaa" });
        }
    }
}
