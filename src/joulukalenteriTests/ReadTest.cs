using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using joulukalenteri.Server.Controllers;
using joulukalenteri.Shared;
using joulukalenteri.Client.SharedCode;

namespace joulukalenteriTests
{
    public class ReadTest
    {
        [Fact]
        public void ServerDataTest(){
            DayReaderController readerController = new DayReaderController();
            string day1 = readerController.Get(1);
            Assert.Equal("asdf", day1);
            //Assert.Equal("My Title", day1.Title);
            //Assert.Equal("In A nutshell!", day1.Summary);
            //Assert.Equal("Here's some Content...", day1.Content);
        }
    }
}
