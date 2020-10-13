using Xunit;
using AdventCalendar.Services;
using Moq;
using System.Threading.Tasks;
using System.Linq;
using AdventCalendar.Models;
using AdventCalendar.Settings;

namespace AdventCalendarests
{
    public class ParseTest
    {
        [Theory]
        [InlineData("# asdf\n", "asdf", "", "")]
        [InlineData("notitle\n\nasdf", "Day 1", "notitle", "<p>notitle</p>\n<p>asdf</p>\n")]
        [InlineData("aaa\n# asdf\n", "asdf", "aaa", "<p>aaa</p>\n")]
        [InlineData("aaa\n# asdf\nghi\n", "asdf", "aaa", "<p>aaa</p>\n<p>ghi</p>\n")]
        [MemberData(nameof(TestData))]
        public async Task DataParserTest(string input, string title, string summary, string content) {
            var receiverMock = new Mock<IDataReceiver>();
            receiverMock.Setup(receiver => receiver.CheckDayData(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);
            receiverMock.Setup(receiver => receiver.ReceiveDayData(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(input);
            var setting = Mock.Of<IAppSettings>(set => set.SummaryLength == 80);

            var dateTime = new DefaultDateTime();

            DayReader reader = new DayReader(receiverMock.Object, dateTime, setting);
            DayInfoData data = (await reader.GetContent(1).ConfigureAwait(false));
            string parseResult = (await reader.GetContent(1).ConfigureAwait(false))?.Title;
            Assert.Equal(title, data.Title);
            Assert.Equal(summary, data.Summary);
            Assert.Equal(content, data.Content);
        }
        public static TheoryData<string, string, string, string> TestData() {
            string repeat = string.Concat(Enumerable.Repeat("a",DayInfoData.SummaryLength));
            string content = repeat + "asdfg";
            TheoryData<string, string, string, string> data = new TheoryData<string, string, string, string>
            {
                { $"{content}\n# mytitle", "mytitle", repeat + "...", $"<p>{content}</p>\n" }
            };
            return data;
        }
    }
}
