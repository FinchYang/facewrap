using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    partial class Program
    {
        const double WARNING_VALUE = 73.0f;
        public struct Engine { };
      //  string dllpath = @"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll";

        [DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int mgv_set_log(int level);
        [DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int mgv_create_engine(string model_path, Engine** pengine);
        [DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static string mgv_get_error_str(int code);
        [DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int mgv_destroy_engine(Engine* engine);
        [DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int  GetFeatureFromJpeg(byte[] f1, int len1, byte[] f2, int len2);
        [DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe float         CalcFeatureSimilarity(byte[]featData1,int featLen1, byte[] featData2,int featLen2);

      
        static int Main(string[] args)
        {
            try
            {
                ClassLibrary1.Class1. CreateNewQueue();
            }
            catch (Exception ex)
            {
                Console.WriteLine("【消息队列】{0}",  ex.Message);
            }

            unsafe
            {
               // var a= new Engine();
                 Engine* engine=null;
           
            var ret = mgv_set_log(1);
            Console.WriteLine(ret);
            ret = -1;
            Console.WriteLine("haha 111");
            ret = mgv_create_engine("", &engine);
            Console.WriteLine("haha 222-{0}-",ret);
          //  Console.WriteLine("mgv_create_engine return:{0}\n", mgv_get_error_str(ret));
            Console.WriteLine("haha 333");

                FaceFile fcontent1, fcontent2;
                fcontent1 = freadAll(args[0]);
                fcontent2 = freadAll(args[1]);

                int featLen1 = 0;
                int featLen2 = 0;
                 byte[] featData1 =new byte[4096] ;
                byte[] featData2 = new byte[4096];

                // static unsigned char featData2[4096] = { 0 };

                featLen1 = GetFeatureFromJpeg(fcontent1.fcontent, fcontent1.flen, featData1, 4096*8);
                ret = ShowReturnCode(featLen1);
                if (ret <= 0)
                {
                    mgv_destroy_engine(engine);
                    return ret;
                }
                featLen2 = GetFeatureFromJpeg(fcontent2.fcontent, fcontent2.flen, featData2, 4096 * 8);
                ret = ShowReturnCode(featLen2);
                if (ret <= 0)
                {
                    mgv_destroy_engine(engine);
                    return ret;
                }

                float score = CalcFeatureSimilarity(featData1, featLen1, featData2, featLen2);
                ret = mgv_destroy_engine(engine);

                if (score <= 57.0f)
                {
                    Console.WriteLine("no\n");
                    Console.ReadLine();
                    return 1;
                }
                else if (score > WARNING_VALUE)
                {
                    Console.WriteLine("yes\n");
                    Console.ReadLine();
                    return 0;
                }
                else
                {
                    Console.WriteLine("uncerntainty ");
                    Console.ReadLine();
                    return 99;
                }
            }
           
        }
      static  FaceFile freadAll(string fname)
        {
            FaceFile ret=new FaceFile();
            ret.flen = 0;
            if(!File.Exists(fname))
            {
                Console.WriteLine("file %s not exist.\n", fname);
                return ret;
            }
           
            ret.fcontent =File.ReadAllBytes(fname);
            ret.flen = ret.fcontent.Length;
            return ret;
        }

        static int ShowReturnCode(int code)
        {
            switch (code)
            {
                case -1:
                    Console.WriteLine("interal error");
                    break;
                case -2:
                    Console.WriteLine("apply for memory error");
                    break;
                case -3:
                    Console.WriteLine("picture format error");
                    break;
                case -4:
                    Console.WriteLine("parameter error");
                    break;
                case -5:
                    Console.WriteLine("resolution ratio >1080P");
                    break;
                case -6:
                    Console.WriteLine("no face");
                    break;
                case -7:
                    Console.WriteLine("multiple face \r\n");
                    break;
                default:
                    break;
            }
            return code;
        }
        public static string SendRestHttpClientRequest(string host, string method, string param)
        {

            var url = string.Format("http://{0}/{1}", host, method);
            try
            {
                Console.WriteLine("hah 111");
                var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
                Console.WriteLine("hah 222");
                using (var http = new HttpClient(handler))
                {
                    Console.WriteLine("hah 333");
                    var content = new StringContent(param);
                    Console.WriteLine("hah 444");
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    Console.WriteLine("hah 555");
                    var response = http.PostAsync(url, content).Result;
                    Console.WriteLine("hah 666");
                    string srcString = response.Content.ReadAsStringAsync().Result;
                    return srcString;
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(new { StatusCode = "000001", Result = ex.Message });
            }
        }
    }
}
