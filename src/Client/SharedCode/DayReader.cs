using joulukalenteri.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Markdig;
using Markdig.Syntax;
using System.Linq;

namespace joulukalenteri.Client.SharedCode
{
    public interface IDayReader
    {
        Task<DayInfoData> GetContent(int day, string baseUri);
        Task<string> Generate(int day, string baseUri);
    }
    public class DayReader
    {
        private readonly IDataReceiver receiver;
        public DayReader(IDataReceiver _receiver) {
            receiver = _receiver;
        }
        public DayReader(HttpClient client)
        {
            receiver = new DataReceiver(client);
        }

        private Dictionary<int, DayInfoData> dataList = new Dictionary<int, DayInfoData>();
        public async Task<DayInfoData> GetContent(int day, string baseUri)
        {
            if (!dataList.ContainsKey(day))
            {
                dataList.Add(day, Parse(day, await receiver.Generate(day, baseUri)));
            }
            return dataList[day];
        }
        private DayInfoData Parse(int day, string input) {
            MarkdownDocument markdown = Markdown.Parse(input);
            HeadingBlock title = markdown.Descendants<HeadingBlock>().Where(block => block.Level == 1)?.FirstOrDefault();
            DayInfoData daydata = new DayInfoData();
            daydata.Title = title?.ToString() ?? $"Day {day}";
            return daydata;
        }
    }
}
