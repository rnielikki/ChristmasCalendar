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
    public class ReadTest
    {
        [Fact]
        public void ServerDataTest(){
            DayReaderController readerController = new DayReaderController();
            string day1 = readerController.Get(1);
            Assert.Equal("asdf", day1);
            //Assert.Equal("My Title", day1.Title);
            //Assert.Equal("In A nutshell!", day1.Summary);
            //Assert.Equal("Here's some Content...", day1.Content);
        }
        ///<summary>
        ///This mocks "Handler", not client itself.
        ///HttpClient.GetStringAsync has no interface, so it cannot be mocked.
        ///but the Handler mock has some problem - the Client is sometimes disposed, which makes error, thus test fails.
        ///it's like a schrödinger cat test... even if not fifty-fifty.
        ///</summary>
        /*
        [Fact]
        public async Task ClientDataTest() {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               // Setup the PROTECTED method to mock
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               // prepare the expected response of the mocked http call
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent("asdf"),
               })
               .Verifiable();

            // use real http client with mocked handler here
            var httpClient = new HttpClient(handlerMock.Object);

            DayReader reader = new DayReader(httpClient);
            string daydata = await reader.GetContent(1, "http://localhost:49522/");
            Assert.Equal("asdf", daydata);
        }
        */
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
