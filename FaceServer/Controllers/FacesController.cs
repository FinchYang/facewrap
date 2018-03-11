using GrainInterfaces;
using log4net;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Orleans;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Http;

namespace FaceServer.Controllers
{
    public partial class FacesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        const double WARNING_VALUE = 73.0f;
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        public struct FaceFile
        {
            public byte[] fcontent;
            public int flen;
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

        // POST api/<controller>
        public ReturnCode Post([FromBody]CompareFaceInput input)
        {
            try
            {
                var faces = new FaceSource();
                 faces.FaceFile1 = Path.GetTempFileName();
                File.WriteAllBytes(faces.FaceFile1, Convert.FromBase64String(input.picture1));
                 faces.FaceFile2 = Path.GetTempFileName();
                File.WriteAllBytes(faces.FaceFile2, Convert.FromBase64String(input.picture2));

                //var url = HttpContext.Current.Request.Url.Host+":"+ HttpContext.Current.Request.Url.Port;
                //PushNewProject(faces, url);

                var config = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo(30000);
                GrainClient.Initialize(config);

                var friend = GrainClient.GrainFactory.GetGrain<IFaceCompare>("face haha");
                var result = friend.SayHello(faces.FaceFile1, faces.FaceFile2).Result;
              //  Console.WriteLine(result);

               // var id = Guid.NewGuid().ToString("N");
               // var content = JsonConvert.SerializeObject(faces);
               //var sendret= MsmqOps.SendComplexMsg(MsmqOps.sourceQueueName, content, id);
               // if (sendret != string.Empty)
               // {
               //     return new ReturnCode { code = -101, explanation =string.Format("msmq except={0},qname={1},content={2},id={3}",
               //         sendret ,MsmqOps.sourceQueueName, content,id)
               //     };
               // }
               // var recvret = MsmqOps.ReceiveById(MsmqOps.resultQueueName, id);
               // if (recvret.status != 0)
               // {
               //     return new ReturnCode { code = recvret.status, explanation = recvret.content };
               // }
               // var code = 0;
               // if (int.TryParse(recvret.content,out code))
               // {
               //     return new ReturnCode { code = code, explanation = recvret.content };
               // }
                return new ReturnCode { code = result, explanation = "" };
            }
            catch (Exception ex)
            {
                return new ReturnCode { code = -100, explanation = ex.Message };
            }
        }
  
        private static IHubProxy HubProxy { get; set; }
        private static HubConnection Connection { get; set; }
        private static async void PushNewProject(FaceSource mcc, string homeurl)
        {
            try
            {
                Connection = new HubConnection(homeurl);
                HubProxy = Connection.CreateHubProxy("FaceHub");
                try
                {
                    await Connection.Start();
                }
                catch (HttpRequestException hex)
                {
                    Log.Info(
                        "Unable to connect to server: Start server before connecting clients.HttpRequestException" +
                        hex.Message);
                    return;
                }
                catch (Exception ex)
                {
                    Log.Info("Unable to connect to server: Start server before connecting clients." + ex.Message);
                    return;
                }
                await HubProxy.Invoke("PushOneTask", mcc);
            }
            catch (Exception ex)
            {
                Log.Info("PushNewProject." + ex.Message);
            }
            finally
            {
                if (Connection != null)
                {
                    Connection.Stop();
                    Connection.Dispose();
                }
            }
        }
    }
}