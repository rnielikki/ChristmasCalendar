using System;
using System.Linq;
using Xunit;
using AdventCalendar.Services;
using Moq;
using AdventCalendar.Settings;
using AdventCalendar.Models;

namespace AdventCalendarests
{
    public class ShuffleTest
    {
        private const int _days = 30;
        private readonly IAppSettings appSettings;
        public ShuffleTest()
        {
            var settingMock = new Mock<IAppSettings>();
            settingMock.Setup(item => item.Days).Returns(_days);
            appSettings = settingMock.Object;
        }
        [Fact]
        public void ValidShuffleTest()
        {
            var daymock = new Mock<IDateTime>();
            daymock.Setup(day => day.Now).Returns(DateTime.Now);
            DaysShuffler shuffler = new DaysShuffler(appSettings, daymock.Object);
            int[] shuffled = shuffler.ShuffleDays();
            //without duplication
            Assert.Equal(shuffled.Length, shuffled.Distinct().Count());

            Assert.Equal(1, shuffled.Min());
            Assert.Equal(_days, shuffled.Max());
        }
        [Fact]
        public void ShuffleConsistenceTest() {
            var daymock = new Mock<IDateTime>();
            daymock.Setup(day => day.Now).Returns(new DateTime(2019,01,25));
            DaysShuffler shuffler = new DaysShuffler(appSettings, daymock.Object);
            int[] shuffled = shuffler.ShuffleDays();
            //System.Index exists : must be written explicitly.
            Assert.Equal(shuffler.ShuffleDays(), shuffler.ShuffleDays());

            //make sure that not shuffled. very rarely not succeeded.
            Assert.NotEqual(shuffled, Enumerable.Range(1, _days).ToArray());

            //Different years, different days.
            var daymock2 = new Mock<IDateTime>();
            daymock2.Setup(day => day.Now).Returns(new DateTime(2020,12,12));
            DaysShuffler shuffler2 = new DaysShuffler(appSettings, daymock2.Object);
            int[] shuffled2 = shuffler2.ShuffleDays();
            Assert.NotNull(shuffled2);
            Assert.NotEqual(shuffled, shuffled2);
        }
    }
}
