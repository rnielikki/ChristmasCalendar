using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using joulukalenteri.Shared;

namespace joulukalenteri.Server.Controllers
{
    /// <summary>
    /// Returns list of valid and available markdown file names from the archive.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveCheckController : ControllerBase
    {
        private readonly IFileSystem fileSystemWrap;
        private readonly IDateTime datewrap;
        /// <summary>
        /// Gets wrappers as parameter for test purpose.
        /// </summary>
        /// <param name="_fileSystemWrap">Mocked directoryper for the test.</param>
        /// <param name="_datewrap">Mocked dateTimeper for the test.</param>
        public ArchiveCheckController(IFileSystem _fileSystemWrap, IDateTime _datewrap)
        {
            fileSystemWrap = _fileSystemWrap;
            datewrap = _datewrap;
        }
        /// <summary>
        /// Reads available file names from each year.
        /// </summary>
        /// <returns>Dictionary of archive file names for each valid year, which produces JSON file for clients.</returns>
        /// <remarks>The archive file path should be in the path from <see cref="AppConfig.__dirpath"/></remarks>
        /// <seealso cref="joulukalenteri.Client.SharedCode.ArchiveReader"/>
        public Dictionary<int,IEnumerable<string>> GetArchive() {
            if (fileSystemWrap.Directory.Exists(AppConfig.__dirpath))
            {
                int thisYear = datewrap.Now.Year;
                Regex regex = new Regex(@"^day([1-9]|1[0-9]|2[0-5])\.md$");
                Dictionary<int, IEnumerable<string>> results = new Dictionary<int, IEnumerable<string>>();
                int pathLength = fileSystemWrap.Path.GetFullPath(AppConfig.__dirpath).Length;
                string[] dirs = fileSystemWrap.Directory.GetDirectories(AppConfig.__dirpath, "*").Select(dir=>fileSystemWrap.Path.GetFullPath(dir)).ToArray();

                foreach (string dir in dirs) {
                    string dirName = dir.Substring(pathLength);
                    int year;
                    if (int.TryParse(dirName, out year) && year < thisYear)
                    {
                        results.Add(year, fileSystemWrap.Directory.GetFiles(dir).Select(str => Path.GetFileName(str)).Where(str => regex.Match(str).Success).ToArray());
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