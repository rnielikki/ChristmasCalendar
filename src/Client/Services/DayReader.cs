using System.Collections.Generic;
using System.Threading.Tasks;
using Markdig;
using Markdig.Syntax;
using System.Linq;
using Markdig.Renderers;
using System.IO;
using System;
using AdventCalendar.Models;
using AdventCalendar.Settings;

namespace AdventCalendar.Services
{
    /// <summary>
    /// Parses and reads markdown of the day data from the server.
    /// </summary>
    public class DayReader
    {
        private readonly IDataReceiver _receiver;
        private readonly IDateTime _datetime;
        private readonly int _summaryLength;
        private readonly MarkdownPipeline _pipeline;
        /// <summary>
        /// Calls day reader and abstract datetime manually for test purpose.
        /// </summary>
        /// <param name="receiver"><see cref="IDataReceiver"/>, which contains HTTP Client</param>
        /// <param name="datetime"><see cref="IDateTime"/>, which is possibly fake date.</param>
        /// <param name="settings">Injected application Setting.</param>
        public DayReader(IDataReceiver receiver, IDateTime datetime, IAppSettings settings)
        {
            _receiver = receiver;
            _datetime = datetime;
            _summaryLength = settings.SummaryLength;
            _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        }
        private readonly Dictionary<ValueTuple<int, int>, DayInfoData> dataList = new Dictionary<ValueTuple<int, int>, DayInfoData>();
        /// <summary>
        /// Get parsed markdown object asynchronously for current year with a day.
        /// </summary>
        /// <param name="day">The target day to get data.</param>
        /// <returns>Parsed <see cref="DayInfoData"/></returns>
        public async Task<DayInfoData> GetContent(int day) => await GetContent(_datetime.Now.Year, day).ConfigureAwait(true);
        /// <summary>
        /// Get Availability of specific day data
        /// </summary>
        /// <param name="year">The target year check data.</param>
        /// <param name="day">The target day to check data.</param>
        /// <returns></returns>
        public async Task<bool> GetAvailability(int year, int day) => await _receiver.CheckDayData(year, day).ConfigureAwait(true);
        /// <summary>
        /// Get parsed markdown object asynchronously with a day and a year.
        /// </summary>
        /// <param name="year">The target year to get data.</param>
        /// <param name="day">The target day to get data.</param>
        /// <returns>Parsed <see cref="DayInfoData"/></returns>
        public async Task<DayInfoData> GetContent(int year, int day)
        {
            if (!dataList.ContainsKey((year, day)))
            {
                if (await _receiver.CheckDayData(year, day).ConfigureAwait(true))
                    dataList.Add((year, day), Parse(day, await _receiver.ReceiveDayData(year, day).ConfigureAwait(true)));
                else
                    dataList.Add((year, day), DayInfoData.CreateEmpty(day));
            }
            return dataList[(year, day)];
        }
        private DayInfoData Parse(int day, string input)
        {
            MarkdownDocument markdown = Markdown.Parse(input, _pipeline);
            DayInfoData daydata = new DayInfoData
            {
                Day = day
            };
            //title part
            HeadingBlock title = markdown.Descendants<HeadingBlock>().Where(block => block.Level == 1)?.FirstOrDefault();
            if (title != null)
            {
                daydata.Title = title.Inline.FirstChild.ToString();
                markdown.Remove(title);
            }
            else
            {
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
                daydata.Summary = GetSummary(summary.ToString());
            }
            StringWriter writer = new StringWriter();
            HtmlRenderer renderer = new HtmlRenderer(writer);
            renderer.Render(markdown);
            daydata.Content = writer.ToString();
            return daydata;
        }
        private string GetSummary(string text) =>
            (text.Length > _summaryLength) ? text.Substring(0, _summaryLength)+"..." : text;
    }
}
