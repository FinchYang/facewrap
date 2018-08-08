using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Bface.Controllers
{
    [Route("api/[controller]")]
    
    public class face2facev4Controller : Controller
    {
        public readonly ILogger<face2facev4Controller> _log;
        public face2facev4Controller(ILogger<face2facev4Controller> log)
        {
            _log = log;
        }
        //  [HttpPost]
        //public void _Post111()
        //{
        //    StreamReader reader = new StreamReader(Request.Body);
        //    String xmlData = reader.ReadToEnd();
        //    Console.WriteLine(xmlData);
        //    //string strRequestData = "接收到的请求数据是：" + xmlData;
        //    //string strReponseData = "要返回的数据是：" + "<root><attReaderName>1</attReaderName></root>";
        //    string strReponseData = "";
        //    strReponseData += "<root>" +
        //               "<head>" +
        //               "<errorCode>0</errorCode>" +
        //               "<result>success</result>" +
        //               "</head>" +
        //               "</root>";

        //  //  Response.Body.w(strReponseData);
        //}
        //[HttpPost]
        //public HttpResponseMessage Post()
        //{
            //var request = HttpContext.Current.Request;
            //HttpResponseMessage result = null;
           
            //    if (request.Files.Count == 0)
            //    {
            //        result = Request.CreateResponse(HttpStatusCode.BadRequest);
            //    }
            //    var resultFiles = new List<DatabaseFile>();
            //    using (var connection = sqlConnectionFactory.Create())
            //    using (var transaction = connection.BeginTransaction())
            //    {
            //        for (var i = 0; i < request.Files.Count; i++)
            //        {
            //            var postedFile = request.Files[i];
            //            var id = fileRepository.AddFile(postedFile.InputStream, postedFile.FileName, postFile.OrderId,
            //                                            postFile.RootFolderName, connection, transaction);
            //            resultFiles.Add(fileRepository.GetInfo(id, connection, transaction));
            //        }
            //        transaction.Commit();
            //    }
            //    result = Request.CreateResponse(HttpStatusCode.Created, resultFiles);
           

        //    return result;
        //}
        [HttpPost]
      //  [Route("PostCompared")]
      
      //  public async Task<IActionResult> Post([FromBody] ComparedInfo trails)//{"similar":0.436878175,"result":true,"errorinfo":""}
          //  public async Task<IActionResult> Post([FromBody] string trails)
              public async Task<IActionResult> Post()//[FromForm] ComparedInfo trails)
        {
            StreamReader reader = new StreamReader(Request.Body);
            String xmlData = reader.ReadToEnd();
          //  var reg = @"(?<=faceimage1=).*(?=&faceimage2)";
            var reg = @"(?<=faceimage2=).*";
            var m = Regex.Match(xmlData, reg);
            if (m.Success)
            {
              //  System.IO.File.WriteAllBytes("image111.jpg", Convert.FromBase64String(m.Value));
                Console.WriteLine("------------ match");
                //  var score = double.Parse(m.Value);
            }
            else Console.WriteLine("no match");

              var reg11 = @"(?<=faceimage1=).*(?=&faceimage2)";
            var m11 = Regex.Match(xmlData, reg11);
            if (m11.Success)
            {
               // System.IO.File.WriteAllBytes("image222.jpg", Convert.FromBase64String(m11.Value));
                Console.WriteLine("------222------ match");
                //  var score = double.Parse(m.Value);
            }
            else Console.WriteLine("no 22 match");
            //Console.WriteLine(xmlData);
            //Console.WriteLine("=---{0}", JsonConvert.SerializeObject( trails));
            // return BadRequest("ajkd");
            var trails = new ComparedInfo { faceimage1 = m11.Value, faceimage2 = m.Value };
            //   
            //    //   System.IO.File.WriteAllBytes("image2.jpg", Convert.FromBase64String(trails.faceimage2));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _log.LogDebug("{0}", 2222);
            try
            {
                if (expiretime.CompareTo(DateTime.Now) <= 0)
                {
                    var to = getAccessToken();
                    if (!to.ok)
                    {
                        return Ok(new commonresponse { result = false, errorinfo = "getAccessToken 失败，请稍后再试" });
                    }
                }
                var req = new List<matchreq>();
                req.Add(new matchreq
                {
                    // image = Convert.ToBase64String(File.ReadAllBytes(idimage)),
                    image = trails.faceimage1,
                    face_type = "LIVE",
                    image_type = "BASE64",
                    quality_control = "NONE",
                    liveness_control = "NONE",
                });
                req.Add(new matchreq
                {
                    // image = Convert.ToBase64String(File.ReadAllBytes(capture)),
                    image = trails.faceimage2,
                    face_type = "LIVE",
                    image_type = "BASE64",
                    quality_control = "NONE",
                    liveness_control = "NONE",
                });

                var bm = match(access_token, JsonConvert.SerializeObject(req));
                var bret = JsonConvert.DeserializeObject<matchresponse>(bm);
                if (bret.error_code == 0)
                {
                    if (bret.result.score > 70)
                    {
                        //  MessageBox.Show("比对成功，是同一个人！");
                        // needReadId = true;
                       // return Ok(new commonresponse { similar = bret.result.score / 100 - 0.10f, result = true, errorinfo = "" });
                        return Ok(new commonresponse { similar = bret.result.score / 100 , result = true, errorinfo = "" });
                    }
                    else
                    {
                        // Console.WriteLine("not ok-" + ret.result.score);
                       // return Ok(new commonresponse { similar = bret.result.score / 100 - 0.10f, result = false, errorinfo = "相似度低" });
                        return Ok(new commonresponse { similar = bret.result.score / 100 , result = false, errorinfo = "相似度低" });
                    }
                }
                else // byun error ,local compare
                {
                    //  Console.WriteLine(ret.error_code + ret.error_msg);
                    return Ok(new commonresponse { result = false, errorinfo = "比对 失败，请稍后再试" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        DateTime expiretime = DateTime.Now;
        string access_token = string.Empty;
        /* ,15921124834*/
        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        private static String clientId = "rcCArR8S4zWdbz7fSGqocFeB";
        // 百度云中开通对应服务应用的 Secret Key
        private static String clientSecret = "1MZAYFpx8QhdNFap8PbPM3q9WOwwIiVq";
        public class tokenret
        {
            public string access_token { get; set; }
            public bool ok { get; set; }
        }
        public class tokenreponse
        {
            public string access_token { get; set; }
            public string session_key { get; set; }
            public string scope { get; set; }
            public string refresh_token { get; set; }
            public string session_secret { get; set; }
            public string expires_in { get; set; }
        }
        public tokenret getAccessToken()
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;

            var re = new tokenret { ok = false, access_token = result };
            if (result.Contains("access_token"))
            {
                var ret = JsonConvert.DeserializeObject<tokenreponse>(result);
                re.ok = true;
                re.access_token = ret.access_token;
                access_token = ret.access_token;
                expiretime = DateTime.Now.AddSeconds(long.Parse(ret.expires_in) - 10);
            }
            //   Console.WriteLine(result);
            return re;
        }
        public class matchreq
        {
            public string image { get; set; }
            public string image_type { get; set; }
            public string face_type { get; set; }
            public string quality_control { get; set; }
            public string liveness_control { get; set; }
        }
        public static string match(string token, string req)
        {
            //  var re = new matchresponse();
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/match?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            //   String str = "[{\"image\":\"sfasq35sadvsvqwr5q...\",\"image_type\":\"BASE64\",\"face_type\":\"LIVE\",\"quality_control\":\"LOW\",\"liveness_control\":\"HIGH\"},{\"image\":\"sfasq35sadvsvqwr5q...\",\"image_type\":\"BASE64\",\"face_type\":\"IDCARD\",\"quality_control\":\"LOW\",\"liveness_control\":\"HIGH\"}]";
            byte[] buffer = encoding.GetBytes(req);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();

            Console.WriteLine("人脸对比:");
            Console.WriteLine(result);
            return result;
        }
        public class matchresponse
        {
            public int error_code { get; set; }
            public scoreret result { get; set; }
            public string error_msg { get; set; }
        }
        public class scoreret
        {
            public float score { get; set; }
        }
        int cloudc(string idimage, string capture, string id)
        {
            try
            {
                if (expiretime.CompareTo(DateTime.Now) <= 0)
                {
                    var to = getAccessToken();
                    if (!to.ok)
                    {
                        // goto localc;
                        return -2;
                    }
                }
                var req = new List<matchreq>();
                req.Add(new matchreq
                {
                   // image = Convert.ToBase64String(File.ReadAllBytes(idimage)),
                    face_type = "IDCARD",
                    image_type = "BASE64",
                    quality_control = "NONE",
                    liveness_control = "NONE",
                });
                req.Add(new matchreq
                {
                   // image = Convert.ToBase64String(File.ReadAllBytes(capture)),
                    face_type = "LIVE",
                    image_type = "BASE64",
                    quality_control = "NONE",
                    liveness_control = "NONE",
                });

                var bm = match(access_token, JsonConvert.SerializeObject(req));
                var bret = JsonConvert.DeserializeObject<matchresponse>(bm);
                if (bret.error_code == 0)
                {
                    if (bret.result.score > 80)
                    {
                        //  MessageBox.Show("比对成功，是同一个人！");
                        // needReadId = true;
                        return 1;
                    }
                    else
                    {
                        // Console.WriteLine("not ok-" + ret.result.score);
                        return 0;
                    }
                }
                else // byun error ,local compare
                {
                    //  Console.WriteLine(ret.error_code + ret.error_msg);
                    return -1;
                }
            }
            catch (Exception ex)
            {
                return -3;
            }

        
        }
        public class ComparedInfo
        {
            public string faceimage1 { get; set; }
            public string faceimage2 { get; set; }
            //public string nation { get; set; }
            //public string nationality { get; set; }
            //public string address { get; set; }
            //public string idaddress { get; set; }
            //public string operatingagency { get; set; }
            //public string issuer { get; set; }
            //public string gender { get; set; }
            //public string birthday { get; set; }
            //public string startdate { get; set; }
            //public string enddate { get; set; }
            //public byte[] idphoto { get; set; }
            //public byte[] capturephoto { get; set; }
        }
        public class PictureInfo
        {
            public byte[] base64pic { get; set; }
        }
        public class PictureInfoNoid
        {
            public byte[] base64pic1 { get; set; }
            public byte[] base64pic2 { get; set; }
            public byte[] base64pic3 { get; set; }
        }
        public class commonresponse//{"similar":0.436878175,"result":true,"errorinfo":""}
        {
            public float similar { get; set; }
            public bool result { get; set; }
            public string errorinfo { get; set; }
        }
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

      

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
