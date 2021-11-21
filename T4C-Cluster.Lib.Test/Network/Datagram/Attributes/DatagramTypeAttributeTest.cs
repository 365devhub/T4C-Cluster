using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Test.Network.Datagram.Attributes
{
    [TestFixture]
    class DatagramTypeAttributeTest
    {
        [Test]
        public void DatagramTypeAttribute_Constructor([Values] DatagramTypeEnum datagramType, [Values]DatagramDirectionEnum dir, [Values]bool needAck)
        {
            //Arrange,Act
            DatagramTypeAttribute t = new DatagramTypeAttribute(datagramType, dir, needAck);
            //Assert
            Assert.AreEqual(datagramType, t.DatagramType);
            Assert.AreEqual(dir, t.Direction);
            Assert.AreEqual(needAck, t.NeedAck);
        }

        [Test]
        public void DataTypeAttribute_AttributeUsage()
        {
            //Arrange
            //Act
            AttributeUsageAttribute usage = typeof(DatagramTypeAttribute).GetCustomAttributes(true).OfType<AttributeUsageAttribute>().Single();
            //Assert
            Assert.AreEqual(AttributeTargets.Class, usage.ValidOn);
        }
    }
}
