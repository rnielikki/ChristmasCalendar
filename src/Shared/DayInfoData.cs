using System;
using System.Collections.Generic;
using System.Text;

namespace joulukalenteri.Shared
{
    public class DayInfoData
    {
        private const int SummaryLength = 30;
        public int Day { get; set; }
        public string Title { get; set; }
        private string _summary;
        public string Summary
        {
            get => _summary;
            set => _summary = (value.Length > SummaryLength) ? value.Substring(0, SummaryLength)+"..." : value;
        }
        public string Content { get; set; }
    }
}
