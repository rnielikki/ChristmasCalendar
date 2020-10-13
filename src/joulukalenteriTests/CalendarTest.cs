using System;
using Xunit;
using Moq;
using AdventCalendar.Services;
using AdventCalendar.Settings;
using AdventCalendar.Models;

namespace AdventCalendarests
{
    public class CalendarTest
    {
        private readonly IAppSettings _settings;
        public CalendarTest()
        {
            var appSettings = new Mock<IAppSettings>();
            appSettings.Setup(set => set.Days).Returns(24);
            _settings = appSettings.Object;
        }
        [Theory]
        [MemberData(nameof(OpenByDateData))]
        public void OpenByDateTest(DateTime fakeday, int openday, bool isOpen)
        {
            var daymock = new Mock<IDateTime>();
            daymock.Setup(day => day.Now).Returns(fakeday);
            bool? result=null;
            Validator validator = new Validator(_settings, daymock.Object);
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
            Validator validator = new Validator(_settings, daymock.Object);
            result=validator.IsOpenToday(openyear, openday);
            Assert.Equal(isOpen, result);
        }

        [Fact]
        public void MaximumDayTest()
        {
            var day = Mock.Of<IDateTime>(mock => mock.Now == new DateTime(2020, 12, 25));
            Validator validator = new Validator(_settings, day);
            Assert.False(validator.IsOpenToday(2020, 25));
        }

        //-------------memberdata
        public static TheoryData<DateTime, int, bool> OpenByDateData() =>
            new TheoryData<DateTime, int, bool>() {
                { new DateTime(2020, 11, 11), 12, false },
                { new DateTime(2020, 12, 22), 21, true },
                { new DateTime(2019, 12, 15), 15, true },
                { new DateTime(2021, 12, 5), 19, false },
                { new DateTime(2021, 6, 1), 1, false }
            };
        public static TheoryData<DateTime, int, int, bool> OpenByYearData() =>
            new TheoryData<DateTime, int, int, bool>() {
                { new DateTime(2020, 11, 11), 2020, 12, false },
                { new DateTime(2021, 6, 5), 2022, 3, false },
                { new DateTime(2021, 12, 26), 2021, 24, true },
                { new DateTime(2020, 8, 12), 2018, 6, true },
            };
    }
}
