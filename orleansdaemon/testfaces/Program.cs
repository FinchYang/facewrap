using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace testfaces
{
    class Program
    {
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
        static void Main(string[] args)
        {
            var param = new CompareFaceInput();
            param.picture1 = Convert.ToBase64String(File.ReadAllBytes(args[0]));
            param.picture2 = Convert.ToBase64String(File.ReadAllBytes(args[1]));
            var url = string.Format("http://{0}/{1}", "localhost:58070", "api/faces");
          //  var url = string.Format("http://{0}/{1}", "localhost:801", "api/faces");
            try
            {
                Console.WriteLine("hah 111");
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                Console.WriteLine("hah 222");
                using (var http = new HttpClient(handler))
                {
                    Console.WriteLine("hah 333");
                    var content = new StringContent(JsonConvert.SerializeObject(param));
                    Console.WriteLine("hah 444");
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    Console.WriteLine("hah 555");
                    var response = http.PostAsync(url, content).Result;
                    Console.WriteLine("hah 666");
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine( srcString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(new { StatusCode = "000001", Result = ex.Message });
            }
            Console.ReadLine();
        }
      
    }
}
