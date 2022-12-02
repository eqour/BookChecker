using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;

namespace LinkCheckerLib.Tests
{
    [TestClass]
    public class WebWorkerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetStatusCode_NullString_ThrowArgumentNullException()
        {
            // Arrange
            string url = null;

            // Act
            WebWorker.GetStatusCode(url);
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void GetStatusCode_EmptyString_ThrowUriFormatException()
        {
            // Arrange
            string url = "";

            // Act
            WebWorker.GetStatusCode(url);
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void GetStatusCode_WorkingUrlWithoutProtocol_ThrowUriFormatException()
        {
            // Arrange
            string url = "google.com";

            // Act
            WebWorker.GetStatusCode(url);
        }

        [TestMethod]
        public void GetStatusCode_HttpsWorkingUrl_ReturnsCode200()
        {
            // Arrange
            HttpStatusCode actual;
            HttpStatusCode expected = HttpStatusCode.OK;
            string url = "https://google.com";

            // Act
            actual = WebWorker.GetStatusCode(url);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetStatusCode_HttpWorkingUrl_ReturnsCode200()
        {
            // Arrange
            HttpStatusCode actual;
            HttpStatusCode expected = HttpStatusCode.OK;
            string url = "http://google.com";

            // Act
            actual = WebWorker.GetStatusCode(url);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetStatusCode_HttpsUrlToEmptyPage_ReturnsCode404()
        {
            // Arrange
            HttpStatusCode actual;
            HttpStatusCode expected = HttpStatusCode.NotFound;
            string url = "https://www.google.ru/drive/e4tgeugb34";

            // Act
            actual = WebWorker.GetStatusCode(url);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetStatusCode_HttpsUrlToNotExistsPage_ReturnsCode404()
        {
            // Arrange
            HttpStatusCode actual;
            HttpStatusCode expected = HttpStatusCode.NotFound;
            string url = "https://yguiw64hcnz9n6f735n/";

            // Act
            actual = WebWorker.GetStatusCode(url);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        // ---------------------------------------------------------------------------------------------------

        [TestMethod]
        public void GetResponse_NullString_ReturnsEmptyString()
        {
            // Arrange
            string url = null;

            // Act
            string actual = WebWorker.GetResponse(url);

            // Assert
            Assert.AreEqual(actual, "");
        }

        [TestMethod]
        public void GetResponse_EmptyString_ReturnsEmptyString()
        {
            // Arrange
            string url = string.Empty;

            // Act
            string actual = WebWorker.GetResponse(url);

            // Assert
            Assert.AreEqual(actual, "");
        }

        [TestMethod]
        public void GetResponse_UncorrectUrl_ReturnsEmptyString()
        {
            // Arrange
            string url = "456hgrnf.org/456845tbdg7483]";

            // Act
            string actual = WebWorker.GetResponse(url);

            // Assert
            Assert.AreEqual(actual, "");
        }

        [TestMethod]
        public void GetResponse_CorrectUrl1_ReturnsPageTextLength153()
        {
            // Arrange
            string url = "https://egorb17.github.io";
            int exceptedLength = 153;

            // Act
            string actual = WebWorker.GetResponse(url);

            // Assert
            Assert.AreEqual(exceptedLength, actual.Length);
        }

        [TestMethod]
        public void GetResponse_CorrectUrl2_ReturnsPageTextLength1256()
        {
            // Arrange
            string url = "https://example.com";
            int exceptedLength = 1256;

            // Act
            string actual = WebWorker.GetResponse(url);

            // Assert
            Assert.AreEqual(exceptedLength, actual.Length);
        }
    }
}
