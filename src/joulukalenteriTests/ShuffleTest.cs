using System;
using System.Collections.Generic;
using Pose;
using System.Text;
using System.Linq;
using Xunit;

namespace joulukalenteriTests
{
    public class ShuffleTest
    {
        joulukalenteri.Client.Pages.Index indexpage = new joulukalenteri.Client.Pages.Index();
        [Fact]
        public void ValidShuffleTest()
        {
            int[] shuffled = indexpage.ShuffleDays();
            //without duplication
            Assert.Equal(shuffled.Length, shuffled.Distinct().Count());

            //1-25
            Assert.Equal(1, shuffled.Min());
            Assert.Equal(25, shuffled.Max());

        }
        [Fact]
        public void ShuffleConsistenceTest() {
            int[] shuffled = indexpage.ShuffleDays();
            //System.Index exists : must be written explicitly.
            Assert.Equal(indexpage.ShuffleDays(), indexpage.ShuffleDays());

            //make sure that not shuffled. very rarely not succeeded.
            Assert.NotEqual(shuffled, Enumerable.Range(1, 25).ToArray());

            //Different years, different days.
            int[] shuffled2 = null;
            Shim shim =  Shim.Replace(() => DateTime.Today).With(() => new DateTime(DateTime.Today.Year-1,1,1));
            PoseContext.Isolate(() => {
                shuffled2 = indexpage.ShuffleDays();
            }, shim);
            Assert.NotNull(shuffled2);
            Assert.NotEqual(shuffled, shuffled2);
        }
    }
}
