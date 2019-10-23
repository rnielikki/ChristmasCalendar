using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using SystemWrapper.IO;
using SystemWrapper;

namespace joulukalenteri.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayReaderController : ControllerBase
    {
        public const string WrongDateMessage = "You have wrong date.";
        public const string NotFoundMessage = "Sorry, the message is not ready!";
        private Dictionary<ValueTuple<int, int>, string> dataList = new Dictionary<ValueTuple<int, int>, string>();
        private readonly IFileWrap _filewrap;
        private readonly IDateTimeWrap _datewrap;

        [HttpGet("{day}")]
        public string Get(int day) => Get(_datewrap.Today.Year, day);
        [HttpGet("{year}/{day}")]
        public string Get(int year, int day){
            IDateTimeWrap today = _datewrap.Today;
            if ((day > 0 && day < 26) && ((year < today.Year) || (year == today.Year && day <= today.Day)))
            {
                return ReadData(year, day);
            }
            return WrongDateMessage;
        }
        public DayReaderController(IFileWrap wrap, IDateTimeWrap datewrap)
        {
            _filewrap = wrap;
            _datewrap = datewrap;
        }
        private string ReadData(int year, int day) {
            if (!dataList.ContainsKey((year, day))) {
                string textPath = $"contents/{year}/day{day}.md";
                if (!_filewrap.Exists(textPath))
                    dataList[(year, day)] = NotFoundMessage;

                else
                    dataList[(year, day)] = _filewrap.ReadAllText(textPath);
            }
            return dataList[(year, day)];
        }
    }
}