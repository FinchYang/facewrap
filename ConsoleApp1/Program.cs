using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static int mgv_set_log(int level);
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int mgv_create_engine(string model_path, Engine** pengine);
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static string mgv_get_error_str(int code);
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int mgv_destroy_engine(Engine* engine);
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe int GetFeatureFromJpeg(byte[] f1, int len1, byte[] f2, int len2);
        [DllImport(@"core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        public extern static unsafe float CalcFeatureSimilarity(byte[] featData1, int featLen1, byte[] featData2, int featLen2);

        //[DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static int mgv_set_log(int level);
        //[DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static unsafe int mgv_create_engine(string model_path, Engine** pengine);
        //[DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static string mgv_get_error_str(int code);
        //[DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static unsafe int mgv_destroy_engine(Engine* engine);
        //[DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static unsafe int GetFeatureFromJpeg(byte[] f1, int len1, byte[] f2, int len2);
        //[DllImport(@"E:\BaiduNetdiskDownload\windows_c_sdk_x64_small_440hard_release_20180306\windows_c_sdk_x64_small_440hard_release_20180306\exe\core_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
        //public extern static unsafe float CalcFeatureSimilarity(byte[] featData1, int featLen1, byte[] featData2, int featLen2);
        public class result
        {
            public bool ok { get; set; }
            public float score { get; set; }
            public CompareStatus status { get; set; }
            public mgverror errcode            { get; set; }
        }
        public enum CompareStatus
        {
            unkown,success,failure,uncertainty
        }

        public enum mgverror
        {
            unkown,
            MGV_ERR = -1,
            MGV_MALLOC_ERR = -2,
            MGV_IMAGE_FORMAT_ERR = -3,
            MGV_PARA_ERR = -4,
            MGV_IMAGE_OUT_OF_RANGE = -5,
            MGV_NO_FACE_DETECTED = -6,
            MGV_MULTIPLE_FACES_DETECTED = -7,
        }
        static void Main(string[] args)
        {
            var ret = new result { ok = true, score = 0, status = CompareStatus.unkown ,errcode=mgverror.unkown};
            unsafe
            {
                Engine* engine = null;

                mgv_set_log(1);
                mgv_create_engine("", &engine);
                //  Console.WriteLine("mgv_create_engine return:{0}\n", mgv_get_error_str(ret));

                FaceFile fcontent1, fcontent2;
                fcontent1 = freadAll(args[0]);
                fcontent2 = freadAll(args[1]);

                int featLen1 = 0;
                int featLen2 = 0;
                byte[] featData1 = new byte[4096];
                byte[] featData2 = new byte[4096];

                featLen1 = GetFeatureFromJpeg(fcontent1.fcontent, fcontent1.flen, featData1, 4096 * 8);
               var  mgvret = ShowReturnCode(featLen1);
                if (mgvret <= 0)
                {
                    mgv_destroy_engine(engine);
                    ret.errcode =(mgverror) mgvret;
                    ret.ok = false;
                    File.WriteAllText("compareresult.txt", JsonConvert.SerializeObject(ret));
                    return ;
                }
                featLen2 = GetFeatureFromJpeg(fcontent2.fcontent, fcontent2.flen, featData2, 4096 * 8);
                mgvret = ShowReturnCode(featLen2);
                if (mgvret <= 0)
                {
                    mgv_destroy_engine(engine);
                    ret.errcode = (mgverror)mgvret;
                    ret.ok = false;
                    File.WriteAllText("compareresult.txt", JsonConvert.SerializeObject(ret));
                    return;
                }

                float score = CalcFeatureSimilarity(featData1, featLen1, featData2, featLen2);
                mgvret = mgv_destroy_engine(engine);
                ret.score = score;
                if (score <= 57.0f)
                {
                    ret.status = CompareStatus.failure;
                    File.WriteAllText("compareresult.txt", JsonConvert.SerializeObject(ret));
                    return;
                }
                else if (score > WARNING_VALUE)
                {
                    ret.status = CompareStatus.success;
                    File.WriteAllText("compareresult.txt", JsonConvert.SerializeObject(ret));
                    return;
                }
                else
                {
                    ret.status = CompareStatus.uncertainty;
                    File.WriteAllText("compareresult.txt", JsonConvert.SerializeObject(ret));
                    return;
                }
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
