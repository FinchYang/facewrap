﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace com.baidu.ai
{
    public class oneofnreq
    {
        public string image { get; set; }
        public string image_type { get; set; }
        public string group_id_list { get; set; }
    }
    public class FaceSearch
    {
        // 人脸搜索
        public static string search(string token, String str)
        {
            string host = "https://aip.baidubce.com/rest/2.0/face/v3/search?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "post";
            request.KeepAlive = true;
          //  String str = "{\"image\":\"027d8308a2ec665acb1bdf63e513bcb9\",\"image_type\":\"FACE_TOKEN\",\"group_id_list\":\"group_repeat,group_233\",\"quality_control\":\"LOW\",\"liveness_control\":\"NORMAL\"}";
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            Console.WriteLine("人脸搜索:");
            Console.WriteLine(result);
            return result;
        }
    }
}