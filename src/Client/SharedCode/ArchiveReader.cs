using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace joulukalenteri.Client.SharedCode
{
    /// <summary>
    /// Parses and reads JSON archive list from the server.
    /// </summary>
    public class ArchiveReader
    {
        private readonly IConfiguration _config;
        private readonly joulukalenteri.Shared.IDateTime _datetime;
        private IEnumerable<int> _archiveYears = null;
        /// <summary>
        /// Calls archive reader manually for test purpose.
        /// </summary>
        /// <param name="config">Configuration from appsettings.json</param>
        /// <param name="datetime">Current date</param>
        public ArchiveReader(IConfiguration config, joulukalenteri.Shared.IDateTime datetime) {
            _config = config;
            _datetime = datetime;
        }
        /// <summary>
        /// Get list of available calendar years asynchronously.
        /// </summary>
        /// <returns>Enumerable integers of available years.</returns>
        public IEnumerable<int> GetYears() {
            if (_archiveYears == null) {
                int startYear = _config.GetValue<int>("startYear");
                _archiveYears = Enumerable.Range(startYear, _datetime.Year - startYear);
                var skipYears = _config.GetSection("skipYears")?.Get<int[]>();
                if(skipYears!=null)
                    _archiveYears=_archiveYears.Except(skipYears);
            }
            return _archiveYears;
        }
    }
}
