using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using joulukalenteri.Client.Pages;
using Pose;

namespace joulukalenteriTest
{
    public class CalendarTest
    {
        joulukalenteri.Client.Pages.Index indexpage = new joulukalenteri.Client.Pages.Index();
        [Fact]
        public void ShuffleTest()
        {
            //System.Index exists : must be written explicitly.
            Assert.Equal(indexpage.ShuffleDays(), indexpage.ShuffleDays());

            int[] shuffled = indexpage.ShuffleDays();

            //without duplication
            Assert.Equal(shuffled.Length, shuffled.Distinct().Count());

            //1-25
            Assert.Equal(1, shuffled.Min());
            Assert.Equal(25, shuffled.Max());

            //make sure that not shuffled. very rarely not succeeded.
            Assert.NotEqual(shuffled, Enumerable.Range(1, 25).ToArray());

            /*
            Test TODO:
            1. Random 1-25 mixing.
                -> use YEAR as random seed. Hint: new Random(YEAR);
            2. Check if activates according to date.
                -> Vaihtoehto 1: "Shim" with *isolation framework*. ( Typemock, Microsoft moles, Pose, ... )
                -> Vaihtoehto 2: Using "Moq" but do something more ( https://stackoverflow.com/questions/2425721/unit-testing-datetime-now )
            3. Read Custom JSON file ([{day:..., title:..., content:..., image:...}, ...]) and asserts if everything Equals.
                -> use "Theory data (inline data is ok.)"
                -> DateTime Change trick is same as 2.
            4. Show only some months. (not open for summer!)
                -> Hint is same as 2.

            Archive not supported :)
             */
        }
        [Theory]
        [MemberData(nameof(OpenByDateData))]
        //[InlineData(new DateTime(2020, 12, 21), 23, false)]
        public void OpenByDateTest(DateTime fakeday, int openday, bool isOpen)
        {
            bool? result=null;
            Shim shim = Shim.Replace(() => DateTime.Now).With(() => fakeday);
            PoseContext.Isolate(() =>
            {
                result=indexpage.IsOpenToday(openday);
            }, shim);
            //Assert.False(result);
            Assert.Equal(isOpen, result);
        }

        //-------------memberdata
        public static TheoryData<DateTime, int, bool> OpenByDateData() =>
            new TheoryData<DateTime, int, bool>() {
                { new DateTime(2020, 11, 11), 12, false },
                { new DateTime(2020, 12, 22), 21, true },
                { new DateTime(2019, 12, 15), 15, true },
                { new DateTime(2021, 12, 5), 19, false },
                { new DateTime(2021, 12, 26), 25, true },
                { new DateTime(2022, 8, 12), 3, false },
                { new DateTime(2021, 6, 1), 1, false }
            };
    }
}
