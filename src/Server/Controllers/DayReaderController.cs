using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using joulukalenteri.Shared;
using Markdig;

namespace joulukalenteri.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DayReaderController : ControllerBase
    {
        private const string WrongDateMessage = "You have wrong date.";
        private const string NotFoundMessage = "Sorry, the message is not ready!";
        private Dictionary<int, string> dataList = new Dictionary<int, string>();
        private string ReadData(int day) {
            if (!dataList.ContainsKey(day)) {
                string textPath = $"contents/day{day}.md";
                if (!System.IO.File.Exists(textPath))
                    dataList[day] = NotFoundMessage;

                else
                    dataList[day] = System.IO.File.ReadAllText(textPath);
            }
            return dataList[day];
        }
        //public DayInfoData GetDayInfoData(int day) => data.Where(dayinfo => dayinfo.Day == day).FirstOrDefault();
        [HttpGet]
        public string Get(int day) => (day>0 && day<DateTime.Today.Day)?ReadData(day):WrongDateMessage;

    }
}