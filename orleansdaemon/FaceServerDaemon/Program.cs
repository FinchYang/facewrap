
using GrainInterfaces;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime.Host;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FaceServerDaemon
{
    partial class Program
    {
        const double WARNING_VALUE = 73.0f;
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

        private static IHubProxy HubProxy { get; set; }
        private static HubConnection Connection { get; set; }
        private static bool IsSignalrConnected = false;
        private static async void connectSignalr()
        {
            try
            {
                HubProxy = Connection.CreateHubProxy("FaceHub");
                
                HubProxy.On<string>("NewMsg", (ui) => NewMsg(      ui        )           );
                try
                {
                    await Connection.Start();
                    IsSignalrConnected = true;
                    await HubProxy.Invoke("Login", "", Connection.ConnectionId);
                }
                catch (Exception hex)
                {
                    IsSignalrConnected = false;
                    Console.WriteLine("hex--" + hex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("--ex--" + ex.Message);
            }
        }
        static SiloHost siloHost;
        private static Action<string> NewMsg(string ui)
        {
            throw new NotImplementedException();
        }

        static void CheckSignalr(string server)
        {
            do
            {
              var   ServerURI = string.Format("http://{0}/", server);
                Connection = new HubConnection(ServerURI);
                connectSignalr();

                Thread.Sleep(1000 * 6);
            } while (!IsSignalrConnected);
            //var index = 0;
            //do
            //{
            //    try
            //    {
            //        index++;
            //        if (Connection.State.Equals(Microsoft.AspNet.SignalR.Client.ConnectionState.Disconnected))
            //        {
            //            Console.WriteLine("CheckSignalr reconnecting:{0}", index) ;
            //            connectSignalr();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine("CheckSignalr reconnecting error:{0},{1}",
            //            index, ex.Message) ;
            //    }
            //    Thread.Sleep(1000 * 60 * 1);
            //} while (true);
        }


        static void InitSilo(string[] args)
        {
            siloHost = new SiloHost(System.Net.Dns.GetHostName());
            // The Cluster config is quirky and weird to configure in code, so we're going to use a config file
            siloHost.ConfigFileName = "OrleansConfiguration.xml";

            siloHost.InitializeOrleansSilo();
            var startedok = siloHost.StartOrleansSilo();
            if (!startedok)
                throw new SystemException(String.Format("Failed to start Orleans silo '{0}' as a {1} node", siloHost.Name, siloHost.Type));

        }
        //private static async Task<ISiloHost> StartSilo()
        //{
        //    // define the cluster configuration
        //    var siloPort = 11111;
        //    int gatewayPort = 30000;
        //    var siloAddress = IPAddress.Loopback;
        //    var builder = new SiloHostBuilder()
        //        //configure ClusterOptions to set CluserId and ServiceId
        //        .Configure(options => options.ClusterId = "helloworldcluster")
        //        //Configure local primary silo using DevelopmentClustering
        //        .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(siloAddress, siloPort))
        //        //Configure silo endpoint and gatewayport
        //        .ConfigureEndpoints(siloAddress, siloPort, gatewayPort)
        //        // Add assemblies to scan for grains and serializers.
        //        // For more info read the Application Parts section
        //        .ConfigureApplicationParts(parts =>
        //            parts.AddApplicationPart(typeof(HelloGrain).Assembly)
        //                 .WithReferences())
        //        // Configure logging with any logging framework that supports Microsoft.Extensions.Logging.
        //        // In this particular case it logs using the Microsoft.Extensions.Logging.Console package.
        //        .ConfigureLogging(logging => logging.AddConsole());

        //    var host = builder.Build();
        //    await host.StartAsync();
        //    return host;
        //}
        private static string GetTraceFile()
        {
            var basePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var date = DateTime.Now.Date.ToString("yy-MM-dd");
            var traceFile = basePath + "\\grainlog" + date + ".txt";
            return traceFile;
        }
        static   int Main(string[] args)
           //  static async Task<int> Main(string[] args)
        {
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null,
           new AppDomainSetup()
           {
               AppDomainInitializer = InitSilo
           });
            var traceFile = GetTraceFile();
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(traceFile));
            unsafe
            {
                Engine* engine = null;
                Trace.TraceInformation("before mgv_set_log,{0},", "");
                var ret = mgv_set_log(0);
                Trace.TraceInformation("after mgv_set_log,{0},",  ret);
                ret = mgv_create_engine("", &engine);
                Trace.TraceInformation("after mgv_create_engine,{0},", ret);
                //  Console.WriteLine("mgv_create_engine return:{0}\n", mgv_get_error_str(ret));

                
                Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
                Console.ReadLine();
                ret = mgv_destroy_engine(engine);
            }
           

            // We do a clean shutdown in the other AppDomain
            hostDomain.DoCallBack(ShutdownSilo);
            return 0;
        }
        static void ShutdownSilo()
        {
            if (siloHost != null)
            {
                siloHost.Dispose();
                GC.SuppressFinalize(siloHost);
                siloHost = null;
            }
        }
        public class FaceSource
        {
            public string FaceFile1 { get; set; }
            public string FaceFile2 { get; set; }
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
