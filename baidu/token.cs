
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace com.baidu.ai
{
   public class tokenreponse {
        public string access_token { get; set; }
        public string session_key { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string session_secret { get; set; }
        public string expires_in { get; set; }
    }
    public class tokenret
    {
        public string access_token { get; set; }
        public bool ok { get; set; }
    }
    public static class AccessToken

    {
        static string[] API_KEY = { "5GoFKjiWGDYo4WyCGZZHA8l6", "HjdLAeGmhlIxweeSCOggjqBk",
            "rcCArR8S4zWdbz7fSGqocFeB" ,"ZvrDVb6ndf478tEGEsn6seWu","MT094ThGMBcu0P2MUGidm81X"}; // 13708919431
        static string[] SECRET_KEY = { "pPUD4YoQZLGYhFcBN8ahwyeUaF4GDLQw", "64aYytEj8V2FkqQ2cPnGF8fwh2XDRNyF" ,
            "1MZAYFpx8QhdNFap8PbPM3q9WOwwIiVq" ,"Y0SGApzLOFbyLPEoTqVWBNtSpPUfCAvW", "QQmRQggCRYYW5UVhXMN6iQrtChdVaGdo"}; // 13708919431

        // 调用getAccessToken()获取的 access_token建议根据expires_in 时间 设置缓存
        // 返回token示例
        //   public static String TOKEN = "24.adda70c11b9786206253ddb70affdc46.2592000.1493524354.282335-1234567";

        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务///18521561581
        //   private static String clientId = "ZvrDVb6ndf478tEGEsn6seWu";
        // 百度云中开通对应服务应用的 Secret Key
        //  private static String clientSecret = "Y0SGApzLOFbyLPEoTqVWBNtSpPUfCAvW";

        // 13345158191
        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        //  private static String clientId = "MT094ThGMBcu0P2MUGidm81X";
        // 百度云中开通对应服务应用的 Secret Key
        //  private static String clientSecret = "QQmRQggCRYYW5UVhXMN6iQrtChdVaGdo";
        //

        /* 2849978760,15921124834*/
        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        private static String clientId = "rcCArR8S4zWdbz7fSGqocFeB";
        // 百度云中开通对应服务应用的 Secret Key
       private static String clientSecret = "1MZAYFpx8QhdNFap8PbPM3q9WOwwIiVq";

        // HjdLAeGmhlIxweeSCOggjqBk 18106385083
        //  private static String clientId = "HjdLAeGmhlIxweeSCOggjqBk";
        //  // 百度云中开通对应服务应用的 Secret Key
        // private static String clientSecret = "64aYytEj8V2FkqQ2cPnGF8fwh2XDRNyF";

        // HjdLAeGmhlIxweeSCOggjqBk 13708919431
        //  private static String clientId = "5GoFKjiWGDYo4WyCGZZHA8l6";
        // 百度云中开通对应服务应用的 Secret Key
        // private static String clientSecret = "pPUD4YoQZLGYhFcBN8ahwyeUaF4GDLQw";
        public static tokenret getAccessToken(int index)
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", API_KEY[index]));
            paraList.Add(new KeyValuePair<string, string>("client_secret", SECRET_KEY[index]));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;

            var re = new tokenret { ok = false, access_token = result };
            if (result.Contains("access_token"))
            {
                var ret = JsonConvert.DeserializeObject<tokenreponse>(result);
                re.ok = true;
                re.access_token = ret.access_token;
            }
            Console.WriteLine(result);
            return re;
        }
        public static tokenret getAccessToken()
        {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;

            var re = new tokenret { ok = false,access_token=result };
            if (result.Contains("access_token"))
            {
                var ret = JsonConvert.DeserializeObject<tokenreponse>(result);
                re.ok = true;
                re.access_token = ret.access_token;
            }
            Console.WriteLine(result);
            return re;
        }
    }
}
