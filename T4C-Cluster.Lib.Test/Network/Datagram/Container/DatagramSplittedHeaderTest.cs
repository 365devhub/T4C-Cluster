using AutoFixture;
using AutoFixture.Kernel;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4C_Cluster.Lib.Network.Datagram.Container;

namespace T4C_Cluster.Lib.Test.Network.Datagram.Container
{
    [TestFixture]
    public class DatagramSplittedHeaderTest
    {

        [Test]
        public void DatagramSplittedHeader_Constructor()
        {

            //Arrange
            Fixture fixture = new Fixture();
            var request1 = new RangedNumberRequest(typeof(byte), 0, 63);
            var request2 = new RangedNumberRequest(typeof(byte), 0, 31);
            var request3 = new RangedNumberRequest(typeof(byte), 0, 31);
            var request4 = new RangedNumberRequest(typeof(ushort), 0, 65535);
            RandomRangedNumberGenerator generator = new RandomRangedNumberGenerator();
            
            byte SplitedGroupId = (byte)generator.Create(request1, new SpecimenContext(fixture));
            byte SplitedPartNumber = (byte)generator.Create(request2, new SpecimenContext(fixture));
            byte SplitedTotalPartNumber = (byte)generator.Create(request3, new SpecimenContext(fixture)); ;
            ushort SplitedTotalDataSize = (ushort)generator.Create(request4, new SpecimenContext(fixture)); ;

            DatagramSplittedHeader d = new DatagramSplittedHeader();

            //Act
            d.SplitedGroupId = SplitedGroupId;
            d.SplitedPartNumber = SplitedPartNumber;
            d.SplitedTotalDataSize = SplitedTotalDataSize;
            d.SplitedTotalPartNumber = SplitedTotalPartNumber;
            DatagramSplittedHeader e = new DatagramSplittedHeader( d.GenerateBuffer());

            //Assert
            Assert.AreEqual(SplitedGroupId, e.SplitedGroupId);
            Assert.AreEqual(SplitedPartNumber, e.SplitedPartNumber);
            Assert.AreEqual(SplitedTotalPartNumber, e.SplitedTotalPartNumber);
            Assert.AreEqual(SplitedTotalDataSize, e.SplitedTotalDataSize);
        }
    }
}
