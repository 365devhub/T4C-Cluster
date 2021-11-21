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
    public class DatagramBodyTest
    {
        [Test]
        public void DatagramBody_()
        {
            //Arrange
            Fixture fixture = new Fixture();
            DatagramBody body = new DatagramBody();

            var a1 = fixture.Create<bool>();
            var a2 = fixture.Create<byte>();
            var a3 = fixture.Create<Int16>();
            var a4 = fixture.Create<Int32>();
            var a5 = fixture.Create<Int64>();
            var a6 = fixture.Create<string>();
            var a7 = fixture.Create<string>();
            var a8 = fixture.Create<UInt16>();
            var a9 = fixture.Create<UInt32>();
            var a10 = fixture.Create<UInt64>();


            //Act
            body.WriteBool(a1);
            body.WriteByte(a2);
            body.WriteInt16(a3);
            body.WriteInt32(a4);
            body.WriteInt64(a5);
            body.WriteString16(a6);
            body.WriteString8(a7);
            body.WriteUInt16(a8);
            body.WriteUInt32(a9);
            body.WriteUInt64(a10);

            body.SetCursorPosition(0);
            //Assert
            Assert.AreEqual(a1, body.ReadBool());
            Assert.AreEqual(a2, body.ReadByte());
            Assert.AreEqual(a3, body.ReadInt16());
            Assert.AreEqual(a4, body.ReadInt32());
            Assert.AreEqual(a5, body.ReadInt64());
            Assert.AreEqual(a6, body.ReadString16());
            Assert.AreEqual(a7, body.ReadString8());
            Assert.AreEqual(a8, body.ReadUInt16());
            Assert.AreEqual(a9, body.ReadUInt32());
            Assert.AreEqual(a10, body.ReadUInt64());

            Assert.IsNull(body.ReadBool());
            Assert.IsNull(body.ReadByte());
            Assert.IsNull(body.ReadInt16());
            Assert.IsNull(body.ReadInt32());
            Assert.IsNull(body.ReadInt64());
            Assert.IsNull(body.ReadString16());
            Assert.IsNull(body.ReadString8());
            Assert.IsNull(body.ReadUInt16());
            Assert.IsNull(body.ReadUInt32());
            Assert.IsNull(body.ReadUInt64());

        }
    }
}
