using System;
using System.IO.Abstractions;
using Xunit;
using Moq;
using joulukalenteri.Server.Controllers;
using joulukalenteri.Shared;

namespace joulukalenteriTests
{
    public class ReadTest
    {
        private const string testText = "Lorem ipsum dolor sit amet";
        [Theory]
        [InlineData(9, true)]
        [InlineData(10, true)]
        [InlineData(0, false)]
        [InlineData(11, false)]
        [InlineData(26, false)]
        public void ServerReadDayTest(int day, bool success)
        {
            DayReaderController controller = mockedController(new DateTime(DateTime.Today.Year, 12, 10));
            string result = controller.Get(day);
            Assert.Equal(success?testText:DayReaderController.WrongDateMessage, result);
        }
        [Theory]
        [MemberData(nameof(ServerReadYearTestData))]
        public void ServerReadYearTest(DateTime today, int year, int day, bool isValidDate)
        {
            DayReaderController controller = mockedController(today);
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
        private DayReaderController mockedController(DateTime datetime) {
            var fileSystemMock = new Mock<IFileSystem>(MockBehavior.Strict);
            fileSystemMock.Setup(filesystem => filesystem.File.ReadAllText(It.IsAny<string>())).Returns(testText);
            fileSystemMock.Setup(filesystem => filesystem.File.Exists(It.IsAny<string>())).Returns(true);
            var datemock = new Mock<IDateTime>(MockBehavior.Strict);
            datemock.Setup(date => date.Now).Returns(datetime);
            return new DayReaderController(fileSystemMock.Object, datemock.Object);
        }
    }
}
