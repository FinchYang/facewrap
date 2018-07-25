using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
namespace testoneofn
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("usage: cmd picpath host11");
                Thread.Sleep(100);
                return;
            }
            var samecout = 0;
            var all = 0;
           // var dimension = 10;
            var aaa =new int[11] {0,0,0,0,0 ,0,0,0,0,0,0};
            var bbb = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //foreach (var d in new DirectoryInfo(args[0]).GetDirectories())
            //{
              //  var files = new DirectoryInfo(d.FullName).GetFiles("*_2.jpg");
            var files = new DirectoryInfo(args[0]).GetFiles("*_2.jpg");
            foreach (var f in files)
                {
                if (f.Name.CompareTo(args[2]) < 0)//|| s.Name.CompareTo(end) > 0)
                {
                    continue;
                }

                var param = Convert.ToBase64String(File.ReadAllBytes(f.FullName));
                    var cbase = f.Name.Substring(0, f.Name.IndexOf('_'));
                    // var url = string.Format("http://{0}/{1}", "192.168.0.176:5000", "oneofn");
                    var url = string.Format("http://{0}/{1}", args[1], "oneofn");
                    try
                    {
                        all++;

                        var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                        //  Console.WriteLine("hah 222");
                        using (var http = new HttpClient(handler))
                        {
                            //   Console.WriteLine("hah 333");
                            var content = new StringContent(param);
                            //  Console.WriteLine("hah 444");
                            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                            content.Headers.Add("tempid", f.Name.Replace("_2.jpg", "_1.jpg"));//oneofn
                          //  content.Headers.Add("tempid", f.Name);//purify
                            //  Console.WriteLine("hah 555");
                            var response = http.PostAsync(url, content).Result;
                            //   Console.WriteLine("hah 666");
                            string srcString = response.Content.ReadAsStringAsync().Result;
                            
                            if (srcString.Contains(cbase))
                            {
                                samecout++;
                                Console.WriteLine("true--{0}--{1},{2},{3}", f.Name, srcString, samecout, all);
                                var sreg = @"(?<=score=)0\.[\d]+";
                                var sm = Regex.Match(srcString, sreg);
                                if (sm.Success)
                                {
                                    var score = float.Parse(sm.Value);
                                    if (score > 0.5) bbb[0]++;
                                    else if (score > 0.45) bbb[1]++;
                                    else if (score > 0.4) bbb[2]++;
                                    else if (score > 0.35) bbb[3]++;
                                    else if (score > 0.3) bbb[4]++;

                                    else if (score > 0.25) bbb[5]++;
                                    else if (score > 0.2) bbb[6]++;
                                    else if (score > 0.15) bbb[7]++;
                                    else if (score > 0.1) bbb[8]++;
                                    else bbb[9]++;
                                }
                                else bbb[10]++;
                            }
                            else
                                Console.WriteLine("false--{0}--{1},{2},{3}", f.Name, srcString, samecout, all);

                            var reg = @"(?<=index=)[\d]+";
                            var m = Regex.Match(srcString, reg);
                            if (m.Success)
                            {
                                var score = int.Parse(m.Value);
                                if (score < 9) aaa[score]++;
                                else aaa[9]++;
                            }
                            else aaa[10]++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(new { StatusCode = f.Name, Result = ex.Message });
                    }
                }
          //  }
            for(int i=0;i<11;i++)
            {
                Console.Write("{0}--{1}, ", i, aaa[i]);
            }
            Console.WriteLine("");
            for (int i = 0; i < 11; i++)
            {
                Console.Write("{0}--{1}, ", i, bbb[i]);
            }
            // Console.ReadLine();
        }
    }
}
