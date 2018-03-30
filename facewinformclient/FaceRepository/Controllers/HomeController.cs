using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FaceRepository.Models;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace FaceRepository.Controllers
{
    public class HomeController : Controller
    {
        public readonly ILogger<HomeController> _log;

        public HomeController(ILogger<HomeController> log)
        {
            _log = log;
        }

        public IActionResult GetNoticeUpdatePackage(long version)
        {
            var ret = new UpdateInfo();
            try
            {
                var ppath = @"c:\installer";
              //  Log.Info("path:" + ppath);
                var di = new DirectoryInfo(ppath).GetFiles();

                foreach (FileInfo fileInfo in di)
                {
                    if (fileInfo.Name.Contains("FaceDesktop"))
                    {
                        var tmp = fileInfo.Name.Replace(".", "");
                        var reg = new Regex(@"\d+");
                        var m = reg.Match(tmp).ToString();
                        if (long.Parse(m) > version)
                        {
                            ret.Name = fileInfo.Name;
                            ret.Date = fileInfo.CreationTime.ToLocalTime().ToString("F");
                            ret.FileContent =System.IO. File.ReadAllBytes(fileInfo.FullName);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
             _log.LogError(string.Format("FaceDesktop,GetNoticeUpdatePackage error:{0}", version), ex);
            }

            return Ok(ret);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    internal class UpdateInfo
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public byte[] FileContent { get; set; }
        public UpdateInfo()
        {
        }
    }
}
