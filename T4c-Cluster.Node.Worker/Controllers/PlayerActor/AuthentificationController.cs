using Akka.Actor;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4c_Cluster.Node.Worker.Attributes;
using T4c_Cluster.Node.Worker.Controlers;
using T4c_Cluster.Node.Worker.Sessions.PlayerActor;
using T4C_Cluster.Lib;
using T4C_Cluster.Lib.Network.Datagram.Message;
using static T4c_Cluster.Node.Worker.Actors.PlayerActor;
using static T4C_Cluster.API.Account;
using static T4C_Cluster.API.Configuration;

namespace T4c_Cluster.Node.Worker.Controllers.PlayerActor
{
    public class AuthentificationController : IControlerAction<RequestAuthenticateServerVersion, PlayerSession>,
                                              IControlerAction<RequestMessageOfTheDay, PlayerSession>,
                                              IControlerAction<RequestPatchServerInfoNew, PlayerSession>,
                                              IControlerAction<RequestRegisterAccount, PlayerSession>,
                                              //IControlerAction<RequestExitGame, PlayerSession>,
                                              IControlerAction<RequestAck, PlayerSession>


    {
        private ConfigurationClient _configurationClient;
        private AccountClient _accountClient;

        public AuthentificationController(ConfigurationClient configurationClient, AccountClient accountClient)
        {
            _configurationClient = configurationClient;
            _accountClient = accountClient;
        }

        [ValidatePlayerAuthenticated]
        [ValidatePlayerNotInGame]
        public void Action(RequestAuthenticateServerVersion data, PlayerSession session, IActorRef actor)
        {
            if (data.Version == Constants.SERVER_VERSION)
                actor.Tell(new ResponseAuthenticateServerVersion() { ServerVersion = 1 });
            else
                actor.Tell(new ResponseAuthenticateServerVersion() { ServerVersion = 0 });
        }

        //[ValidatePlayerAuthenticated]
        //[ValidatePlayerNotInGame]
        public void Action(RequestAck data, PlayerSession session, IActorRef actor)
        {
            actor.Tell(ScheduledEvent.ScheduleAck);
        }
        /*
        //[ValidatePlayerAuthenticated]
        //[ValidatePlayerNotInGame]
        public void Action(RequestExitGame data, PlayerSession session, IActorRef actor)
        {
            
        }*/

        [SaveSnapshot]
        [ValidatePlayerNotAuthenticated]
        [ValidatePlayerNotInGame]
        public void Action(RequestRegisterAccount data, PlayerSession session, IActorRef actor)
        {
            var psplit = data.Password.Split("\\");
            var auth = new T4C_Cluster.API.AuthenticateRequest() { Username = data.Account, Password = psplit[0], Token = psplit.Length == 2 ? psplit[1] : "" };
            if (_accountClient.Authenticate(auth).Result)
            {
                actor.Tell(new ResponseRegisterAccount() { Code = 0, Message = "haaaaa", UniqueKey = 1010101 });
                session.IsAuthenticated = true;
            }
            else
            {
                actor.Tell(new ResponseRegisterAccount() { Code = 1, Message = "merde", UniqueKey = 0 });
            }

        }

        [ValidatePlayerNotAuthenticated]
        [ValidatePlayerNotInGame]
        public void Action(RequestPatchServerInfoNew data, PlayerSession session, IActorRef actor)
        {
            var info = _configurationClient.GetPatchServerInformations(new T4C_Cluster.API.PatchServerInformationsRequest());
            actor.Tell(new ResponsePatchServerInfoNew()
            {
                ImagePath = info.ImagePath,
                Lang = (ushort?)info.Lang,
                Password = info.Password,
                ServerVersion = Constants.SERVER_VERSION,
                Username = info.Username,
                WebPatchIP = info.WebPatchIP,
            });

        }

        [ValidatePlayerNotAuthenticated]
        public void Action(RequestMessageOfTheDay data, PlayerSession session, IActorRef actor)
        {
            actor.Tell(new ResponseMessageOfTheDay() { Message = _configurationClient.GetMessageOfTheDay(new T4C_Cluster.API.MessageOfTheDayRequest()).Message });
        }
    }
}
