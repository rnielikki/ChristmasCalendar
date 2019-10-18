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
        private string[] data;
        public DayReaderController() {
            DateTime now = DateTime.Now;
            /*if (now.Month != 12)
            {
                data = new string[0];
                return;
            }*/
            int today = DateTime.Now.Day;
            data = new string[today];
            for (int i=0;i<today;i++) {
                string textPath = $"contents/day{i + 1}.md";
                if (!System.IO.File.Exists(textPath))
                    data[i] = "";

                else
                    data[i] = System.IO.File.ReadAllText(textPath);
            }
           // HttpClient client = new HttpClient();
            //data = JsonSerializer.Serialize();
        }
        //public DayInfoData GetDayInfoData(int day) => data.Where(dayinfo => dayinfo.Day == day).FirstOrDefault();
        [HttpGet]
        public string Get(int day) => (day>0 && day<26)?data[day - 1]:"";

    }
}