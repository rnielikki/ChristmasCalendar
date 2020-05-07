using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System;

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
        /// Gets if day file is availble.
        /// </summary>
        /// <param name="year">Target year to receive data.</param>
        /// <param name="day">Target day to receive data.</param>
        /// <param name="baseUri">The base uri of the current <see cref="HttpClient"/> page.</param>
        /// <returns>true if the file exists, false if it the file doesn't exist.</returns>
        Task<bool> CheckDayData(int year, int day, string baseUri);
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
        private readonly Dictionary<ValueTuple<int, int>, HttpResponseMessage> dataList = new Dictionary<ValueTuple<int, int>, HttpResponseMessage>();
        private readonly HttpClient _client;
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
        /// Gets if day file is availble.
        /// </summary>
        /// <param name="year">Target year to receive data.</param>
        /// <param name="day">Target day to receive data.</param>
        /// <param name="baseUri">The base uri of the current <see cref="HttpClient"/> page.</param>
        /// <returns>true if the file exists, false if it the file doesn't exist.</returns>
        // TODO: ALSO TEST
        public async Task<bool> CheckDayData(int year, int day, string baseUri)
        {
            if (!dataList.ContainsKey((year, day))) {
                dataList.Add((year, day), await _client.GetAsync($"{baseUri}contents/{year}/day{day}.md", HttpCompletionOption.ResponseHeadersRead));
            }
            return (dataList[(year, day)].IsSuccessStatusCode);
        }
        /// <summary>
        /// Gets markdown of day file.
        /// </summary>
        /// <param name="year">Target year to receive data.</param>
        /// <param name="day">Target day to receive data.</param>
        /// <param name="baseUri">The base uri of the current <see cref="HttpClient"/> page.</param>
        /// <returns>Raw, unparsed markdown file as string.</returns>
        public async Task<string> ReceiveDayData(int year, int day, string baseUri)
        {
            //TODO: Save checkDayData result for further use!
            if (await CheckDayData(year, day, baseUri))
            {
                var response = dataList[(year, day)];
                try
                {
                    return await response.Content.ReadAsStringAsync();
                }
                finally {
                    response.Dispose();
                    dataList.Remove((year, day));
                }
            }
            else {
                return null;
            }
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
