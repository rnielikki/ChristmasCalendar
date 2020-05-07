﻿using joulukalenteri.Client.SharedCode;
using joulukalenteri.Shared;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace joulukalenteriTests
{
    public class ArchiveTest
    {
        [Fact]
        public void ArchiveRangeTest()
        {
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("startYear", "2010"),
                new KeyValuePair<string, string>("skipYears:0", "2012"),
                new KeyValuePair<string, string>("skipYears:1", "2015")
            }).Build();
            Assert.Equal(2010, config.GetValue<int>("startYear"));
            Assert.Equal(new int[] { 2012, 2015 }, config.GetSection("skipYears").Get<int[]>());

            ArchiveReader reader = new ArchiveReader(config, YearMock(2018));
            Assert.Equal(new int[] { 2010, 2011, 2013, 2014, 2016, 2017}, reader.GetYears());
        }
        [Fact]
        public void ArchiveThrowTest() {
            IConfiguration config = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("startYear", "2010"),
            }).Build();

            ArchiveReader reader = new ArchiveReader(config, YearMock(2015));

            //Assert.NotThrows
            reader.GetYears();
            IConfiguration failConfig = new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("startYear", "2022"),
            }).Build();

            ArchiveReader sameYearReader = new ArchiveReader(failConfig, YearMock(2022));
            sameYearReader.GetYears();
            ArchiveReader failReader = new ArchiveReader(failConfig, YearMock(2010));
            Assert.Throws<ArgumentOutOfRangeException>(failReader.GetYears);
        }
        private IDateTime YearMock(int year) {
            Mock<IDateTime> datetime = new Mock<IDateTime>();
            datetime.Setup(datetime => datetime.Year).Returns(year);
            return datetime.Object;
        }
    }
}
