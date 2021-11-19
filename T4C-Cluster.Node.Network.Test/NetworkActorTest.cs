using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.TestKit;
using Akka.Actor;
using Akka.Actor.Setup;
using Akka.Configuration;
using Akka.TestKit.NUnit;
using Microsoft.Extensions.Options;
using Akka.IO;
using Akka.Cluster.Sharding;
using T4C_Cluster.Lib.Cluster;
using System.Net;
using NSubstitute;

namespace T4C_Cluster.Node.Network.Test
{
    [TestFixture]
    public class NetworkActorTest : TestKit
    {
        [Test]
        public void NetworkActor_Constructor_OK()
        {
            //Arrange
            var opt = Options.Create<Configuration.NetworkConfiguration>(new Configuration.NetworkConfiguration() { Address = "127.0.0.1", Port = 11677 });
            var mock = Substitute.For<IActorRef>();
            //Act
            try
            {
                var actor = Sys.ActorOf(Props.Create(() => new NetworkActor(opt, mock)));
                
            }
            catch(Exception e)
            {
                //Assert
                Assert.Fail(e.Message);
            }

        }

        [Test]
        public void NetworkActor_OnUdpReceive_RedirectToShards()
        {

            //Arrange
            var opt = Options.Create<Configuration.NetworkConfiguration>(new Configuration.NetworkConfiguration() { Address = "127.0.0.1", Port = 11677 });
            var mock = Substitute.For<IActorRef>();


            var actor = Sys.ActorOf(Props.Create(() => new NetworkActor(opt, mock)));
            //Act
            actor.Tell(new Udp.Received(ByteString.Empty,IPEndPoint.Parse("127.0.0.1:11677")));
            //Assert
            base.AwaitAssert(() => mock.Received().Tell(Arg.Any<ShardingEnvelope>(), Arg.Any<IActorRef>()));
           
        }
    }
}
