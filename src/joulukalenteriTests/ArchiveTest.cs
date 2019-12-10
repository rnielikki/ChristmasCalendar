using joulukalenteri.Client.SharedCode;
using joulukalenteri.Server;
using joulukalenteri.Server.Controllers;
using joulukalenteri.Shared;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

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

            var nodata = new MockFileData("");
            IMockFileDataAccessor accessor = new MockFileSystem();

            foreach (var entry in entries)
            {
                string _key = entry.Key;
                foreach (var filename in entry.Value)
                {
                    accessor.AddFile($"{_key}/{filename}", new MockFileData(""));
                }
            }

            var daymock = new Mock<IDateTime>();
            daymock.Setup(day => day.Now).Returns(new DateTime(2020, 10, 10));

            ArchiveCheckController controller = new ArchiveCheckController(accessor.FileSystem, daymock.Object);
            var dataJson = controller.GetArchive();
            Assert.Equal(new int[] { 1991, 2015 }, dataJson.Keys.ToArray());
            Assert.Equal(new string[] { "day5.md", "day7.md", "day25.md" }, dataJson[2015]);
        }
        [Fact]
        public async Task ClientSideTest()
        {
            string raw = "{ \"2010\": [\"day1.md\", \"day5.md\", \"day24.md\"], \"2015\": [\"day5.md\", \"day7.md\",\"day9.md\",\"day24.md\"]}";
            var ReceiverMock = new Mock<IDataReceiver>(MockBehavior.Strict);
            ReceiverMock.Setup(receiver => receiver.ReceiveArchive(It.IsAny<string>())).ReturnsAsync(raw);
            ArchiveReader reader = new ArchiveReader(ReceiverMock.Object);

            int[] years = (await reader.GetYears("whatever")).ToArray();
            int[] days = (await reader.GetDays(2015, "meh")).ToArray();

            Assert.Equal(new int[] { 2010, 2015 }, years);
            Assert.Equal(new int[] { 5, 7, 9, 24 }, days);
        }
    }
}
