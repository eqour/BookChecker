using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkCheckerLib.Tests
{
    [TestClass]
    public class DataHandlerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SelectLinksFromText_NullStringArray_ThrowsArgumentNullException()
        {
            // Arrange
            string[] textLines = null;

            // Act
            DataHandler.SelectLinksFromText(textLines);
        }

        [TestMethod]
        public void SelectLinksFromText_StringArrayWithoutLinks_ReturnsEmptyDictionary()
        {
            // Arrange
            string[] textLines = new string[] { "Some text some text", "Print your text here..." };

            // Act
            var actual = DataHandler.SelectLinksFromText(textLines);

            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void SelectLinksFromText_EmptyStringArray_ReturnsEmptyDictionary()
        {
            // Arrange
            string[] textLines = new string[] { };

            // Act
            var actual = DataHandler.SelectLinksFromText(textLines);

            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void SelectLinksFromText_StringArrayWithoutText_ReturnsEmptyDictionary()
        {
            // Arrange
            string[] textLines = new string[] { "" };

            // Act
            var actual = DataHandler.SelectLinksFromText(textLines);

            // Assert
            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void SelectLinksFromText_StringArrayWithRandomLinks1_ReturnsDictionaryWithLinks()
        {
            // Arrange
            string[] textLines = new string[] { "Some text", "Go to http://google.com", "Some text", "Search - https://yandex.ru and go to https://habr.com" };

            // Act
            var actual = DataHandler.SelectLinksFromText(textLines);

            // Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(1, actual[1].Length);
            Assert.AreEqual(2, actual[3].Length);
            Assert.AreEqual("http://google.com", actual[1][0]);
            Assert.AreEqual("https://yandex.ru", actual[3][0]);
            Assert.AreEqual("https://habr.com", actual[3][1]);
        }

        [TestMethod]
        public void SelectLinksFromText_StringArrayWithRandomLinks2_ReturnsDictionaryWithLinks()
        {
            // Arrange
            string[] textLines = new string[] { "link", "https://google.com, and https://vk.com, and https://clck.ru.", "Some text", "Search some text!" };

            // Act
            var actual = DataHandler.SelectLinksFromText(textLines);

            // Assert
            Assert.AreEqual(1, actual.Count);
            Assert.AreEqual(3, actual[1].Length);
            Assert.AreEqual("https://google.com", actual[1][0]);
            Assert.AreEqual("https://vk.com", actual[1][1]);
            Assert.AreEqual("https://clck.ru", actual[1][2]);
        }

        // --------------------------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HandleLinks_NullArgument_ThrowsArgumentNullException()
        {
            // Arrange
            Dictionary<int, string[]> data = null;

            // Act
            DataHandler.HandleLinks(data);
        }

        [TestMethod]
        public void HandleLinks_EmptyDictionary_ReturnsEmptyDictionary()
        {
            // Arrange
            Dictionary<int, string[]> data = new Dictionary<int, string[]>();
            Dictionary<int, LinkInfo> expected = new Dictionary<int, LinkInfo>();

            // Act
            var actual = DataHandler.HandleLinks(data);

            // Assert
            Assert.AreEqual(expected.Count, actual.Count);
        }

        [TestMethod]
        public void HandleLinks_DictionaryWithOneCorrectLink_ReturnsDictionaryWithCorrectInfo()
        {
            // Arrange
            Dictionary<int, string[]> data = new Dictionary<int, string[]> { { 0, new string[] { "https://example.com" } } };
            Dictionary<int, LinkInfo> expected = new Dictionary<int, LinkInfo> { { 0, new LinkInfo("https://example.com", 200, Array.Empty<string>()) } } ;

            // Act
            var actual = DataHandler.HandleLinks(data);

            // Assert
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected[0].Link, actual[0][0].Link);
            Assert.AreEqual(expected[0].ResponseCode, actual[0][0].ResponseCode);
            Assert.AreEqual(expected[0].FoundErrorMessageMatches.Length, actual[0][0].FoundErrorMessageMatches.Length);
        }

        [TestMethod]
        public void HandleLinks_DictionaryWithTwoCorrectLinksInOneRecord_ReturnsDictionaryWithCorrectInfo()
        {
            // Arrange
            Dictionary<int, string[]> data = new Dictionary<int, string[]>
            {
                { 0, new string[] { "https://example.com", "https://vk.com" } }
            };
            Dictionary<int, LinkInfo[]> expected = new Dictionary<int, LinkInfo[]>
            {
                {
                    0, new LinkInfo[]
                    {
                        new LinkInfo("https://example.com", 200, Array.Empty<string>()),
                        new LinkInfo("https://vk.com", 200, Array.Empty<string>())
                    }
                }
            };

            // Act
            var actual = DataHandler.HandleLinks(data);

            // Assert
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected[0][0].Link, actual[0][0].Link);
            Assert.AreEqual(expected[0][0].ResponseCode, actual[0][0].ResponseCode);
            Assert.AreEqual(expected[0][0].FoundErrorMessageMatches.Length, actual[0][0].FoundErrorMessageMatches.Length);
            Assert.AreEqual(expected[0][1].Link, actual[0][1].Link);
            Assert.AreEqual(expected[0][1].ResponseCode, actual[0][1].ResponseCode);
            Assert.AreEqual(expected[0][1].FoundErrorMessageMatches.Length, actual[0][1].FoundErrorMessageMatches.Length);
        }

        [TestMethod]
        public void HandleLinks_DictionaryWithOneCorrectAndOneUncorrectLinks_ReturnsDictionaryWithCorrectInfo()
        {
            // Arrange
            Dictionary<int, string[]> data = new Dictionary<int, string[]>
            { 
                { 0, new string[] { "https://example.com" } },
                { 3, new string[] { "https://www.google.ru/drive/e4tgeugb34" } }
            };
            Dictionary<int, LinkInfo> expected = new Dictionary<int, LinkInfo>
            {
                { 0, new LinkInfo("https://example.com", 200, Array.Empty<string>()) },
                { 3, new LinkInfo("https://www.google.ru/drive/e4tgeugb34", 404, Array.Empty<string>()) }
            };

            // Act
            var actual = DataHandler.HandleLinks(data);

            // Assert
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected[0].Link, actual[0][0].Link);
            Assert.AreEqual(expected[0].ResponseCode, actual[0][0].ResponseCode);
            Assert.AreEqual(expected[0].FoundErrorMessageMatches.Length, actual[0][0].FoundErrorMessageMatches.Length);
            Assert.AreEqual(expected[3].Link, actual[3][0].Link);
            Assert.AreEqual(expected[3].ResponseCode, actual[3][0].ResponseCode);
        }
    }
}
