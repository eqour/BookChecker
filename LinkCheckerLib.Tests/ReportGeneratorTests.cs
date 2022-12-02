using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LinkCheckerLib.Tests
{
    [TestClass]
    public class ReportGeneratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateReportBasedOnLinkInfo_NullArgument_ThrowsArgumentNullException()
        {
            // Arrange
            LinkInfo linkInfo = null;

            // Act
            ReportGenerator.GenerateReportBasedOnLinkInfo(linkInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateReportBasedOnLinkInfo_EmptyLinkInfo_ThrowsArgumentNullException()
        {
            // Arrange
            LinkInfo linkInfo = new LinkInfo();

            // Act
            ReportGenerator.GenerateReportBasedOnLinkInfo(linkInfo);
        }

        [TestMethod]
        public void GenerateReportBasedOnLinkInfo_OkLink_ReturnsOkString()
        {
            // Arrange
            LinkInfo linkInfo = new LinkInfo("https://google.com", 200, Array.Empty<string>());
            string excepted = ReportGenerator.GoodMsgPrefix;

            // Act
            string actual = ReportGenerator.GenerateReportBasedOnLinkInfo(linkInfo);

            // Assert
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void GenerateReportBasedOnLinkInfo_DoubtLink_ReturnsDoubtString()
        {
            // Arrange
            LinkInfo linkInfo = new LinkInfo("https://google.com", 200, new string[] { "error", "not found" });
            string excepted = ReportGenerator.DoubtfulMsgPrefix + "error, not found";

            // Act
            string actual = ReportGenerator.GenerateReportBasedOnLinkInfo(linkInfo);

            // Assert
            Assert.AreEqual(excepted, actual);
        }

        [TestMethod]
        public void GenerateReportBasedOnLinkInfo_Badink_ReturnsNoString()
        {
            // Arrange
            LinkInfo linkInfo = new LinkInfo("https://google.com", 404, Array.Empty<string>());
            string excepted = ReportGenerator.BadMsgPrefix + "404";

            // Act
            string actual = ReportGenerator.GenerateReportBasedOnLinkInfo(linkInfo);

            // Assert
            Assert.AreEqual(excepted, actual);
        }

        // ----------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateReportBasedOnLinkInfos_NullArgument_ThrowsArgumentNullException()
        {
            // Arrange
            LinkInfo[] linkInfo = null;

            // Act
            ReportGenerator.GenerateReportBasedOnLinkInfo(linkInfo);
        }

        [TestMethod]
        public void GenerateReportBasedOnLinkInfos_OkDoubtOkBadDoubt_ReturnsNotNullString()
        {
            // Arrange
            LinkInfo[] linkInfo = new LinkInfo[]
            {
                new LinkInfo("https://google.com", 200, Array.Empty<string>()),
                new LinkInfo("https://google.com", 200, new string[] { "strange text" }),
                new LinkInfo("https://google.com", 200, Array.Empty<string>()),
                new LinkInfo("https://google.com", 404, Array.Empty<string>()),
                new LinkInfo("https://yandex.ru", 200, new string[] { "not found text" }),
            };

            // Act
            string actual = ReportGenerator.GenerateReportBasedOnLinkInfo(linkInfo);

            // Assert
            Assert.IsNotNull(actual);
        }
    }
}
