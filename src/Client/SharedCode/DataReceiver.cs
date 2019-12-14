﻿using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace joulukalenteri.Client.SharedCode
{
    //for testing purpose, it's outside of the part of the DayReader.
    public interface IDataReceiver {
        Task<string> Generate(int year, int day, string baseUri);
        Task<string> ReceiveArchive(string baseUri);
    }
    public class DataReceiver:IDataReceiver
    {
        private HttpClient _client;
        public DataReceiver() {
            _client = new HttpClient();
        }
        public DataReceiver(HttpClient client) {
            _client = client;
        }
        public async Task<string> Generate(int year, int day, string baseUri)
        {
            int today = DateTime.Today.Day;
            return await _client.GetStringAsync($"{baseUri}api/DayReader/{year}/{day}");
        }
        public async Task<string> ReceiveArchive(string baseUri) {
            return await _client.GetStringAsync($"{baseUri}api/ArchiveCheck");
        }
    }
}
