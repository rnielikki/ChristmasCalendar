namespace joulukalenteri.Shared
{
    /// <summary>
    /// Parsed and organized information from a day markdown file.
    /// </summary>
    public class DayInfoData
    {
        /// <summary>
        /// Maximum length of the summary from the content.
        /// </summary>
        public const int SummaryLength = 80;
        /// <summary>
        /// Represents the corresponding day of the data.
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// Title of the data.
        /// </summary>
        /// <remarks>Always first header of markdown syntax is title.</remarks>
        public string Title { get; set; }
        private string _summary;
        /// <summary>
        /// Truncated contents to <see cref="SummaryLength"/>
        /// </summary>
        public string Summary
        {
            get => _summary;
            set => _summary = (value.Length > SummaryLength) ? value.Substring(0, SummaryLength)+"..." : value;
        }
        /// <summary>
        /// HTML format text of the content.
        /// </summary>
        /// <remarks>Content doesn't contain text from the title. It's only in the <see cref="Title"/> propery.</remarks>
        public string Content { get; set; }
        /// <summary>
        /// Check if it's real data or placeholder. Default is true, unless it's created by <see cref="CreateEmpty" />.
        /// </summary>
        public bool Exists { get; private set; } = true;
        /// <summary>
        /// Create <see cref="DayInfoData" /> for non-existance. Placeholder of null.
        /// </summary>
        /// <param name="day">The day to set for the empty data</param>
        /// <returns>Pre-defined specific <see cref="DayInfoData" />.</returns>
        public static DayInfoData CreateEmpty(int day) => new DayInfoData() {
            Day = day,
            Title = "Not found",
            Summary = "This calendar content is not ready.",
            Content = "The text doesn't exist.",
            Exists = false
        };
    }
}
