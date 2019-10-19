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
    }
}
