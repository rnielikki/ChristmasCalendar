namespace AdventCalendar.Settings
{
    /// <summary>
    /// Provides interface of <see cref="AppSettings"/> for testing purpose.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Represents from which year the archive starts.
        /// </summary>
        public int StartYear { get; }
        /// <summary>
        /// The years archive didn't exist (skipped). Should be more than <see cref="StartYear"/>, but less than current year.
        /// </summary>
        public int[] SkipYears { get; }
        /// <summary>
        /// The title of the application. For example, it can represent the whole calendar theme.
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// The days in the calendar.
        /// </summary>
        public int Days { get; }
        /// <summary>
        /// The path, where the contents are. Empty BaseUri resolves contents/ folder as default.
        /// </summary>
        public string BaseUri { get; }
        /// <summary>
        /// Length of the sumamry.
        /// </summary>
        public int SummaryLength { get; }
    }
}
