using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4c_Cluster.Node.Worker.Attributes;

namespace T4c_Cluster.Node.Worker.Test.Attribute
{
    [TestFixture]
    public class ValidatePlayerAuthenticatedAttributeTest
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ValidatePlayerAuthenticatedAttribute_Validate(bool isAuthentificated)
        {
            //Arrage
            ValidatePlayerAuthenticatedAttribute att = new ValidatePlayerAuthenticatedAttribute();
            //Act
            var result = att.Validate(new Sessions.PlayerActor.PlayerSession() { IsAuthenticated = isAuthentificated });
            //Assert
            Assert.AreEqual(isAuthentificated, result);
        }
    }
}
