using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using SystemWrapper.IO;

namespace joulukalenteri.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayReaderController : ControllerBase
    {
        private const string WrongDateMessage = "You have wrong date.";
        private const string NotFoundMessage = "Sorry, the message is not ready!";
        private Dictionary<int, string> dataList = new Dictionary<int, string>();
        private readonly IFileWrap _filewrap;

        public DayReaderController(IFileWrap wrap)
        {
            _filewrap = wrap;
        }

        private string ReadData(int day) {
            if (!dataList.ContainsKey(day)) {
                string textPath = $"contents/day{day}.md";
                if (!_filewrap.Exists(textPath))
                    dataList[day] = NotFoundMessage;

                else
                    dataList[day] = _filewrap.ReadAllText(textPath);
            }
            return dataList[day];
        }
        //public DayInfoData GetDayInfoData(int day) => data.Where(dayinfo => dayinfo.Day == day).FirstOrDefault();
        [HttpGet]
        public string Get(int day) => (day>0 && day<DateTime.Today.Day && day<26)?ReadData(day):WrongDateMessage;

    }
}