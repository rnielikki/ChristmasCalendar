using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using SystemWrapper.IO;
using SystemWrapper;

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
        private readonly IFileWrap _filewrap;
        private readonly IDateTimeWrap _datewrap;
        /// <summary>
        /// Gets wrappers as parameter for test purpose.
        /// </summary>
        /// <param name="filewrap">Mocked fileyWrapper for the test.</param>
        /// <param name="datewrap">Mocked dateTimeWrapper for the test.</param>
        public DayReaderController(IFileWrap filewrap, IDateTimeWrap datewrap)
        {
            _filewrap = filewrap;
            _datewrap = datewrap;
        }
        /// <summary>
        /// Get the raw data from a day of current year.
        /// </summary>
        /// <param name="day">Day of current year to get markdown string.</param>
        /// <returns>Raw markdown text file of the given day.</returns>
        [HttpGet("{day}")]
        public string Get(int day) => Get(_datewrap.Today.Year, day);
        /// <summary>
        /// Get the raw data with a day and a year.
        /// </summary>
        /// <param name="year">Year to get markdown string.</param>
        /// <param name="day">Day to get markdown string.</param>
        /// <returns>Raw markdown text file of the given year and day.</returns>
        [HttpGet("{year}/{day}")]
        public string Get(int year, int day){
            IDateTimeWrap today = _datewrap.Today;
            if ((day > 0 && day < 26) && ((year < today.Year) || (year == today.Year && day <= today.Day)))
            {
                return ReadData(year, day);
            }
            return WrongDateMessage;
        }
        private string ReadData(int year, int day)
        {
            string textPath = $"{AppConfig.__dirpath}{year}/day{day}.md";
            if (!_filewrap.Exists(textPath))
                return NotFoundMessage;

            else
                return _filewrap.ReadAllText(textPath);
        }
    }
}