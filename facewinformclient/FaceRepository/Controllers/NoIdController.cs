using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FaceRepository.dbmodel;
using FaceRepository.Models;
using RepositoryCommon;
using Microsoft.Extensions.Logging;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace FaceRepository.Controllers
{
    [Produces("application/json")]
    public partial class NoIdController : Controller
    {
        private string FtpServerIP = "localhost";//, 
        private string FtpUserID = "user";//FtpUserID, FtpPassword
        private string FtpPassword = "pass";//FtpUserID, FtpPassword
        public readonly ILogger<NoIdController> _log;

        public NoIdController(ILogger<NoIdController> log)
        {
            _log = log;
        }


        //#region 上传文件到FTP服务器
        /// <summary>
        /// 上传文件到FTP服务器
        /// </summary>
        /// <param name="localFullPath">本地带有完整路径的文件名</param>
        /// <param name="updateProgress">报告进度的处理(第一个参数：总大小，第二个参数：当前进度)</param>
        /// <returns>是否下载成功</returns>
        public bool FtpUploadFile(string localFullPathName, Action<int, int> updateProgress = null)
        {
            FtpWebRequest reqFTP;
            Stream stream = null;
            FtpWebResponse response = null;
            FileStream fs = null;
            try
            {
                FileInfo finfo = new FileInfo(localFullPathName);
                if (FtpServerIP == null || FtpServerIP.Trim().Length == 0)
                {
                    throw new Exception("ftp上传目标服务器地址未设置！");
                }
                Uri uri = new Uri("ftp://" + FtpServerIP + "/" + finfo.Name);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(uri);
                reqFTP.KeepAlive = false;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(FtpUserID, FtpPassword);//用户，密码
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;//向服务器发出下载请求命令
                reqFTP.ContentLength = finfo.Length;//为request指定上传文件的大小
                response = reqFTP.GetResponse() as FtpWebResponse;
                reqFTP.ContentLength = finfo.Length;
                int buffLength = 1024;
                byte[] buff = new byte[buffLength];
                int contentLen;
                fs = finfo.OpenRead();
                stream = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                int allbye = (int)finfo.Length;
                //更新进度 
                if (updateProgress != null)
                {
                    updateProgress((int)allbye, 0);//更新进度条 
                }
                int startbye = 0;
                while (contentLen != 0)
                {
                    startbye = contentLen + startbye;
                    stream.Write(buff, 0, contentLen);
                    //更新进度 
                    if (updateProgress != null)
                    {
                        updateProgress((int)allbye, (int)startbye);//更新进度条 
                    }
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                stream.Close();
                fs.Close();
                response.Close();
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
        }
        [HttpPost]
        [Route("NoidUpload")]
        public async Task<IActionResult> NoidUpload([FromBody] NoidInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                using (var db = new dbmodel.faceContext())
                {
                    db.Noidlog.Add(new Noidlog
                    {
                        Idcardno = input.id,
                        Capturephoto = new PictureInfo { base64pic = input.pic1 }
                    });
                    await db.SaveChangesAsync();
                }
                var temp = Path.Combine(Path.GetTempPath(), input.id);
                System.IO.File.WriteAllText(temp, JsonConvert.SerializeObject(input));
                FtpUploadFile(temp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok(new commonresponse { status = 0, explanation = "ok" });
        }
        [HttpGet]
        [Route("NoidUpload")]
        public async Task<IActionResult> GetNoidResult([FromBody] NoidResultInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                using (var db = new dbmodel.faceContext())
                {
                    var result = db.Noidlog.Where(c => c.Businessnumber == input.businessnumber && c.Compared == 1 && c.Notified == 0);
                    var rl = new List<NoidResult>();
                    foreach (var a in result)
                    {
                        a.Notified = 1;
                        rl.Add(new NoidResult
                        {
                            id = a.Idcardno,
                            status = (CompareResult)a.Result
                        });
                        db.Noidloghis.Add(new Noidloghis
                        {
                            Idcardno = a.Idcardno,
                            Capturephoto = a.Capturephoto,
                            Compared = a.Compared.Value,
                            Result = a.Result.Value,
                            Businessnumber = a.Businessnumber,
                            Stamp = DateTime.Now
                        });
                        db.Noidlog.Remove(a);
                    }
                    await db.SaveChangesAsync();
                    return Ok(rl);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}