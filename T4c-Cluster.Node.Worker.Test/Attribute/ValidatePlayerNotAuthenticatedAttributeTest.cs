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
    public class ValidatePlayerNotAuthenticatedAttributeTest
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ValidatePlayerNotAuthenticatedAttribute_Validate(bool isAuthentificated)
        {
            //Arrage
            ValidatePlayerNotAuthenticatedAttribute att = new ValidatePlayerNotAuthenticatedAttribute();
            //Act
            var result = att.Validate(new Sessions.PlayerActor.PlayerSession() { IsAuthenticated = isAuthentificated });
            //Assert
            Assert.AreEqual(!isAuthentificated, result);
        }
    }
}
