using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FaceServerDaemon
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
        public extern static unsafe int GetFeatureFromJpeg(byte[] f1, int len1, byte[] f2, int len2);
        [DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe float CalcFeatureSimilarity(byte[] featData1, int featLen1, byte[] featData2, int featLen2);


        static int Main(string[] args)
        {
            try
            {
                var qret=MsmqOps.CreateNewQueue(MsmqOps.sourceQueueName);
                if (qret != string.Empty)
                {
                    return -124;
                }
                 qret = MsmqOps.CreateNewQueue(MsmqOps.resultQueueName);
                if (qret != string.Empty)
                {
                    return -125;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【消息队列】{0}", ex.Message);
                return -123;
            }

            unsafe
            {
                Engine* engine = null;

                var ret = mgv_set_log(1);
                Console.WriteLine(ret);
                ret = -1;
                Console.WriteLine("haha 111");
                ret = mgv_create_engine("", &engine);
                Console.WriteLine("haha 222-{0}-", ret);
                //  Console.WriteLine("mgv_create_engine return:{0}\n", mgv_get_error_str(ret));

                do
                {
                   var onemsg= MsmqOps.ReceiveByPeek(MsmqOps.sourceQueueName);
                    var param = JsonConvert.SerializeObject(onemsg);
                    var _tcheck = new Thread( new ParameterizedThreadStart(Checkmflogin));
                    _tcheck.Start(param);
                    Console.WriteLine("haha thread start-{0}-", param);
                } while (true);
            }
        }
        public class FaceSource
        {
            public string FaceFile1 { get; set; }
            public string FaceFile2 { get; set; }
        }
        private static void Checkmflogin(object param)
        {
            var faces = JsonConvert.DeserializeObject<mqreturn>(param.ToString());
            var twoface = JsonConvert.DeserializeObject<FaceSource>(faces.content);
            FaceFile fcontent1, fcontent2;
            fcontent1 = freadAll(twoface.FaceFile1);
            fcontent2 = freadAll(twoface.FaceFile2);

            int featLen1 = 0;
            int featLen2 = 0;
            byte[] featData1 = new byte[4096];
            byte[] featData2 = new byte[4096];

            var ret = -1;
            featLen1 = GetFeatureFromJpeg(fcontent1.fcontent, fcontent1.flen, featData1, 4096 * 8);
            ret = ShowReturnCode(featLen1);
            if (ret <= 0)
            {
                MsmqOps.SendComplexMsg(MsmqOps.resultQueueName, ret.ToString(), faces.id);
            }
            featLen2 = GetFeatureFromJpeg(fcontent2.fcontent, fcontent2.flen, featData2, 4096 * 8);
            ret = ShowReturnCode(featLen2);
            if (ret <= 0)
            {
                MsmqOps.SendComplexMsg(MsmqOps.resultQueueName, ret.ToString(), faces.id);
            }

            float score = CalcFeatureSimilarity(featData1, featLen1, featData2, featLen2);
            if (score <= 57.0f)
            {
                MsmqOps.SendComplexMsg(MsmqOps.resultQueueName, "1", faces.id);
            }
            else if (score > WARNING_VALUE)
            {
                MsmqOps.SendComplexMsg(MsmqOps.resultQueueName, "0", faces.id);
            }
            else
            {
                MsmqOps.SendComplexMsg(MsmqOps.resultQueueName, "99", faces.id);
            }
        }

        static FaceFile freadAll(string fname)
        {
            FaceFile ret = new FaceFile();
            ret.flen = 0;
            if (!File.Exists(fname))
            {
                Console.WriteLine("file %s not exist.\n", fname);
                return ret;
            }

            ret.fcontent = File.ReadAllBytes(fname);
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
    }
}
