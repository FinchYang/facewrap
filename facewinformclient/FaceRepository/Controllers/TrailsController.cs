using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using FaceRepository.dbmodel;
using FaceRepository.Models;
using RepositoryCommon;
using Microsoft.Extensions.Logging;
using FaceRepository.dbmodel;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.InteropServices;

namespace FaceRepository.Controllers
{
    [Produces("application/json")]
    //  [Route("api/Trails")]
    public partial class TrailsController : Controller
    {
        public readonly ILogger<TrailsController> _log;
        const string sofile = "dnn_face_recognition64.dll";


        //  [DllImport(sofile, CallingConvention = CallingConvention.Cdecl)]
        [DllImport(sofile)]
        public extern static string compare(string file1, string file2);
        public TrailsController(ILogger<TrailsController> log)
        {
            _log = log;
        }
        public class ReturnCode
        {
            public int code { get; set; }
            public string explanation { get; set; }
        }
        public class CompareFaceInput
        {
            public string picture1 { get; set; }
            public string picture2 { get; set; }
        }
       // [HttpPost]
        [Route("testcompare")]
        public ReturnCode testcompare()
        {
            try
            {
                var FaceFile1 = "222.jpg";
                var FaceFile2 = "233.jpg";

                return new ReturnCode { code = SmartCompare(FaceFile1, FaceFile2) ? 1 : 0, explanation = "" };
            }
            catch (Exception ex)
            {
                return new ReturnCode { code = -100, explanation = ex.Message };
            }
        }
        [HttpPost]
        [Route("cloudCompare")]
        public ReturnCode cloudCompare([FromBody]CompareFaceInput input)
        {
            try
            {
                var FaceFile1 = Path.GetTempFileName() + ".jpg";
                System.IO.File.WriteAllBytes(FaceFile1, Convert.FromBase64String(input.picture1));
                var FaceFile2 = Path.GetTempFileName() + ".jpg";
                System.IO.File.WriteAllBytes(FaceFile2, Convert.FromBase64String(input.picture2));
                //_log.LogInformation("before compare,{0},{1}", FaceFile1, FaceFile2);
                //var ret = compare(FaceFile1, FaceFile2);
                //_log.LogInformation("after compare");
                //var reg = @"(?<=terminate)0\.[\d]{4,}";
                //var m = Regex.Match(ret, reg);
                //var result = -100;
                //if (m.Success)
                //{
                //    var score = double.Parse(m.Value);
                //    if (score > 0.74)
                //    {
                //        result = 1;
                //    }
                //    else result = 2;
                //}
                //else result = -1;

                return new ReturnCode { code = SmartCompare(FaceFile1, FaceFile2)?1:0, explanation = "" };
            }
            catch (Exception ex)
            {
                return new ReturnCode { code = -100, explanation = ex.Message };
            }
        }
        bool SmartCompare(string f1,string f2)
        {
            var a = new System.Diagnostics.Process();

            a.StartInfo.UseShellExecute = false;
            a.StartInfo.RedirectStandardOutput = true;
            a.StartInfo.CreateNoWindow = true;
            a.StartInfo.FileName =  "compare.exe";
            a.StartInfo.Arguments = string.Format(" \"{0}\"  \"{1}\"", f1, f2);
            a.Start();
            var output = a.StandardOutput.ReadToEnd();
            a.WaitForExit();
            var ret = a.ExitCode;

            var reg = @"(?<=terminate)0\.[\d]{4,}";
            var m = Regex.Match(output, reg);
            if (m.Success)
            {
                var score = double.Parse(m.Value);
                // labelscore.Text = ((int)(score * 100)).ToString() + "%";
                if (score > 0.74)
                {
                    return true;
                }
            }
            return false;
        }
        [HttpPost]
        [Route("PostCompared")]
        public async Task<IActionResult> PostCompared([FromBody] ComparedInfo trails)
        {
             _log.LogDebug("{0}",111111);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
              _log.LogDebug("{0}", 2222);
            try
            {
                using (var db = new dbmodel.faceContext())
                {
                         _log.LogDebug("{0}", 3333);
                    var person = db.Person.FirstOrDefault(c => c.Idcardno == trails.id);
                    if (person == null)
                    {
                        db.Person.Add(new Person
                        {
                            Idcardno = trails.id,
                            Name = trails.name,
                            Nation = trails.nation,
                            Nationality = trails.nationality,
                            Birthday = trails.birthday,
                            Address = trails.idaddress,
                            Startdate = trails.startdate,
                            Enddate = trails.enddate,
                            Gender = trails.gender,
                            Issuer = trails.issuer,
                            Info = new PictureInfo { base64pic = trails.idphoto },
                        });
                    }
                    _log.LogDebug("{0}", Convert.ToBase64String(trails.capturephoto).Take(30));
                    _log.LogDebug("{0}", Convert.ToBase64String(trails.capturephoto).Skip(3000).Take(30));
                    _log.LogDebug("{0}", Convert.ToBase64String(trails.capturephoto).TakeLast(30));
                    db.Trails.Add(new Trails
                    {
                        TimeStamp = DateTime.Now,
                        Address = trails.address,
                        Idcardno = trails.id,
                        Operatingagency = trails.operatingagency,
                        Info = new PictureInfo { base64pic = trails.capturephoto },
                    });
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok(new commonresponse { status = 0, explanation = "ok" });
        }

    }
}