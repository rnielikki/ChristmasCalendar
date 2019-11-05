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
    /// <summary>
    /// Parses and reads markdown of the day data from the server.
    /// </summary>
    public class DayReader
    {
        private readonly IDataReceiver receiver;
        /// <summary>
        /// Calls day reader manually for test purpose.
        /// </summary>
        /// <param name="_receiver"><see cref="IDataReceiver"/>, which contains HTTP Client</param>
        public DayReader(IDataReceiver _receiver) {
            receiver = _receiver;
        }
        private Dictionary<ValueTuple<int, int>, DayInfoData> dataList = new Dictionary<ValueTuple<int, int>, DayInfoData>();
        /// <summary>
        /// Get parsed markdown object asynchronously for current year with a day.
        /// </summary>
        /// <param name="day">The target day to get data.</param>
        /// <param name="baseUri">The base uri of the current <see cref="System.Net.Http.HttpClient"/> page.</param>
        /// <returns>Parsed <see cref="DayInfoData"/></returns>
        public async Task<DayInfoData> GetContent(int day, string baseUri) => await GetContent(DateTime.Today.Year, day, baseUri);
        /// <summary>
        /// Get parsed markdown object asynchronously with a day and a year.
        /// </summary>
        /// <param name="year">The target year to get data.</param>
        /// <param name="day">The target day to get data.</param>
        /// <param name="baseUri">The base uri of the current <see cref="System.Net.Http.HttpClient"/> page.</param>
        /// <returns>Parsed <see cref="DayInfoData"/></returns>
        public async Task<DayInfoData> GetContent(int year, int day, string baseUri)
        {
            if (!dataList.ContainsKey((year, day)))
            {
                dataList.Add((year, day), Parse(day, await receiver.ReceiveDayData(year, day, baseUri)));
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
