using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace testfaces
{
    class Program
    {
        //const string sofile = "face_recognition.dll";


        ////  [DllImport(sofile, CallingConvention = CallingConvention.Cdecl)]
        //[DllImport(sofile)]
        //public extern static string compare(string file1, string file2);
        public enum StatusCode
        {
            UNKNOWN = -99,
            OK = 0, ENGINE_ERROR = -11, NOT_ONE_PERSON = 1, UNCERTAINTY = 99,
            MGV_ERR = -1,
            MGV_MALLOC_ERR = -2,
            MGV_IMAGE_FORMAT_ERR = -3,
            MGV_PARA_ERR = -4,
            MGV_IMAGE_OUT_OF_RANGE = -5,
            MGV_NO_FACE_DETECTED = -6,
            MGV_MULTIPLE_FACES_DETECTED = -7,
        }
        public class CompareFaceInput
        {
            public string picture1 { get; set; }
            public string picture2 { get; set; }
        }
        public class SmartCompareFaceInput
        {
          //  public string id { get; set; }
            public string faceimage1 { get; set; }
            public string faceimage2 { get; set; }
        }
        public class ReturnCode
        {
            public int code { get; set; }
            public string explanation { get; set; }
        }

     
        //public static ReturnCode cloudCompare(string FaceFile1, string FaceFile2)
        //{
        //    var ret = compare(FaceFile1, FaceFile2);
        //    var reg = @"(?<=terminate)0\.[\d]{4,}";
        //    var m = Regex.Match(ret, reg);
        //    var result = -100;
        //    if (m.Success)
        //    {
        //        var score = double.Parse(m.Value);
        //        if (score > 0.74)
        //        {
        //            result = 1;
        //        }
        //        else result = 2;
        //    }
        //    else result = -1;

        //    return new ReturnCode { code = result, explanation = "" };
        //}
        static void Main(string[] args)
        {
            //  Console.WriteLine(cloudCompare(args[0], args[1]).code);
            var param = new SmartCompareFaceInput {
               // id = args[2]
            };
            param.faceimage1 = Convert.ToBase64String(File.ReadAllBytes(args[0]));
            param.faceimage2 = Convert.ToBase64String(File.ReadAllBytes(args[1]));

            // var param = new List<CompareFaceInput>();
            //  var url = string.Format("http://{0}/{1}", "192.168.0.132:5001", "api/values");
           // var url = string.Format("http://{0}/{1}", "192.168.0.161:5001", "smartfacesCompare");
           // var url = string.Format("http://{0}/{1}", "app.ytjj.gov.cn/ZDY_LDKSRLSB", "smartfacesCompare");
            var url = string.Format("http://localhost:{0}/{1}", args[2], "api/face2facev4?appid=ba35e64b0dea032f6ba6a36c134bf3d1");
            //  var url = string.Format("http://{0}/{1}", "localhost:801", "api/faces");
            try
            {
                Console.WriteLine("hah 111");
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                Console.WriteLine("hah 222");
                using (var http = new HttpClient(handler))
                {
                    Console.WriteLine("hah 333");
                  // var content = new StringContent(JsonConvert.SerializeObject(param));
                    var content = new StringContent("faceimage1=" + param.faceimage1 + "&faceimage2=" + param.faceimage2 );
                  //  var content = new StringContent("faceimage1=" + "111" + "&faceimage2=" + "222");
                    Console.WriteLine("hah 444");
                  // content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
                    Console.WriteLine("hah 555");
                    var response = http.PostAsync(url, content).Result;
                    Console.WriteLine("hah 666");
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(srcString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(new { StatusCode = "000001", Result = ex.Message });
            }
           // Console.ReadLine();
        }
      
    }
}
