using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkCheckerLib.Tests
{
    [TestClass]
    public class LinkCheckerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckLink_NullString_ThrowsArgumentNullException()
        {
            // Arrange
            string link = null;

            // Act
            LinkChecker.CheckLink(link);
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void CheckLink_EmptyString_ThrowsUriFormatException()
        {
            // Arrange
            string link = "";

            // Act
            LinkChecker.CheckLink(link);
        }

        [TestMethod]
        public void CheckLink_CorrectLink1_ReturnsFilledLinkInfo()
        {
            // Arrange
            string link = "https://example.com";
            LinkInfo excepted = new LinkInfo("https://example.com", 200, Array.Empty<string>());

            // Act
            LinkInfo actual = LinkChecker.CheckLink(link);

            // Assert
            Assert.AreEqual(excepted.FoundErrorMessageMatches.Length, actual.FoundErrorMessageMatches.Length);
            Assert.AreEqual(excepted.Link, actual.Link);
            Assert.AreEqual(excepted.ResponseCode, actual.ResponseCode);
        }

        [TestMethod]
        public void CheckLink_UncorrectLink1_ReturnsFilledLinkInfoWith404Code()
        {
            // Arrange
            string link = "https://www.google.ru/drive/e4tgeugb34";
            LinkInfo excepted = new LinkInfo("https://www.google.ru/drive/e4tgeugb34", 404, null);

            // Act
            LinkInfo actual = LinkChecker.CheckLink(link);

            // Assert
            Assert.AreEqual(excepted.FoundErrorMessageMatches, actual.FoundErrorMessageMatches);
            Assert.AreEqual(excepted.Link, actual.Link);
            Assert.AreEqual(excepted.ResponseCode, actual.ResponseCode);
        }

        [TestMethod]
        public void CheckLink_UncorrectLink2_ReturnsFilledLinkInfoWithFoundOneErrorMessage()
        {
            // Arrange
            string link = "https://egorb17.github.io/";
            LinkInfo excepted = new LinkInfo("https://egorb17.github.io/", 200, new string[] { "hello" });
            TextParser.MatchFinder.SearchPatterns = new string[] { "hello" };

            // Act
            LinkInfo actual = LinkChecker.CheckLink(link);

            // Assert
            Assert.AreEqual(excepted.FoundErrorMessageMatches.Length, actual.FoundErrorMessageMatches.Length);
            Assert.AreEqual(excepted.FoundErrorMessageMatches[0], actual.FoundErrorMessageMatches[0]);
            Assert.AreEqual(excepted.Link, actual.Link);
            Assert.AreEqual(excepted.ResponseCode, actual.ResponseCode);
        }

        [TestMethod]
        public void CheckLink_UncorrectLink3_ReturnsFilledLinkInfoWithFoundTwoErrorMessages()
        {
            // Arrange
            string link = "https://egorb17.github.io/";
            LinkInfo excepted = new LinkInfo("https://egorb17.github.io/", 200, new string[] { "hello", "!!!" });
            TextParser.MatchFinder.SearchPatterns = new string[] { "hello", "!!!" };

            // Act
            LinkInfo actual = LinkChecker.CheckLink(link);

            // Assert
            Assert.AreEqual(excepted.FoundErrorMessageMatches.Length, actual.FoundErrorMessageMatches.Length);
            Assert.AreEqual(excepted.FoundErrorMessageMatches[0], actual.FoundErrorMessageMatches[0]);
            Assert.AreEqual(excepted.Link, actual.Link);
            Assert.AreEqual(excepted.ResponseCode, actual.ResponseCode);
        }
    }
}
