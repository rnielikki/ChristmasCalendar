using AdventCalendar.Models;
using AdventCalendar.Settings;
using System.Collections.Generic;
using System.Linq;

namespace AdventCalendar.Services
{
    /// <summary>
    /// Parses and reads JSON archive list from the server.
    /// </summary>
    public class ArchiveReader
    {
        private readonly IDateTime _datetime;
        private IEnumerable<int> _archiveYears;
        private readonly IAppSettings _appSettings;
        /// <summary>
        /// Calls archive reader manually for test purpose.
        /// </summary>
        /// <param name="settings">The applicatino settings.</param>
        /// <param name="datetime">Current date</param>
        public ArchiveReader(IAppSettings settings, IDateTime datetime) {
            _datetime = datetime;
            _appSettings = settings;
        }
        /// <summary>
        /// Get list of available calendar years asynchronously.
        /// </summary>
        /// <returns>Enumerable integers of available years.</returns>
        public IEnumerable<int> GetYears()
        {
            if (_archiveYears == null) {
                _archiveYears = Enumerable.Range(_appSettings.StartYear, _datetime.Year - _appSettings.StartYear);
                var skipYears = _appSettings.SkipYears;
                if(skipYears.Length != 0)
                    _archiveYears=_archiveYears.Except(skipYears);
            }
            return _archiveYears;
        }
        /// <summary>
        /// Get Available year.
        /// </summary>
        /// <param name="year">year to know if it's available</param>
        /// <returns><c>true</c> if the year is available, <c>false</c> if it doesn't</returns>
        public bool IsAvailableYear(int year)
        {
            if (_archiveYears == null) GetYears();
            return _archiveYears.Contains(year);
        }
    }
}
