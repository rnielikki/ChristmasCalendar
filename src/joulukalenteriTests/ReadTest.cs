using System;
using Xunit;
using joulukalenteri.Server.Controllers;
using Moq;
using SystemWrapper.IO;
using SystemWrapper;

namespace joulukalenteriTests
{
    public class ReadTest
    {
        private const string testText = "Lorem ipsum dolor sit amet";
        [Fact]
        public void ServerReadDayTest()
        {
            DayReaderController controller = mockedController(new DateTimeWrap(DateTime.Today.Year, 12, 10));

            string result = controller.Get(1);
            string failedResult1 = controller.Get(0);
            string failedResult2 = controller.Get(26);

            Assert.Equal(testText, result);
            Assert.Equal(DayReaderController.WrongDateMessage, failedResult1);
            Assert.Equal(DayReaderController.WrongDateMessage, failedResult2);
        }
        [Theory]
        [MemberData(nameof(ServerReadYearTestData))]
        public void ServerReadYearTest(DateTime today, int year, int day, bool isValidDate)
        {
            DayReaderController controller = mockedController(new DateTimeWrap(today));
            string result = controller.Get(year, day);
            if (isValidDate)
            {
                Assert.NotEqual(result, DayReaderController.WrongDateMessage);
            }
            else
            {
                Assert.Equal(result, DayReaderController.WrongDateMessage);
            }
        }
        public static TheoryData<DateTime, int, int, bool> ServerReadYearTestData(){
            TheoryData<DateTime, int, int, bool> theories = new TheoryData<DateTime, int, int, bool>();
            theories.Add(new DateTime(2019,12,1),2018,99,false);
            theories.Add(new DateTime(2019,12,1),2018,25,true);
            theories.Add(new DateTime(2019,12,24),2019,5,true);
            theories.Add(new DateTime(2019,12,31),2019,26,false);
            theories.Add(new DateTime(2019,12,24),2019,24,true);
            theories.Add(new DateTime(2019,12,22),2020,1,false);
            return theories;
        }
        private DayReaderController mockedController(IDateTimeWrap datetime) {
            var filemock = new Mock<IFileWrap>(MockBehavior.Strict);
            filemock.Setup(fread => fread.ReadAllText(It.IsAny<string>())).Returns(testText);
            filemock.Setup(fread => fread.Exists(It.IsAny<string>())).Returns(true);
            var datemock = new Mock<IDateTimeWrap>(MockBehavior.Strict);
            datemock.Setup(date => date.Today).Returns(datetime);
            return new DayReaderController(filemock.Object, datemock.Object);
        }
    }
}
