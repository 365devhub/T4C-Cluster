using Akka.Actor;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4c_Cluster.Node.Worker.Controllers.PlayerActor;
using T4c_Cluster.Node.Worker.Sessions.PlayerActor;
using T4C_Cluster.Lib.Network.Datagram.Message.MainMenu;

namespace T4c_Cluster.Node.Worker.Test.Controlers.PlayerActor
{
    [TestFixture]
    public class MainMenuControllerTest
    {
        [Test]
        public void Action_RequestAuthenticateServerVersion()
        {
            //Arrange
            var actor = Substitute.For<IActorRef>();
            var conf = Substitute.For<T4C_Cluster.API.Configuration.ConfigurationClient>();
            var conf2 = Substitute.For<T4C_Cluster.API.Caracter.CaracterClient>();
            var data = new RequestGetPlayingCharacterList() { };
            var session = new PlayerSession() { Account = "aaaa" };
            var controller = new MainMenuController(conf, conf2);

            conf2.GetCharacters(Arg.Any<T4C_Cluster.API.GetCharactersRequest>()).Returns(new T4C_Cluster.API.GetCharactersReply());
            conf.GetNbCharacterMax(Arg.Any<T4C_Cluster.API.NbCharacterMaxRequest>()).Returns(new T4C_Cluster.API.NbCharacterMaxReply() { NbMax = 1 });
            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Any<ResponseGetPlayingCharacterList>());
            actor.Received().Tell(Arg.Any<ResponseGetPlayingCharacterListEquitSkin>());
            actor.Received().Tell(Arg.Any<ResponseMaxCharacterPerAccount>());
        }
    }
}
