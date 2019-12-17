using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace joulukalenteri.Client.SharedCode
{
    /// <summary>
    /// Provides the server data receiver interface.
    /// </summary>
    /// <remarks>
    /// Wraps the <see cref="System.Net.Http.HttpClient"/> for testing purpose.
    /// </remarks>
    public interface IDataReceiver {
        /// <summary>
        /// Provides getter interface of markdown of day file from the server.
        /// </summary>
        /// <param name="year">Target year to receive data.</param>
        /// <param name="day">Target day to receive data.</param>
        /// <param name="baseUri">The base uri of the current <see cref="HttpClient"/> page.</param>
        /// <returns>Raw, unparsed markdown file as string.</returns>
        Task<string> ReceiveDayData(int year, int day, string baseUri);
        /// <summary>
        /// Provides getter interface of JSON data of available days and years from the server.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <returns>Unparsed JSON string of the available year and day file names.</returns>
        Task<string> ReceiveArchive(string baseUri);
    }
    /// <summary>
    /// Provides the concrete server data receiver.
    /// </summary>
    public class DataReceiver:IDataReceiver
    {
        private HttpClient _client;
        /// <summary>
        /// Creates DataReceiver for injecting purpose on a razor page.
        /// </summary>
        public DataReceiver() {
            _client = new HttpClient();
        }
        /// <summary>
        /// Creates DataReceiver for testing purpose with custom HttpClient.
        /// </summary>
        /// <param name="client">Custom HTTPClient to get data.</param>
        public DataReceiver(HttpClient client) {
            _client = client;
        }
        /// <summary>
        /// Gets markdown of day file from the server.
        /// </summary>
        /// <param name="year">Target year to receive data.</param>
        /// <param name="day">Target day to receive data.</param>
        /// <param name="baseUri">The base uri of the current <see cref="HttpClient"/> page.</param>
        /// <returns>Raw, unparsed markdown file as string.</returns>
        public async Task<string> ReceiveDayData(int year, int day, string baseUri)
        {
            return await _client.GetStringAsync($"{baseUri}api/DayReader/{year}/{day}");
        }
        /// <summary>
        /// Provides JSON data of available days and years from the server.
        /// </summary>
        /// <param name="baseUri">The base uri of the current <see cref="HttpClient"/> page.</param>
        /// <returns>Unparsed JSON string of the available year and day file names.</returns>
        public async Task<string> ReceiveArchive(string baseUri) {
            return await _client.GetStringAsync($"{baseUri}api/ArchiveCheck");
        }
    }
}
