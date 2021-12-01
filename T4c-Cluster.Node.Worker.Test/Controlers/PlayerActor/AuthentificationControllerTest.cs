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
using static T4c_Cluster.Node.Worker.Actors.PlayerActor;
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
            var conf = Substitute.For<T4C_Cluster.API.Configuration.ConfigurationClient>();
            var conf2 = Substitute.For<T4C_Cluster.API.Account.AccountClient>();
            var data = new RequestAuthenticateServerVersion() { Version= version };
            var session = new PlayerSession();
            var controller = new AuthentificationController(conf, conf2);
            

            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is<ResponseAuthenticateServerVersion>(x => x.ServerVersion == ret));
            
        }

        [Test]
        public void Action_RequestAck()
        {
            //Arrange
            var actor = Substitute.For<IActorRef>();
            var conf = Substitute.For<T4C_Cluster.API.Configuration.ConfigurationClient>();
            var conf2 = Substitute.For<T4C_Cluster.API.Account.AccountClient>();
            var data = new RequestAck();
            var session = new PlayerSession();
            var controller = new AuthentificationController(conf, conf2);


            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is<ScheduledEvent>(e => e == ScheduledEvent.ScheduleAck));
        }

        /*
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
            actor.Received().Tell(Arg.Is<ScheduledEvent>(e=>  e == ScheduledEvent.ScheduleAck));
        }*/

        [Test]
        [TestCase(true,0)]
        [TestCase(false, 1)]
        public void Action_RequestRegisterAccount(bool result, byte code)
        {
            //Arrange
            Fixture fixture = new Fixture();
            var actor = Substitute.For<IActorRef>();
            var conf = Substitute.For<T4C_Cluster.API.Configuration.ConfigurationClient>();
            var conf2 = Substitute.For<T4C_Cluster.API.Account.AccountClient>();
            var data = fixture.Create<RequestRegisterAccount>();
            var session = new PlayerSession();
            var controller = new AuthentificationController(conf,conf2);

            
            var ret = new T4C_Cluster.API.AuthenticateReply() { Result = result };
            conf2.Authenticate(Arg.Any<T4C_Cluster.API.AuthenticateRequest>()).Returns(ret);

            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is<ResponseRegisterAccount>(a=>a.Code == (byte?)code));
        }

        [Test]
        public void Action_RequestPatchServerInfoNew()
        {
            //Arrange
            Fixture fixture = new Fixture();
            var actor = Substitute.For<IActorRef>();
            var conf = Substitute.For<T4C_Cluster.API.Configuration.ConfigurationClient>();
            var conf2 = Substitute.For<T4C_Cluster.API.Account.AccountClient>();
            var data = new RequestPatchServerInfoNew();
            var session = new PlayerSession();
            var controller = new AuthentificationController(conf, conf2);

            var ret = fixture.Create<T4C_Cluster.API.PatchServerInformationsReply>();
            conf.GetPatchServerInformations(Arg.Any<T4C_Cluster.API.PatchServerInformationsRequest>()).Returns(ret);

            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is<ResponsePatchServerInfoNew>(x=>x.ImagePath == ret.ImagePath && 
                                                                        x.Lang == (ushort?)ret.Lang && 
                                                                        x.Password == ret.Password && 
                                                                        x.Username == ret.Username && 
                                                                        x.WebPatchIP == ret.WebPatchIP ));
        }

        [Test]
        public void Action_RequestMessageOfTheDay()
        {
            //Arrange
            Fixture fixture = new Fixture();
            var actor = Substitute.For<IActorRef>();
            var conf = Substitute.For<T4C_Cluster.API.Configuration.ConfigurationClient>();
            var conf2 = Substitute.For<T4C_Cluster.API.Account.AccountClient>();
            var data = new RequestMessageOfTheDay();
            var session = new PlayerSession();
            var controller = new AuthentificationController(conf, conf2);

            var ret = fixture.Create<T4C_Cluster.API.MessageOfTheDayReply>();
            conf.GetMessageOfTheDay(Arg.Any<T4C_Cluster.API.MessageOfTheDayRequest>()).Returns(ret);

            //Act
            controller.Action(data, session, actor);
            //Assert
            actor.Received().Tell(Arg.Is<ResponseMessageOfTheDay>(x=>x.Message == ret.Message));
        }
    }
}
