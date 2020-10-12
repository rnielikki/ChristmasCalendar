using System;
using Xunit;
using Moq;
using AdventCalendar.Client.Services;
using AdventCalendar;

namespace AdventCalendarTests
{
    public class CalendarTest
    {
        [Theory]
        [MemberData(nameof(OpenByDateData))]
        public void OpenByDateTest(DateTime fakeday, int openday, bool isOpen)
        {
            var daymock = new Mock<IDateTime>();
            daymock.Setup(day => day.Now).Returns(fakeday);
            bool? result=null;
            Validator validator = new Validator(daymock.Object);
            result=validator.IsOpenToday(openday);
            Assert.Equal(isOpen, result);
        }
        [Theory]
        [MemberData(nameof(OpenByYearData))]
        public void OpenByYearTest(DateTime fakeday, int openyear, int openday, bool isOpen)
        {
            var daymock = new Mock<IDateTime>();
            daymock.Setup(day => day.Now).Returns(fakeday);
            bool? result=null;
            Validator validator = new Validator(daymock.Object);
            result=validator.IsOpenToday(openyear, openday);
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
        public static TheoryData<DateTime, int, int, bool> OpenByYearData() =>
            new TheoryData<DateTime, int, int, bool>() {
                { new DateTime(2020, 11, 11), 2020, 12, false },
                { new DateTime(2020, 12, 22), 2019, 21, true },
                { new DateTime(2019, 12, 15), 2018, 2, true },
                { new DateTime(2021, 6, 5), 2022, 3, false },
                { new DateTime(2021, 12, 26), 2021, 25, true },
                { new DateTime(2020, 8, 12), 2018, 6, true },
            };
    }
}
