using joulukalenteri.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig;
using Markdig.Syntax;
using System.Linq;
using Markdig.Renderers;
using System.IO;
using System;

namespace joulukalenteri.Client.SharedCode
{
    public class DayReader
    {
        private readonly IDataReceiver receiver;
        public DayReader(IDataReceiver _receiver) {
            receiver = _receiver;
        }
        private Dictionary<ValueTuple<int, int>, DayInfoData> dataList = new Dictionary<ValueTuple<int, int>, DayInfoData>();
        public async Task<DayInfoData> GetContent(int day, string baseUri) => await GetContent(DateTime.Today.Year, day, baseUri);
        public async Task<DayInfoData> GetContent(int year, int day, string baseUri)
        {
            if (!dataList.ContainsKey((year, day)))
            {
                dataList.Add((year, day), Parse(day, await receiver.Generate(year, day, baseUri)));
            }
            return dataList[(year, day)];
        }
        private DayInfoData Parse(int day, string input) {
            MarkdownDocument markdown = Markdown.Parse(input);
            DayInfoData daydata = new DayInfoData();
            daydata.Day = day;
            //title part
            HeadingBlock title = markdown.Descendants<HeadingBlock>().Where(block => block.Level == 1)?.FirstOrDefault();
            if (title != null)
            {
                daydata.Title = title.Inline.FirstChild.ToString();
                markdown.Remove(title);
            }
            else {
                daydata.Title = $"Day {day}";
            }
            //content part
            var summary = markdown.Descendants<LeafBlock>().FirstOrDefault()?.Inline?.FirstChild;
            if (summary == null)
            {
                daydata.Summary = "";
            }
            else
            {
                daydata.Summary = summary.ToString();
            }
            StringWriter writer = new StringWriter();
            HtmlRenderer renderer = new HtmlRenderer(writer);
            renderer.Render(markdown);
            daydata.Content = writer.ToString();
            return daydata;
        }
    }
}
