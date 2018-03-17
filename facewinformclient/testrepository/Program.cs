using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace testrepository
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = new RepositoryCommon.ComparedInfo
            {
                //          public string id { get; set; }
                //public string name { get; set; }
                //public string nation { get; set; }
                //public string nationality { get; set; }
                //public string address { get; set; }
                //public string idaddress { get; set; }
                //public string operatingagency { get; set; }
                //public string birthday { get; set; }
                //public string startdate { get; set; }
                //public string enddate { get; set; }
                //public byte[] idphoto { get; set; }
                //public byte[] capturephoto { get; set; }//  public string issuer { get; set; }
                id = "37900919750819723X",
                name="finch",
                nation="han",nationality="china",address="blue ocean",idaddress="long security",birthday="19750819",startdate="20001111",
                enddate="22210202",operatingagency="hahah",
        idphoto=File.ReadAllBytes(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\photo.jpg"),
                capturephoto = File.ReadAllBytes(@"C:\dev\FaceServer\facewinform\idr210sdk\desktop\photo.bmp"),
            };
            var url = string.Format("http://{0}/{1}", "localhost:50282", "PostCompared");
            try
            {
                Console.WriteLine("hah 111");
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                Console.WriteLine("hah 222");
                using (var http = new HttpClient(handler))
                {
                    Console.WriteLine("hah 333");
                    var content = new StringContent(JsonConvert.SerializeObject(input));
                    Console.WriteLine("hah 444");
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    Console.WriteLine("hah 555");
                    var response = http.PostAsync(url, content).Result;
                    Console.WriteLine("hah 666");
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(srcString);
                    Console.WriteLine(response.StatusCode);
                //    Console.WriteLine(response.);
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
