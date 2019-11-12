using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using joulukalenteri.Client.SharedCode;
using joulukalenteri.Shared;
using Moq;

namespace joulukalenteriTests
{
    public class ShuffleTest
    {
        [Fact]
        public void ValidShuffleTest()
        {
            var daymock = new Mock<IDateTime>();
            daymock.Setup(day => day.Now).Returns(DateTime.Now);
            DaysShuffler shuffler = new DaysShuffler(daymock.Object);
            int[] shuffled = shuffler.ShuffleDays();
            //without duplication
            Assert.Equal(shuffled.Length, shuffled.Distinct().Count());

            //1-25
            Assert.Equal(1, shuffled.Min());
            Assert.Equal(25, shuffled.Max());

        }
        [Fact]
        public void ShuffleConsistenceTest() {
            var daymock = new Mock<IDateTime>();
            daymock.Setup(day => day.Now).Returns(new DateTime(2019,01,25));
            DaysShuffler shuffler = new DaysShuffler(daymock.Object);
            int[] shuffled = shuffler.ShuffleDays();
            //System.Index exists : must be written explicitly.
            Assert.Equal(shuffler.ShuffleDays(), shuffler.ShuffleDays());

            //make sure that not shuffled. very rarely not succeeded.
            Assert.NotEqual(shuffled, Enumerable.Range(1, 25).ToArray());

            //Different years, different days.
            var daymock2 = new Mock<IDateTime>();
            daymock2.Setup(day => day.Now).Returns(new DateTime(2020,12,12));
            DaysShuffler shuffler2 = new DaysShuffler(daymock2.Object);
            int[] shuffled2 = shuffler2.ShuffleDays();
            Assert.NotNull(shuffled2);
            Assert.NotEqual(shuffled, shuffled2);
        }
    }
}
