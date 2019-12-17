using Microsoft.AspNetCore.Mvc;
using System.IO.Abstractions;
using joulukalenteri.Shared;

namespace joulukalenteri.Server.Controllers
{
    /// <summary>
    /// Returns list of valid and available markdown file from the <see cref="AppConfig.__dirpath"/> folder.
    /// </summary>
    /// <seealso cref="joulukalenteri.Client.SharedCode.DayReader"/>
    [Route("api/[controller]")]
    [ApiController]
    public class DayReaderController : ControllerBase
    {
        /// <summary>
        /// Placeholder of content, when the date is invalid or future.
        /// </summary>
        public const string WrongDateMessage = "You have wrong date.";
        /// <summary>
        /// Placeholder of content, when the date is valid but file does not exist.
        /// </summary>
        public const string NotFoundMessage = "Sorry, the message is not ready!";
        private readonly IFileSystem fileSystemWrap;
        private readonly IDateTime datewrap;
        /// <summary>
        /// Gets wrappers as parameter for test purpose.
        /// </summary>
        /// <param name="_fileSystemWrap">Mocked fileyWrapper for the test.</param>
        /// <param name="_datewrap">Mocked dateTimeWrapper for the test.</param>
        public DayReaderController(IFileSystem _fileSystemWrap, IDateTime _datewrap)
        {
            fileSystemWrap = _fileSystemWrap;
            datewrap = _datewrap;
        }
        /// <summary>
        /// Get the raw data from a day of current year.
        /// </summary>
        /// <param name="day">Day of current year to get markdown string.</param>
        /// <returns>Raw markdown text file of the given day.</returns>
        [HttpGet("{day}")]
        public string Get(int day) => Get(datewrap.Now.Year, day);
        /// <summary>
        /// Get the raw data with a day and a year.
        /// </summary>
        /// <param name="year">Year to get markdown string.</param>
        /// <param name="day">Day to get markdown string.</param>
        /// <returns>Raw markdown text file of the given year and day.</returns>
        [HttpGet("{year}/{day}")]
        public string Get(int year, int day){
            var today = datewrap.Now;
            if ((day > 0 && day < 26) && ((year < today.Year) || (year == today.Year && day <= today.Day)))
            {
                return ReadData(year, day);
            }
            return WrongDateMessage;
        }
        private string ReadData(int year, int day)
        {
            string textPath = $"{AppConfig.__dirpath}{year}/day{day}.md";
            if (!fileSystemWrap.File.Exists(textPath))
                return NotFoundMessage;

            else
                return fileSystemWrap.File.ReadAllText(textPath);
        }
    }
}