using AdventCalendar.Models;
using AdventCalendar.Services;
using AdventCalendar.Settings;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AdventCalendarests
{
    public class ArchiveTest
    {
        private const int currentYear = 2018;
        private readonly IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("startYear", "2010"),
            new KeyValuePair<string, string>("skipYears:0", "2012"),
            new KeyValuePair<string, string>("skipYears:1", "2015")
        }).Build();
        private readonly IAppSettings settings = Mock.Of<IAppSettings>(mock =>
                mock.StartYear == 2010 &&
                mock.SkipYears == new int[] { 2012, 2015 }
            );
        [Fact]
        public void ReadAppSettingTest()
        {
            var settingFromConfig = new AppSettings(config);
            Assert.Equal(settingFromConfig.StartYear, settings.StartYear);
            Assert.Equal(settingFromConfig.SkipYears, settings.SkipYears);
        }

        [Fact]
        public void ArchiveRangeTest()
        {
            ArchiveReader reader = new ArchiveReader(settings, YearMock(currentYear));
            Assert.Equal(new int[] { 2010, 2011, 2013, 2014, 2016, 2017}, reader.GetYears());
        }
        [Theory]
        [InlineData(2010, true)]
        [InlineData(2015, false)]
        [InlineData(2016, true)]
        [InlineData(currentYear, false)]
        [InlineData(2019, false)]
        public void ArchiveAvailableYearTest(int year, bool expectResult)
        {
            ArchiveReader reader = new ArchiveReader(settings, YearMock(currentYear));
            Assert.Equal(expectResult, reader.IsAvailableYear(year));
        }
        [Fact]
        public void ArchiveThrowTest()
        {
            //Assert.NotThrows
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("startYear", currentYear.ToString()),
            }).Build();

            ArchiveReader reader = new ArchiveReader(settings, YearMock(currentYear));

            //Assert.Throws
            reader.GetYears();
            IAppSettings failSettings = Mock.Of<IAppSettings>(mock =>
                mock.StartYear == currentYear+1
            );

            ArchiveReader failReader = new ArchiveReader(failSettings, YearMock(currentYear));
            Assert.Throws<ArgumentOutOfRangeException>(failReader.GetYears);
        }
        private IDateTime YearMock(int year) {
            Mock<IDateTime> datetime = new Mock<IDateTime>();
            datetime.Setup(datetime => datetime.Year).Returns(year);
            return datetime.Object;
        }
    }
}
