using AdventCalendar;
using AdventCalendar.Client.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AdventCalendarTests
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
        [Fact]
        public void ConfigGetTest()
        {
            Assert.Equal(2010, config.GetValue<int>("startYear"));
            Assert.Equal(new int[] { 2012, 2015 }, config.GetSection("skipYears").Get<int[]>());
        }

        [Fact]
        public void ArchiveRangeTest()
        {
            ArchiveReader reader = new ArchiveReader(config, YearMock(currentYear));
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
            ArchiveReader reader = new ArchiveReader(config, YearMock(currentYear));
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

            ArchiveReader reader = new ArchiveReader(config, YearMock(currentYear));

            //Assert.Throws
            reader.GetYears();
            IConfiguration failConfig = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("startYear", (currentYear + 1).ToString()),
            }).Build();

            ArchiveReader failReader = new ArchiveReader(failConfig, YearMock(currentYear));
            Assert.Throws<ArgumentOutOfRangeException>(failReader.GetYears);
        }
        private IDateTime YearMock(int year) {
            Mock<IDateTime> datetime = new Mock<IDateTime>();
            datetime.Setup(datetime => datetime.Year).Returns(year);
            return datetime.Object;
        }
    }
}
