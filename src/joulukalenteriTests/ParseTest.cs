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

namespace joulukalenteriTests
{
    public class ParseTest
    {
        [Theory]
        [InlineData(1, "# asdf\n", "asdf", "", "")]
        [InlineData(1, "notitle\n\nasdf", "Day 1", "notitle", "<p>notitle</p>\n<p>asdf</p>\n")]
        [InlineData(1, "aaa\n# asdf\n", "asdf", "aaa", "<p>aaa</p>\n")]
        [InlineData(1, "aaa\n# asdf\nghi\n", "asdf", "aaa", "<p>aaa</p>\n<p>ghi</p>\n")]
        [InlineData(1, "12345678901234567890123456789012345\n# asdf\n", "asdf", "123456789012345678901234567890...", "<p>12345678901234567890123456789012345</p>\n")]
        public async Task DataParserTest(int day, string input, string title, string summary, string content) {
            var ReceiverMock = new Mock<IDataReceiver>(MockBehavior.Strict);
            ReceiverMock.Setup(receiver => receiver.Generate(day, It.IsAny<string>())).ReturnsAsync(input);
            DayReader reader = new DayReader(ReceiverMock.Object);
            DayInfoData data = (await reader.GetContent(day, It.IsAny<string>()));
            string parseResult = (await reader.GetContent(day, It.IsAny<string>()))?.Title;
            //Assert.True(true);
            Assert.Equal(title, data.Title);
            Assert.Equal(summary, data.Summary);
            Assert.Equal(content, data.Content);
        }
    }
}
