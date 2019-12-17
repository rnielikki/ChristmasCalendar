using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using joulukalenteri.Client.SharedCode;
using joulukalenteri.Shared;

namespace joulukalenteriTests
{
    public class ShuffleTest
    {
        [Fact]
        public void ValidShuffleTest()
        {
            DaysShuffler shuffler = new DaysShuffler(new ConcreateDate());
            int[] shuffled = shuffler.ShuffleDays();
            //without duplication
            Assert.Equal(shuffled.Length, shuffled.Distinct().Count());

            //1-25
            Assert.Equal(1, shuffled.Min());
            Assert.Equal(25, shuffled.Max());

        }
        [Fact]
        public void ShuffleConsistenceTest() {
            DaysShuffler shuffler = new DaysShuffler(new ConcreateDate(new DateTime(2019,01,25)));
            int[] shuffled = shuffler.ShuffleDays();
            //System.Index exists : must be written explicitly.
            Assert.Equal(shuffler.ShuffleDays(), shuffler.ShuffleDays());

            //make sure that not shuffled. very rarely not succeeded.
            Assert.NotEqual(shuffled, Enumerable.Range(1, 25).ToArray());

            //Different years, different days.
            DaysShuffler shuffler2 = new DaysShuffler(new ConcreateDate(new DateTime(2020,12,12)));
            int[] shuffled2 = shuffler2.ShuffleDays();
            Assert.NotNull(shuffled2);
            Assert.NotEqual(shuffled, shuffled2);
        }
    }
    internal class ConcreateDate : IDateTime
    {
        private DateTime datetime;
        public ConcreateDate()
        {
            datetime = DateTime.Now;
        }
        public ConcreateDate(DateTime _datetime)
        {
            datetime = _datetime;
        }
        public DateTime Now => datetime;
    }
}
