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
    public class DatagramCompressHeaderTest
    {
        [Test]
        public void DatagramCompressHeader_ConstructorBytes()
        {
            //Arrange
            Fixture fixture = new Fixture();
            var compressedDataLenght = fixture.Create<uint>();
            var uncompressedDataLenght = fixture.Create<uint>();
            byte[] bytes = new byte[8];
            BitConverter.GetBytes(compressedDataLenght).CopyTo(bytes, 0);
            BitConverter.GetBytes(uncompressedDataLenght).CopyTo(bytes, 4);
            //Act
            DatagramCompressHeader h = new DatagramCompressHeader(bytes);
            //Assert
            Assert.AreEqual(compressedDataLenght, h.CompressedDataLenght);
            Assert.AreEqual(uncompressedDataLenght, h.UncompressedDataLenght);
        }

        [Test]
        public void DatagramCompressHeader_GenerateBuffer()
        {
            //Arrange
            Fixture fixture = new Fixture();
            var compressedDataLenght = fixture.Create<uint>();
            var uncompressedDataLenght = fixture.Create<uint>();

            DatagramCompressHeader h = new DatagramCompressHeader();
            h.CompressedDataLenght = compressedDataLenght;
            h.UncompressedDataLenght = uncompressedDataLenght;
            
            //Act
            DatagramCompressHeader h2 = new DatagramCompressHeader(h.GenerateBuffer());

            //Assert
            Assert.AreEqual(compressedDataLenght, h2.CompressedDataLenght);
            Assert.AreEqual(uncompressedDataLenght, h2.UncompressedDataLenght);
        }
    }
}
