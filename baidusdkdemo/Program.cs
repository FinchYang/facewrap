using System;
using Baidu.Aip.Face;
namespace baidusdkdemo
{
    class Program
    {
        // 设置APPID/AK/SK
      static  string APP_ID = "11425770";
        static string API_KEY = "MT094ThGMBcu0P2MUGidm81X";
        static string SECRET_KEY = "QQmRQggCRYYW5UVhXMN6iQrtChdVaGdo";

        public void DetectDemo(Face client)
        {
            var image = "取决于image_type参数，传入BASE64字符串或URL字符串或FACE_TOKEN字符串";

            var imageType = "BASE64";

            // 调用人脸检测，可能会抛出网络等异常，请使用try/catch捕获
            var result = client.Detect(image, imageType);
            Console.WriteLine(result);
            // 如果有可选参数
            var options = new Dictionary<string, object>{
        {"face_field", "age"},
        {"max_face_num", 2},
        {"face_type", "LIVE"}
    };
            // 带参数调用人脸检测
            result = client.Detect(image, imageType, options);
            Console.WriteLine(result);
        }
        static void Main(string[] args)
        {
         

            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 60000;  // 修改超时时间

            Console.WriteLine("Hello World!");
        }
    }
}
