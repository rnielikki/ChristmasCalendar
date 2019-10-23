using System;
using Xunit;
using Pose;
using joulukalenteri.Client.SharedCode;

namespace joulukalenteriTests
{
    public class CalendarTest
    {
        [Theory]
        [MemberData(nameof(OpenByDateData))]
        //[InlineData(new DateTime(2020, 12, 21), 23, false)]
        public void OpenByDateTest(DateTime fakeday, int openday, bool isOpen)
        {
            Shim shim = Shim.Replace(() => DateTime.Now).With(()=>fakeday);
            bool? result=null;
            Validator validator = new Validator();
            PoseContext.Isolate(() =>
            {
                result=validator.IsOpenToday(openday);
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
