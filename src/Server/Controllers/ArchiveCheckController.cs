using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemWrapper;
using SystemWrapper.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace joulukalenteri.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArchiveCheckController : ControllerBase
    {
        private readonly IDirectoryWrap _dirwrap;
        public ArchiveCheckController(IDirectoryWrap dirwrap)
        {
            _dirwrap = dirwrap;
        }
        public string GetArchive() {
            Regex regex = new Regex(@"day([1-9]|1[0-9]|2[0-5])\.md");
            if (_dirwrap.Exists(AppConfig.__dirpath))
            {
                Dictionary<string, string[]> results = new Dictionary<string, string[]>();
                int pathLength = AppConfig.__dirpath.Length;
                string[] dirs = _dirwrap.GetDirectories(AppConfig.__dirpath);

                foreach (string dir in dirs) {
                    string dirName = dir.Substring(pathLength);
                    if (dirName.All(char.IsDigit))
                    {
                        results.Add(dirName, _dirwrap.GetFiles(dir).Where(str => regex.Match(str).Success).ToArray());
                    }
                }
                return JsonConvert.SerializeObject(results);
            }
            else
            {
                return "";
            }
        }
    }
}