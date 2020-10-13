using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Components;
using AdventCalendar.Settings;

namespace AdventCalendar.Services
{
    /// <summary>
    /// Provides the concrete server data receiver.
    /// </summary>
    public class DataReceiver : IDataReceiver
    {
        private readonly Dictionary<ValueTuple<int, int>, HttpResponseMessage> dataList = new Dictionary<ValueTuple<int, int>, HttpResponseMessage>();
        private readonly HttpClient _client;
        private readonly string _baseUri;
        /// <summary>
        /// Creates DataReceiver for injecting purpose on a razor page.
        /// </summary>
        public DataReceiver(IAppSettings settings, NavigationManager nav)
        {
            _client = new HttpClient();
            _baseUri = string.IsNullOrEmpty(settings.BaseUri) ?
                new Uri(new Uri(nav.BaseUri), "contents").ToString()
                : settings.BaseUri;
        }
        /// <summary>
        /// Creates DataReceiver for testing purpose with custom HttpClient.
        /// </summary>
        /// <param name="client">Custom HTTPClient to get data.</param>
        public DataReceiver(HttpClient client)
        {
            _client = client;
            _baseUri = "";
        }
        /// <summary>
        /// Gets if day file is availble.
        /// </summary>
        /// <param name="year">Target year to receive data.</param>
        /// <param name="day">Target day to receive data.</param>
        /// <returns>true if the file exists, false if it the file doesn't exist.</returns>
        // TODO: ALSO TEST
        public async Task<bool> CheckDayData(int year, int day)
        {
            if (!dataList.ContainsKey((year, day)))
            {
                dataList.Add((year, day), await _client.GetAsync($"{_baseUri}/{year}/day{day}.md", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(true));
            }
            return dataList[(year, day)].IsSuccessStatusCode;
        }
        /// <summary>
        /// Gets markdown of day file.
        /// </summary>
        /// <param name="year">Target year to receive data.</param>
        /// <param name="day">Target day to receive data.</param>
        /// <returns>Raw, unparsed markdown file as string.</returns>
        public async Task<string> ReceiveDayData(int year, int day)
        {
            //TODO: Save checkDayData result for further use!
            if (await CheckDayData(year, day).ConfigureAwait(true))
            {
                var response = dataList[(year, day)];
                try
                {
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                }
                finally
                {
                    response.Dispose();
                    dataList.Remove((year, day));
                }
            }
            else
            {
                return null;
            }
        }
    }
}
