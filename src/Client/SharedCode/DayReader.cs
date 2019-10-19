using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace joulukalenteri.Client.SharedCode
{
    public class DayReader
    {
        private readonly HttpClient _client;

        public DayReader(HttpClient client)
        {
            _client = client;
        }

        private string[] dataList = null;
        public async Task<string> GetContent(int day, string baseUri)
        {
            if (dataList == null)
            {
                await Generate(baseUri);
            }
            return dataList[day - 1];
            //return await client.GetStringAsync($"{baseUri}WeatherForecast");
            //return await new Task<string>(()=>"asdf");
            //return await new Task<string>(()=>(nav==null)?"null":"not null");
        }
        private async Task Generate(string baseUri)
        {
            int today = DateTime.Now.Day;
            dataList = new string[today];
            for (int i = 0; i < today; i++)
            {
                dataList[i] = await _client.GetStringAsync($"{baseUri}api/DayReader?day={i + 1}");
            }
        }
    }
}
