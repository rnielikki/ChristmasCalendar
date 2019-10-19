using joulukalenteri.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace joulukalenteri.Client.SharedCode
{
    interface IDayReader
    {
        Task<DayInfoData> GetContent(int day, string baseUri);
        Task<string> Generate(int day, string baseUri);
    }
    public class DayReader : IDayReader
    {
        private readonly HttpClient _client;

        public DayReader(HttpClient client)
        {
            _client = client;
        }

        private Dictionary<int, DayInfoData> dataList = new Dictionary<int, DayInfoData>();
        public async Task<DayInfoData> GetContent(int day, string baseUri)
        {
            if (!dataList.ContainsKey(day))
            {
                dataList.Add(day, Parse(await Generate(day, baseUri)));
            }
            return dataList[day];
        }
        public async Task<string> Generate(int day, string baseUri)
        {
            int today = DateTime.Today.Day;
            return await _client.GetStringAsync($"{baseUri}api/DayReader?day={day}");
        }
        private DayInfoData Parse(string input) {
            return new DayInfoData();
        }
    }
}
