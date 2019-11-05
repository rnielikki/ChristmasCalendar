using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace joulukalenteri.Client.SharedCode
{
    /// <summary>
    /// Parses and reads JSON archive list from the server.
    /// </summary>
    public class ArchiveReader
    {
        private readonly IDataReceiver _receiver;
        private Dictionary<int, IEnumerable<int>> ArchiveMap = null;
        /// <summary>
        /// Calls archive reader manually for test purpose.
        /// </summary>
        /// <param name="receiver"><see cref="IDataReceiver"/>, which contains HTTP Client</param>
        public ArchiveReader(IDataReceiver receiver) {
            _receiver = receiver;
        }
        /// <summary>
        /// Get list of available calendar years asynchronously.
        /// </summary>
        /// <param name="baseUri">The base uri of the current <see cref="System.Net.Http.HttpClient"/> page.</param>
        /// <returns>Enumerable integers of available years.</returns>
        public async Task<IEnumerable<int>> GetYears(string baseUri)
        {
            if (ArchiveMap == null) {
                await ReadArchive(baseUri);
            }
            return ArchiveMap.Keys;
        }
        /// <summary>
        /// Get list of available calendar days from a year asynchronously.
        /// </summary>
        /// <param name="year">The target year to get available days.</param>
        /// <param name="baseUri">The base uri of the current <see cref="System.Net.Http.HttpClient"/> page.</param>
        /// <returns>Enumerable integers of available days.</returns>
        public async Task<IEnumerable<int>> GetDays(int year, string baseUri)
        {
            if (ArchiveMap == null) {
                await ReadArchive(baseUri);
            }
            if (!ArchiveMap.ContainsKey(year))
            {
                return new int[0];
            }
            else
            {
                return ArchiveMap[year];
            }
        }
        private async Task ReadArchive(string baseUri) {
            string awaiter = await _receiver.ReceiveArchive(baseUri);
            var parsedString = JsonConvert.DeserializeObject<Dictionary<int, IEnumerable<string>>>(awaiter);
            ArchiveMap = parsedString.ToDictionary(pair => pair.Key, pair => {
                return pair.Value.Select(filename =>
                {
                    int day;
                    int.TryParse(new string(filename.Where(char.IsDigit).ToArray()), out day);
                    return day;
                });
            });
        }
    }
}
