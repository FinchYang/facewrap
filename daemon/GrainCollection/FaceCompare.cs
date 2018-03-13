using GrainInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GrainCollection
{
    partial class FaceCompare : Orleans.Grain, IFaceCompare
    {
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
        public Task<int> SayHello(string file1, string file2)
        {
            var traceFile = GetTraceFile();
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(traceFile));
            Trace.TraceError("SayHello,{0},{1}", file1, file2);

            return Task.FromResult(CompareTwoPic(file1, file2));
        }
        public struct FaceFile
        {
            public byte[] fcontent;
            public int flen;
        }
        const double WARNING_VALUE = 73.0f;
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
        private static string GetTraceFile()
        {
            var basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var date = DateTime.Now.Date.ToString("yy-MM-dd");
            var traceFile = basePath + "\\grainlog" + date + ".txt";
            return traceFile;
        }
        private static int CompareTwoPic(string FaceFile1, string FaceFile2)
        {
            FaceFile fcontent1, fcontent2;
            fcontent1 = freadAll(FaceFile1);
            fcontent2 = freadAll(FaceFile2);

            int featLen1 = 0;
            int featLen2 = 0;
            byte[] featData1 = new byte[4096];
            byte[] featData2 = new byte[4096];

            var traceFile = GetTraceFile();
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(traceFile));

            var ret = -1;
            featLen1 = GetFeatureFromJpeg(fcontent1.fcontent, fcontent1.flen, featData1, 4096 * 8);
            ret = ShowReturnCode(featLen1);
            if (ret <= 0)
            {
                Trace.TraceError("GetFeatureFromJpeg,{0},{1}", FaceFile1, ret);
                return ret;
            }
            featLen2 = GetFeatureFromJpeg(fcontent2.fcontent, fcontent2.flen, featData2, 4096 * 8);
            ret = ShowReturnCode(featLen2);
            if (ret <= 0)
            {
                Trace.TraceError("GetFeatureFromJpeg,{0},{1}", FaceFile2, ret);
                return ret;
            }

            float score = CalcFeatureSimilarity(featData1, featLen1, featData2, featLen2);
            GC.Collect(0, GCCollectionMode.Forced);
            if (score <= 57.0f)
            {
                return 2;
            }
            else if (score > WARNING_VALUE)
            {
                return 1;
            }
            else
            {
                return 99;
            }
        }
    }
}
