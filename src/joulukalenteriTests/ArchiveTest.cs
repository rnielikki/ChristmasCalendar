using System;
using System.Collections.Generic;
using System.Text;
using joulukalenteri.Server.Controllers;
using Xunit;
using SystemWrapper;
using SystemWrapper.IO;
using Moq;
using System.Text.Json;
using joulukalenteri.Server;
using System.Linq;

namespace joulukalenteriTests
{
    public class ArchiveTest
    {
        [Fact]
        void ServerSideTest()
        {
            var entries = new Dictionary<string, string[]>()
            {
                {AppConfig.__dirpath + "2015", new string[] { "adsf.md", "test.md", "day1.txt", "day5.md", "day32.md" }},
                {AppConfig.__dirpath + "132f", new string[] { "day5.md", "day7.md", "day9.txt", "day24.md", "dummy.txt" }},
                {AppConfig.__dirpath + "2345", new string[] { "day5.md", "day7.md", "day9.txt", "day45.md", "day24.txt", "day25.md" }},
            };


            var mock = new Mock<IDirectoryWrap>();
            mock.Setup(dir => dir.Exists(It.IsAny<string>())).Returns(true);
            mock.Setup(dir => dir.GetDirectories(It.IsAny<string>())).Returns(entries.Keys.ToArray());

            foreach (var entry in entries) {
                mock.Setup(dir => dir.GetFiles(entry.Key))
                    .Returns(entry.Value);
            }
            ArchiveCheckController controller = new ArchiveCheckController(mock.Object);
            string dataJson = controller.GetArchive();
            var deserialized = JsonSerializer.Deserialize<Dictionary<string, string[]>>(dataJson);
            Assert.Equal(new string []{ "2015","2345"}, deserialized.Keys.ToArray());
            Assert.Equal(new string []{ "day5.md", "day7.md", "day25.md"}, deserialized["2345"]);
        }
    }
}
