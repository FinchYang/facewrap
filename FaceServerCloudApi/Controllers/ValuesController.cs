using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;


namespace FaceServer.Controllers
{
    public class ValuesController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        // GET api/values
        public IEnumerable<string> Get()
        {
            Log.Error("haha error");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public ReturnCode Post([FromBody]CompareFaceInput input)
        {
            try
            {
                var f1 = Path.GetTempFileName();
                File.WriteAllBytes(f1, Convert.FromBase64String(input.picture1));
                var f2 = Path.GetTempFileName();
                File.WriteAllBytes(f2, Convert.FromBase64String(input.picture2));

                var a = new System.Diagnostics.Process();
                a.StartInfo.UseShellExecute = true;
                a.StartInfo.WorkingDirectory = @"C:\dev\ocx\TestOCX\ConsoleApplication1";
                a.StartInfo.Arguments = string.Format(" {0} {1}", f1, f2);
                a.StartInfo.FileName = @"C:\dev\ocx\TestOCX\ConsoleApplication1\aaa.exe";
                a.Start();
                a.WaitForExit();
                var ret = a.ExitCode;
                // if(Enum.TryParse<StatusCode>)
                return new ReturnCode { code = ret, explanation = "" };
            }
            catch(Exception ex)
            {
                return new ReturnCode { code = -100, explanation = ex.Message };
            }
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
