using Akka.Cluster.Sharding;
using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4C_Cluster.Lib.Cluster;

namespace T4C_Cluster.Lib.Test.Cluster
{
    [TestFixture]
    public class ShardingMessageExtractorTest
    {
        [Test]
        public void ShardingMessageExtractor_EntityId_WhenShardingEnvelope()
        {
            //Arrange
            Fixture fixture = new Fixture();
            ShardingMessageExtractor extractor = new ShardingMessageExtractor();
            ShardingEnvelope env = fixture.Create<ShardingEnvelope>();
            //Act
            var id = extractor.EntityId(env);
            //Assert
            Assert.AreEqual(env.EntityId, id);
        }

        [Test]
        public void ShardingMessageExtractor_EntityId_WhenStartEntity()
        {
            //Arrange
            Fixture fixture = new Fixture();
            ShardingMessageExtractor extractor = new ShardingMessageExtractor();
            ShardRegion.StartEntity env = fixture.Create<ShardRegion.StartEntity>();
            //Act
            var id = extractor.EntityId(env);
            //Assert
            Assert.AreEqual(env.EntityId, id);
        }

        [Test]
        public void ShardingMessageExtractor_EntityMessage_WhenShardingEnvelope()
        {
            //Arrange
            Fixture fixture = new Fixture();
            ShardingMessageExtractor extractor = new ShardingMessageExtractor();
            ShardingEnvelope env = fixture.Create<ShardingEnvelope>();
            //Act
            var msg = extractor.EntityMessage(env);
            //Assert
            Assert.AreEqual(env.Message, msg);
        }

        [Test]
        public void ShardingMessageExtractor_EntityMessage_WhenNotShardingEnvelope()
        {
            //Arrange
            Fixture fixture = new Fixture();
            ShardingMessageExtractor extractor = new ShardingMessageExtractor();
            string env = fixture.Create<string>();
            //Act
            var msg = extractor.EntityMessage(env);
            //Assert
            Assert.AreEqual(env, msg);
        }

    }
}
