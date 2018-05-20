using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace com.baidu.ai
{
    public class GroupGetlist
    {
        // 组列表查询
        public static string Getlist(string token)
        {
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/faceset/group/getlist?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            String str = "{\"start\":0,\"length\":100}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("组列表查询:");
            Console.WriteLine(result);
            return result;
        }
    }
}
