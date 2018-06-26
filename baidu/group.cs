﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace com.baidu.ai
{
    public class GroupAdd
    {
        // 创建用户组
        public static string groupAdd(string token,string group)
        {
           // string token = "[调用鉴权接口获取的token]";
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/faceset/group/add?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
            String str = "{\"group_id\":\""+group+"\"}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("创建用户组:");
            Console.WriteLine(result);
            return result;
        }
    }
}