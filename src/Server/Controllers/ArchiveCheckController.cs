﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IDirectoryWrap dirwrap;
        private readonly IDateTimeWrap datewrap;
        public ArchiveCheckController(IDirectoryWrap _dirwrap, IDateTimeWrap _datewrap)
        {
            dirwrap = _dirwrap;
            datewrap = _datewrap;
        }
        public string GetArchive() {
            if (dirwrap.Exists(AppConfig.__dirpath))
            {
                int thisYear = datewrap.Today.Year;
                Regex regex = new Regex(@"^day([1-9]|1[0-9]|2[0-5])\.md$");
                Dictionary<int, IEnumerable<string>> results = new Dictionary<int, IEnumerable<string>>();
                int pathLength = AppConfig.__dirpath.Length;
                string[] dirs = dirwrap.GetDirectories(AppConfig.__dirpath);

                foreach (string dir in dirs) {
                    string dirName = dir.Substring(pathLength);
                    int year;
                    if (int.TryParse(dirName, out year) && year < thisYear)
                    {
                        results.Add(year, dirwrap.GetFiles(dir).Where(str => regex.Match(str).Success).ToArray());
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