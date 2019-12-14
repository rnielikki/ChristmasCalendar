using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace joulukalenteri.Client.SharedCode
{
    public class ArchiveReader
    {
        private readonly IDataReceiver receiver;
        private Dictionary<int, IEnumerable<int>> ArchiveMap = null;
        public ArchiveReader(IDataReceiver _receiver) {
            receiver = _receiver;
        }
        public async Task<IEnumerable<int>> GetYears(string baseUri)
        {
            if (ArchiveMap == null) {
                await ReadArchive(baseUri);
            }
            return ArchiveMap.Keys;
        }
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
            string awaiter = await receiver.ReceiveArchive(baseUri);
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
