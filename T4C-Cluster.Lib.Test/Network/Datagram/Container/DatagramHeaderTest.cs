using AutoFixture;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4C_Cluster.Lib.Network.Datagram.Container;

namespace T4C_Cluster.Lib.Test.Network.Datagram.Container
{
    [TestFixture]
    public class DatagramHeaderTest
    {

        [Test]
        public void DatagramHeader_Equality()
        {
            //Arrange
            Fixture fixture = new Fixture();
            DatagramHeader d = new DatagramHeader();
            d.Id = fixture.Create<ushort>();
            d.NeedAck = fixture.Create<bool>();
            d.PartNumber = fixture.Create<byte>();
            d.Splited = fixture.Create<bool>();
            d.Unknown = fixture.Create<bool>();
            d.Crc8 = fixture.Create<byte>();
            d.Compressed = fixture.Create<bool>();
            //Act
            DatagramHeader d2 = new DatagramHeader(d.GenerateBuffer());
            //Assert
            Assert.AreEqual(d.Id, d2.Id);
            Assert.AreEqual(d.NeedAck, d2.NeedAck);
            Assert.AreEqual(d.PartNumber, d2.PartNumber);
            Assert.AreEqual(d.Splited, d2.Splited);
            Assert.AreEqual(d.Unknown, d2.Unknown);
            Assert.AreEqual(d.Crc8, d2.Crc8);
            Assert.AreEqual(d.Compressed, d2.Compressed);
        }
    }
}
