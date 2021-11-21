using AutoFixture;
using AutoFixture.Kernel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4C_Cluster.Lib.Network.Datagram.Attributes;

namespace T4C_Cluster.Lib.Test.Network.Datagram.Attributes
{
    [TestFixture]
    public class DataTypeAttributeTest
    {

        [Test]
        [TestCase(typeof(DataTypeArray16Attribute))]
        [TestCase(typeof(DataTypeArray8Attribute))]
        [TestCase(typeof(DataTypeBoolAttribute))]
        [TestCase(typeof(DataTypeInt16Attribute))]
        [TestCase(typeof(DataTypeInt32Attribute))]
        [TestCase(typeof(DataTypeInt64Attribute))]
        [TestCase(typeof(DataTypeInt8Attribute))]
        [TestCase(typeof(DataTypeString16Attribute))]
        [TestCase(typeof(DataTypeString8Attribute))]
        [TestCase(typeof(DataTypeUInt16Attribute))]
        [TestCase(typeof(DataTypeUInt32Attribute))]
        [TestCase(typeof(DataTypeUInt64Attribute))]
        public void DataTypeAttribute_Constructor(Type attrType) 
        {
            //Arrange
            Fixture fixture = new Fixture();
            var param = fixture.Create(attrType.GetConstructors()[0].GetParameters()[0], new SpecimenContext(fixture));
            //Act
            var attr = Activator.CreateInstance(attrType, param);
            //Assert
            Assert.AreEqual(param,attrType.GetProperty("Index").GetValue(attr));
        }

        [Test]
        [TestCase(typeof(DataTypeArray16Attribute))]
        [TestCase(typeof(DataTypeArray8Attribute))]
        [TestCase(typeof(DataTypeBoolAttribute))]
        [TestCase(typeof(DataTypeInt16Attribute))]
        [TestCase(typeof(DataTypeInt32Attribute))]
        [TestCase(typeof(DataTypeInt64Attribute))]
        [TestCase(typeof(DataTypeInt8Attribute))]
        [TestCase(typeof(DataTypeString16Attribute))]
        [TestCase(typeof(DataTypeString8Attribute))]
        [TestCase(typeof(DataTypeUInt16Attribute))]
        [TestCase(typeof(DataTypeUInt32Attribute))]
        [TestCase(typeof(DataTypeUInt64Attribute))]
        public void DataTypeAttribute_AttributeUsage(Type attrType)
        {
            //Arrange
            //Act
            AttributeUsageAttribute usage = attrType.GetCustomAttributes(true).OfType<AttributeUsageAttribute>().Single();
            //Assert
            Assert.AreEqual(AttributeTargets.Field | AttributeTargets.Property , usage.ValidOn);
        }
    }
}
