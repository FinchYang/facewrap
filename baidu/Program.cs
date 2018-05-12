using com.baidu.ai;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace baidu
{
    class Program
    {
        static void Main(string[] args)
        {
            var to = AccessToken.getAccessToken();
            if (to.ok)
            {
                var req = new List<matchreq>();
                req.Add(new matchreq
                {
                    image=Convert.ToBase64String(File .ReadAllBytes(args[0])),
                    face_type= "IDCARD",
                    image_type= "BASE64",
                    quality_control= "NONE",
                    liveness_control= "NONE",
                });
                req.Add(new matchreq
                {
                    image = Convert.ToBase64String(File.ReadAllBytes(args[1])),
                    face_type = "LIVE",
                    image_type = "BASE64",
                    quality_control = "NONE",
                    liveness_control = "NONE",
                });
               var m= FaceMatch.match(to.access_token, JsonConvert.SerializeObject(req));
                var ret = JsonConvert.DeserializeObject<matchresponse>(m);
                if (ret.error_code == 0)
                {
                    if (ret.result.score > 80)
                    {
                        Console.WriteLine("ok" + ret.result.score);
                    }
                    else
                    {
                        Console.WriteLine("not ok-" + ret.result.score);
                    }
                }
                else
                {

                    Console.WriteLine(ret.error_code+ ret.error_msg);
                }
            }
        }
    }
}
