using System.Collections.Generic;
using joulukalenteri.Server.Controllers;
using joulukalenteri.Client.SharedCode;
using Xunit;
using SystemWrapper;
using SystemWrapper.IO;
using Moq;
using joulukalenteri.Server;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace joulukalenteriTests
{
    public class ArchiveTest
    {
        [Fact]
        public void ServerSideTest()
        {
            Dictionary<string, IEnumerable<string>> entries = new Dictionary<string, IEnumerable<string>>()
            {
                {AppConfig.__dirpath + "1991", new string[] { "adsf.md", "test.md", "day1.txt", "day5.md", "day32.md" }},
                {AppConfig.__dirpath + "132f", new string[] { "day5.md", "day7.md", "day9.txt", "day24.md", "dummy.txt" }},
                {AppConfig.__dirpath + "2015", new string[] { "day5.md", "day7.md", "day9.txt", "day45.md", "day24.txt", "day25.md" }},
                {AppConfig.__dirpath + "2020", new string[] { "day5.md", "day7.md", "day9.txt", "day45.md", "day24.txt", "day25.md" }},
                {AppConfig.__dirpath + "2345", new string[] { "day5.md", "day7.md", "day9.txt", "day45.md", "day24.txt", "day25.md" }},
            };

            var dirmock = new Mock<IDirectoryWrap>();
            dirmock.Setup(dir => dir.Exists(It.IsAny<string>())).Returns(true);
            dirmock.Setup(dir => dir.GetDirectories(It.IsAny<string>())).Returns(entries.Keys.ToArray());

            foreach (var entry in entries) {
                dirmock.Setup(dir => dir.GetFiles(entry.Key))
                    .Returns(entry.Value.ToArray());
            }

            var daymock = new Mock<IDateTimeWrap>();
            daymock.Setup(day => day.Today.Year).Returns(2020);

            ArchiveCheckController controller = new ArchiveCheckController(dirmock.Object, daymock.Object);
            string dataJson = controller.GetArchive();
            var deserialized = JsonConvert.DeserializeObject<Dictionary<int, IEnumerable<string>>>(dataJson);
            Assert.Equal(new int []{ 1991,2015}, deserialized.Keys.ToArray());
            Assert.Equal(new string []{ "day5.md", "day7.md", "day25.md"}, deserialized[2015]);
        }
        [Fact]
        public async Task ClientSideTest()
        {
            Dictionary<int, IEnumerable<string>> entries = new Dictionary<int, IEnumerable<string>>()
            {
                {2000, new string[] { "day1.md", "day5.md", "day24.md" }},
                {2015, new string[] { "day5.md", "day7.md", "day9.md", "day24.md" }},
            };

            string raw = JsonConvert.SerializeObject(entries);
            var ReceiverMock = new Mock<IDataReceiver>(MockBehavior.Strict);
            ReceiverMock.Setup(receiver => receiver.ReceiveArchive(It.IsAny<string>())).ReturnsAsync(raw);
            ArchiveReader reader = new ArchiveReader(ReceiverMock.Object);

            IEnumerable<int> years = await reader.GetYears("whatever");
            IEnumerable<int> days = await reader.GetDays(2015, "meh");

            Assert.Equal(new int[] { 2000, 2015 }, years);
            Assert.Equal(new int[] { 5, 7, 9, 24 }, days);
        }
    }
}
