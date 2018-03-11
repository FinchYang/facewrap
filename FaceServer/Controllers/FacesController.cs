using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;

namespace FaceServer.Controllers
{
    public class FacesController : ApiController
    {
   
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
                var f1 = Path.GetTempFileName();
                File.WriteAllBytes(f1, Convert.FromBase64String(input.picture1));
                var f2 = Path.GetTempFileName();
                File.WriteAllBytes(f2, Convert.FromBase64String(input.picture2));

                FaceFile fcontent1, fcontent2;
                fcontent1 = freadAll(f1);
                fcontent2 = freadAll(f2);

                int featLen1 = 0;
                int featLen2 = 0;
                byte[] featData1 = new byte[4096];
                byte[] featData2 = new byte[4096];

                var ret = 0;

                //featLen1 = GetFeatureFromJpeg(fcontent1.fcontent, fcontent1.flen, featData1, 4096 * 8);
                //ret = ShowReturnCode(featLen1);
                //if (ret <= 0)
                //{
                //    return new ReturnCode { code = ret, explanation = "other error" };
                //}
                //featLen2 = GetFeatureFromJpeg(fcontent2.fcontent, fcontent2.flen, featData2, 4096 * 8);
                //ret = ShowReturnCode(featLen2);
                //if (ret <= 0)
                //{
                //    return new ReturnCode { code = ret, explanation = "other error" };
                //}

                //float score = CalcFeatureSimilarity(featData1, featLen1, featData2, featLen2);

                //if (score <= 57.0f)
                //{
                //    return new ReturnCode { code = 1, explanation = "not one person" };
                //}
                //else if (score > WARNING_VALUE)
                //{
                //    return new ReturnCode { code = 0, explanation = "same person" };
                //}
                //else
                //{
                //    return new ReturnCode { code = 99, explanation = "uncerntainty" };
                //}
                return new ReturnCode { code = 0, explanation = "same person" };
            }
            catch (Exception ex)
            {
                return new ReturnCode { code = -100, explanation = ex.Message };
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}