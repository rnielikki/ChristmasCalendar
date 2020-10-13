using System.Threading.Tasks;
using System.Net.Http;

namespace AdventCalendar.Services
{
    /// <summary>
    /// Provides the server data receiver interface.
    /// </summary>
    /// <remarks>
    /// Wraps the <see cref="HttpClient"/> for testing purpose.
    /// </remarks>
    public interface IDataReceiver {
        /// <summary>
        /// Gets if day file is availble.
        /// </summary>
        /// <param name="year">Target year to receive data.</param>
        /// <param name="day">Target day to receive data.</param>
        /// <returns>true if the file exists, false if it the file doesn't exist.</returns>
        Task<bool> CheckDayData(int year, int day);
        /// <summary>
        /// Provides getter interface of markdown of day file from the server.
        /// </summary>
        /// <param name="year">Target year to receive data.</param>
        /// <param name="day">Target day to receive data.</param>
        /// <returns>Raw, unparsed markdown file as string.</returns>
        Task<string> ReceiveDayData(int year, int day);
    }
}
