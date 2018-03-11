
using GrainInterfaces;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Orleans;
using Orleans.Runtime.Host;
using System;
using System.Collections.Generic;
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
        static   int Main(string[] args)
           //  static async Task<int> Main(string[] args)
        {
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null,
           new AppDomainSetup()
           {
               AppDomainInitializer = InitSilo
           });

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

                var index = 0;
                //do
                //{
                //    try
                //    {
                //        Console.WriteLine("begin receive message -{0}-", index++);
                //        var onemsg = MsmqOps.ReceiveByPeek(MsmqOps.sourceQueueName);
                //        Console.WriteLine(" receive one message -{0}-", onemsg.content);
                //        var param = JsonConvert.SerializeObject(onemsg);
                //        var _tcheck = new Thread(new ParameterizedThreadStart(CompareTwoPic));
                //        _tcheck.Start(param);
                //        Console.WriteLine("haha thread start-{0}-", param);
                //    }
                //    catch(Exception ex)
                //    {
                //        Console.WriteLine("message processing error -{0}-", ex.Message);
                //    }
                //} while (index<999);
                Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
                Console.ReadLine();
                ret = mgv_destroy_engine(engine);
            }
            //   DoSomeClientWork();

            //try
            //{
            //    var host = await StartSilo();
            //    Console.WriteLine("Press Enter to terminate...");
            //    Console.ReadLine();

            //    await host.StopAsync();

            //    return 0;
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex);
            //    return 1;
            //}
            //Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            //Console.ReadLine();

            // We do a clean shutdown in the other AppDomain
          //  hostDomain.DoCallBack(ShutdownSilo);
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
        private static void CompareTwoPic(object param)
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
                MsmqOps.SendComplexMsg(MsmqOps.resultQueueName, "2", faces.id);
            }
            else if (score > WARNING_VALUE)
            {
                MsmqOps.SendComplexMsg(MsmqOps.resultQueueName, "1", faces.id);
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
