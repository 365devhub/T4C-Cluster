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
    public class ValidatePlayerInGameAttributeTest
    {
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ValidatePlayerInGameAttribute_Validate(bool isInGame)
        {
            //Arrage
            ValidatePlayerInGameAttribute att = new ValidatePlayerInGameAttribute();
            //Act
            var result = att.Validate(new Sessions.PlayerActor.PlayerSession() { IsInGame = isInGame });
            //Assert
            Assert.AreEqual(isInGame, result);
        }
    }
}
