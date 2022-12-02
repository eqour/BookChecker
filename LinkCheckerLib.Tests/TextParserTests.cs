using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LinkCheckerLib.Tests
{
    [TestClass]
    public class TextParserTests
    {
        private const string Pattern = @"((http|https):\/\/|)[A-z0-9,.\\\/:;?<>!@#$%^&*()\-+=_]{0,}(\.ru|\.com|\.net)[A-z0-9,.\\\/:;?<>!@#$%^&*()\-+=_]{0,}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindLinks_NullString_ThrowArgumentNullException()
        {
            // Arrange
            string text = null;

            // Act
            TextParser.LinkFinder.FindLinks(text);
        }

        [TestMethod]
        public void FindLinks_EmptyString_ReturnsEmptyArray()
        {
            // Arrange
            string text = "";

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void FindLinks_StringWithoutLinks_ReturnsEmptyArray()
        {
            // Arrange
            string text = "Some text .NET framework .NET Core 5.0 another text end text";

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            Assert.AreEqual(null, actual);
        }

        [TestMethod]
        public void FindLinks_SimpleLink_ReturnsLink()
        {
            // Arrange
            string text = "https://example.com/";
            string[] expected = { "https://example.com/" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkInStartString_ReturnsLink()
        {
            // Arrange
            string text = "https://example.com/ some text";
            string[] expected = { "https://example.com/" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkInEndString_ReturnsLink()
        {
            // Arrange
            string text = "some text https://example.com/";
            string[] expected = { "https://example.com/" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkInCenterString_ReturnsLink()
        {
            // Arrange
            string text = "some text https://example.com/ some text";
            string[] expected = { "https://example.com/" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkInCenterStringWithDelimeters1_ReturnsLink()
        {
            // Arrange
            string text = "some text (https://example.com/) some text";
            string[] expected = { "https://example.com/" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkInCenterStringWithDelimeters2_ReturnsLink()
        {
            // Arrange
            string text = "some text ..<;?>(https://example.com/)][.,()[]{},.!?<>;: some text";
            string[] expected = { "https://example.com/" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkHttp_ReturnsLink()
        {
            // Arrange
            string text = "http://example.com/";
            string[] expected = { "http://example.com/" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkWithoutProtocol1_ReturnsLinkWithProtocol()
        {
            // Arrange
            string text = "example.com/";
            string[] expected = { "http://example.com/" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_ThreeLinksInText_ReturnsThreeLinks()
        {
            // Arrange
            string text = "[example.com some text, http://example.com/]. Some text, some text, просто текст: {https://example.com.} .";
            string[] expected = { "http://example.com", "http://example.com/", "https://example.com" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_RealLinks_ReturnsLinks()
        {
            // Arrange
            string text =
                "https://go.microsoft.com/fwlink/?LinkId=2134209&0x419; " +
                "https://docs.microsoft.com/ru-ru/dotnet/csharp/; " +
                "https://skillbox.ru/media/code/kak_pisat_tekhnicheskuyu_dokumentatsiyu/; " +
                "https://en.wikipedia.org/wiki/Wikipedia:About; ";

            string[] expected = 
            { 
                "https://go.microsoft.com/fwlink/?LinkId=2134209&0x419",
                "https://docs.microsoft.com/ru-ru/dotnet/csharp/",
                "https://skillbox.ru/media/code/kak_pisat_tekhnicheskuyu_dokumentatsiyu/",
                "https://en.wikipedia.org/wiki/Wikipedia:About"
            };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LongLink_ReturnsLinks()
        {
            // Arrange
            string text = "some text https://www.google.com/maps/place/%D0%A1%D0%BE%D0%B5%D0%B4%D0%B8%D0%BD%D0%B5%D0%BD%D0%BD%D1%8B%D0%B5+%D0%A8%D1%82%D0%B0%D1%82%D1%8B+%D0%90%D0%BC%D0%B5%D1%80%D0%B8%D0%BA%D0%B8/@31.7860603,-132.0853276,16099470m/data=!3m2!1e3!4b1!4m5!3m4!1s0x54eab584e432360b:0x1c3bb99243deb742!8m2!3d37.09024!4d-95.712891 some text";

            string[] expected = { "https://www.google.com/maps/place/%D0%A1%D0%BE%D0%B5%D0%B4%D0%B8%D0%BD%D0%B5%D0%BD%D0%BD%D1%8B%D0%B5+%D0%A8%D1%82%D0%B0%D1%82%D1%8B+%D0%90%D0%BC%D0%B5%D1%80%D0%B8%D0%BA%D0%B8/@31.7860603,-132.0853276,16099470m/data=!3m2!1e3!4b1!4m5!3m4!1s0x54eab584e432360b:0x1c3bb99243deb742!8m2!3d37.09024!4d-95.712891" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkWithoutProtocol2_ReturnsLinkWithProtocol()
        {
            // Arrange
            string text = "some text example.com/ some text";
            string[] expected = { "http://example.com/" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkWithoutProtocol3_ReturnsLinkWithProtocol()
        {
            // Arrange
            string text = "vk.com";
            string[] expected = { "http://vk.com" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkWithoutProtocol4_ReturnsLinkWithProtocol()
        {
            // Arrange
            string text = "vkk.com";
            string[] expected = { "http://vkk.com" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_LinkWithoutProtocol5_ReturnsLinkWithProtocol()
        {
            // Arrange
            string text = "vkkk.com";
            string[] expected = { "http://vkkk.com" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_SmallLink1_ReturnsLink()
        {
            // Arrange
            string text = "http://ya.ru";
            string[] expected = { "http://ya.ru" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void FindLinks_SmallLink2_ReturnsLink()
        {
            // Arrange
            string text = "https://ya.ru";
            string[] expected = { "https://ya.ru" };

            // Act
            string[] actual = TextParser.LinkFinder.FindLinks(text);

            // Assert
            for (int i = 0; i < actual.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        // -------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindMatches_NullArguments_ThrowsArgumentNullException()
        {
            // Arrange
            string text = null;
            string[] textForFind = null;

            // Act
            TextParser.MatchFinder.FindMatches(text, textForFind);
        }

        [TestMethod]
        public void FindMatches_EmptyTextForFind_ReturnsEmptyString()
        {
            // Arrange
            string text = "123 4 5 67 text!";
            string[] textForFind = Array.Empty<string>();

            // Act
            string[] actual = TextParser.MatchFinder.FindMatches(text, textForFind);

            // Assert
            Assert.AreEqual(0, actual.Length);
        }

        [TestMethod]
        public void FindMatches_TextAndTextForFindCompares1_ReturnsOneResult()
        {
            // Arrange
            string text = "123 4 5 67 text! Some text error! text text some text...";
            string[] textForFind = { "error" };

            // Act
            string[] actual = TextParser.MatchFinder.FindMatches(text, textForFind);

            // Assert
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("error", actual[0]);
        }

        [TestMethod]
        public void FindMatches_TextAndTextForFindCompares2_ReturnsOneResult()
        {
            // Arrange
            string text = "123 4 5 67 text! Some text error! text text some text...";
            string[] textForFind = { "warning", "bad", "error", "exception" };

            // Act
            string[] actual = TextParser.MatchFinder.FindMatches(text, textForFind);

            // Assert
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual("error", actual[0]);
        }
    }
}
