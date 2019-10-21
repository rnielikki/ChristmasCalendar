using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using joulukalenteri.Server.Controllers;
using joulukalenteri.Shared;
using joulukalenteri.Client.SharedCode;
using System.Net.Http;
using Moq;
using Moq.Protected;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using SystemWrapper.IO;

namespace joulukalenteriTests
{
    public class ReadTest
    {
        [Fact]
        public void ServerReadTest() {
            var filemock = new Mock<IFileWrap>(MockBehavior.Strict);
            string testText = "Lorem ipsum dolor sit amet";
            filemock.Setup(fread => fread.ReadAllText(It.IsAny<string>())).Returns(testText);
            filemock.Setup(fread => fread.Exists(It.IsAny<string>())).Returns(true);
            DayReaderController controller = new DayReaderController(filemock.Object);
            var result = controller.Get(1);
            Assert.Equal(testText, result);
            var failedResult1 = controller.Get(0);
            Assert.NotEqual(testText, failedResult1);
            var failedResult2 = controller.Get(26);
            Assert.NotEqual(testText, failedResult2);
        }
    }
}
