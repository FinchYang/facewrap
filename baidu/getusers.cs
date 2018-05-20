using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace com.baidu.ai
{
    public class GroupGetusers
    {
        // 获取用户列表
        public static string getUsers(string token, string group)
        {
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/faceset/group/getusers?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
           // String str = "{\"group_id\":\"group1\"}";
            String str = "{\"group_id\":\"" + group + "\"}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("获取用户列表:");
            Console.WriteLine(result);
            return result;
        }
    }
}
