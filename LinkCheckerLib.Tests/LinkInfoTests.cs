using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkCheckerLib.Tests
{
    [TestClass]
    public class LinkInfoTests
    {
        [TestMethod]
        public void ConstructorWithoutParameters_ReturnsAllNullProperties()
        {
            // Act
            LinkInfo actual = new LinkInfo();

            // Assert
            Assert.AreEqual(null, actual.Link);
            Assert.AreEqual(0, actual.ResponseCode);
            Assert.AreEqual(null, actual.FoundErrorMessageMatches);
        }

        [TestMethod]
        public void ConstructorWitgParameters_ReturnsFillProperties()
        {
            // Arrange
            string link = "https://vk.com";
            int responseCode = 200;
            string[] foundErrorMessageMatches = new string[] { "match1", "match2" };

            // Act
            LinkInfo actual = new LinkInfo(link, responseCode, foundErrorMessageMatches);

            // Assert
            Assert.AreEqual(link, actual.Link);
            Assert.AreEqual(responseCode, actual.ResponseCode);
            Assert.AreEqual(foundErrorMessageMatches, actual.FoundErrorMessageMatches);
        }

        [TestMethod]
        public void ConstructorWithProperties_ReturnsFillProperties()
        {
            // Arrange
            string link = "https://vk.com";
            int responseCode = 200;
            string[] foundErrorMessageMatches = new string[] { "match1", "match2" };

            // Act
            LinkInfo actual = new LinkInfo { Link = link, ResponseCode = responseCode, FoundErrorMessageMatches = foundErrorMessageMatches };

            // Assert
            Assert.AreEqual(link, actual.Link);
            Assert.AreEqual(responseCode, actual.ResponseCode);
            Assert.AreEqual(foundErrorMessageMatches, actual.FoundErrorMessageMatches);
        }
    }
}
