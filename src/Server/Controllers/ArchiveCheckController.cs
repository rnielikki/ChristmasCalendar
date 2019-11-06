using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SystemWrapper;
using SystemWrapper.IO;
using System.IO;
using System.Text.RegularExpressions;

namespace joulukalenteri.Server.Controllers
{
    /// <summary>
    /// Returns list of valid and available markdown file names from the archive.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveCheckController : ControllerBase
    {
        private readonly IDirectoryWrap _dirwrap;
        private readonly IDateTimeWrap _datewrap;
        /// <summary>
        /// Gets wrappers as parameter for test purpose.
        /// </summary>
        /// <param name="dirwrap">Mocked directoryWrapper for the test.</param>
        /// <param name="datewrap">Mocked dateTimeWrapper for the test.</param>
        public ArchiveCheckController(IDirectoryWrap dirwrap, IDateTimeWrap datewrap)
        {
            _dirwrap = dirwrap;
            _datewrap = datewrap;
        }
        /// <summary>
        /// Reads available file names from each year.
        /// </summary>
        /// <returns>Dictionary of archive file names for each valid year, which produces JSON file for clients.</returns>
        /// <remarks>The archive file path should be in the path from <see cref="AppConfig.__dirpath"/></remarks>
        /// <seealso cref="joulukalenteri.Client.SharedCode.ArchiveReader"/>
        public Dictionary<int,IEnumerable<string>> GetArchive() {
            if (_dirwrap.Exists(AppConfig.__dirpath))
            {
                int thisYear = _datewrap.Today.Year;
                Regex regex = new Regex(@"^day([1-9]|1[0-9]|2[0-5])\.md$");
                Dictionary<int, IEnumerable<string>> results = new Dictionary<int, IEnumerable<string>>();
                int pathLength = AppConfig.__dirpath.Length;
                string[] dirs = _dirwrap.GetDirectories(AppConfig.__dirpath);

                foreach (string dir in dirs) {
                    string dirName = dir.Substring(pathLength);
                    int year;
                    if (int.TryParse(dirName, out year) && year < thisYear)
                    {
                        results.Add(year, _dirwrap.GetFiles(dir).Select(str => Path.GetFileName(str)).Where(str => regex.Match(str).Success).ToArray());
                    }
                }
                return results;
            }
            else
            {
                return null;
            }
        }
    }
}