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
        public void Action_RequestGetPlayingCharacterList()
        {
            //Arrange
            var actor = Substitute.For<IActorRef>();
            var conf = Substitute.For<T4C_Cluster.API.Configuration.ConfigurationClient>();
            var conf2 = Substitute.For<T4C_Cluster.API.Character.CharacterClient>();
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

        [Test]
        [TestCase(true, DeleteCharErrorCode.OK)]
        [TestCase(true, DeleteCharErrorCode.NotYourPlayer)]
        [TestCase(false, DeleteCharErrorCode.OtherError)]
        public void Action_RequestDeleteCharacter(bool serverResult, DeleteCharErrorCode result)
        {
            //Arrange
            var actor = Substitute.For<IActorRef>();
            var conf = Substitute.For<T4C_Cluster.API.Configuration.ConfigurationClient>();
            var conf2 = Substitute.For<T4C_Cluster.API.Character.CharacterClient>();
            var data = new RequestDeleteCharacter() { Name = "paul" };


            var session = new PlayerSession() { Account = "aaaa" };
            if (result != DeleteCharErrorCode.NotYourPlayer)
                session.Caracters.Add("paul");

            var controller = new MainMenuController(conf, conf2);

            conf2.DeleteCaracter(Arg.Any<T4C_Cluster.API.DeleteCharacterRequest>()).Returns(new T4C_Cluster.API.DeleteCharacterReply() { Result = serverResult });
            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is<ResponseDeleteCharacter>(d=>d.ErrorCode == result));
        }
    }
}
