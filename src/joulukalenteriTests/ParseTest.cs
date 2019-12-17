using System;
using Xunit;
using joulukalenteri.Shared;
using joulukalenteri.Client.SharedCode;
using Moq;
using System.Threading.Tasks;
using System.Linq;

namespace joulukalenteriTests
{
    public class ParseTest
    {
        [Theory]
        [InlineData(1, "# asdf\n", "asdf", "", "")]
        [InlineData(1, "notitle\n\nasdf", "Day 1", "notitle", "<p>notitle</p>\n<p>asdf</p>\n")]
        [InlineData(1, "aaa\n# asdf\n", "asdf", "aaa", "<p>aaa</p>\n")]
        [InlineData(1, "aaa\n# asdf\nghi\n", "asdf", "aaa", "<p>aaa</p>\n<p>ghi</p>\n")]
        [MemberData(nameof(TestData))]
        public async Task DataParserTest(int day, string input, string title, string summary, string content) {
            var receiverMock = new Mock<IDataReceiver>(MockBehavior.Strict);
            receiverMock.Setup(receiver => receiver.ReceiveDayData(It.IsAny<int>(), day, It.IsAny<string>())).ReturnsAsync(input);
            var dayMock = new Mock<IDateTime>();
            dayMock.Setup(day => day.Now).Returns(DateTime.Today);
            DayReader reader = new DayReader(receiverMock.Object, dayMock.Object);
            DayInfoData data = (await reader.GetContent(day, It.IsAny<string>()));
            string parseResult = (await reader.GetContent(day, It.IsAny<string>()))?.Title;
            //Assert.True(true);
            Assert.Equal(title, data.Title);
            Assert.Equal(summary, data.Summary);
            Assert.Equal(content, data.Content);
        }
        public static TheoryData<int, string, string, string, string> TestData() {
            string repeat = string.Concat(Enumerable.Repeat("a",DayInfoData.SummaryLength));
            string content = repeat + "asdfg";
            TheoryData<int, string, string, string, string> data =  new TheoryData<int, string, string, string, string>();
            data.Add(1, $"{content}\n# mytitle", "mytitle", repeat + "...", $"<p>{content}</p>\n");
            return data;
        }
    }
}
