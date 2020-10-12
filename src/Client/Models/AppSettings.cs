using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("AdventCalendarTests")]
namespace AdventCalendar.Client
{
    /// <summary>
    /// Model of app user settings. Same structure as appsettings.json, except 'days' data.
    /// </summary>
    internal class AppSettings:IAppSettings
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
        /// <value>If <c>containsChristmas</c> in the settings is <c>true</c>, 25. Otherwise 24.</value>
        public int Days { get; }
        /// <summary>
        /// The path, where the contents are. Empty BaseUri resolves contents/ folder as default.
        /// </summary>
        public string BaseUri { get; }

        internal AppSettings(IConfiguration config)
        {
            StartYear = config.GetValue("startYear", 2000);
            SkipYears = config.GetSection("skipYears")?.Get<int[]>() ?? new int[] { };
            Title = config.GetValue("title", "Advent Calendar");
            Days = (config.GetValue("containsChristmas", false)) ? 25 : 24;
            BaseUri = config.GetValue("baseUri", "");
        }
    }
}
