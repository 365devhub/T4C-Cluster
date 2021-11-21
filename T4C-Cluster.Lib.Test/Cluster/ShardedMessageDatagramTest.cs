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
    public class ShardedMessageDatagramTest
    {
        [Test]
        public void ShardedMessageDatagram_Contructeur()
        {
            //Arrange
            Fixture fixture = new Fixture();
            byte[] a = fixture.CreateMany<byte>().ToArray();
            //Act
            var msg = new ShardedMessageDatagram(a,"127.0.0.1:11677");
            //Assert
            Assert.AreEqual(a,msg.Datas);
        }
    }
}
