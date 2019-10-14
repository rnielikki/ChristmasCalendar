using System;
using Xunit;
using System.Linq;
using joulukalenteri.Client.Pages;

namespace joulukalenteriTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //System.Index exists : must be written explicitly.
            joulukalenteri.Client.Pages.Index indexpage = new joulukalenteri.Client.Pages.Index();
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
    }
}
