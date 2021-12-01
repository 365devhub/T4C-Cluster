using Akka.Actor;
using AutoFixture;
using Grpc.Core;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4c_Cluster.Node.Worker.Controlers.PlayerActor;
using T4c_Cluster.Node.Worker.Sessions.PlayerActor;
using T4C_Cluster.Lib.Network.Datagram.Message;
using static T4C_Cluster.API.Configuration;

namespace T4c_Cluster.Node.Worker.Test.Controlers.PlayerActor
{
    [TestFixture]
    public class AuthentificationControllerTest
    {
        [Test]
        [TestCase(1720u, 1u)]
        [TestCase(1721u, 0u)]
        public void Action_RequestAuthenticateServerVersion(uint version,uint ret)
        {
            //Arrange
            var actor = Substitute.For<IActorRef>();
            var data = new RequestAuthenticateServerVersion() { Version= version };
            var session = new PlayerSession();
            var controller = new AuthentificationController(null);
            

            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is<ResponseAuthenticateServerVersion>(x => x.ServerVersion == ret));
            
        }

        /*[Test]
        public void Action_RequestAck()
        {
            //Arrange
            var actor = Substitute.For<IActorRef>();
            var data = new RequestAuthenticateServerVersion();
            var session = new PlayerSession();
            var controller = new AuthentificationController();


            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is());
        }

        [Test]
        public void Action_RequestExitGame()
        {
            //Arrange
            var actor = Substitute.For<IActorRef>();
            var data = new RequestAuthenticateServerVersion();
            var session = new PlayerSession();
            var controller = new AuthentificationController();


            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is());
        }

        [Test]
        public void Action_RequestRegisterAccount()
        {
            //Arrange
            var actor = Substitute.For<IActorRef>();
            var data = new RequestAuthenticateServerVersion();
            var session = new PlayerSession();
            var controller = new AuthentificationController();


            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is());
        }

        [Test]
        public void Action_RequestPatchServerInfoNew()
        {
            //Arrange
            var actor = Substitute.For<IActorRef>();
            var data = new RequestAuthenticateServerVersion();
            var session = new PlayerSession();
            var controller = new AuthentificationController();


            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is());
        }*/

        [Test]
        public void Action_RequestMessageOfTheDay()
        {
            //Arrange
            var actor = Substitute.For<IActorRef>();
            var conf = Substitute.For<T4C_Cluster.API.Configuration.ConfigurationClient>();
            var data = new RequestMessageOfTheDay();
            var session = new PlayerSession();
            var controller = new AuthentificationController(conf);

            conf.GetMessageOfTheDay(Arg.Any<T4C_Cluster.API.MessageOfTheDayRequest>()).Returns(new T4C_Cluster.API.MessageOfTheDayReply() { Message = "aaaa" });

            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is<ResponseMessageOfTheDay>(x=>x.Message == "aaaa"));
        }
    }
}
