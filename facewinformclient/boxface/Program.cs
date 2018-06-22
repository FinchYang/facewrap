using Baidu.Aip.Face;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace boxface
{
    class Program
    {
        private static bool ThumbnailCallback()
        {
            return false;
        }


        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        public static void GenerateHighThumbnail(string oldImagePath, string newImagePath, int width, int height)
        {
            System.Drawing.Image oldImage = System.Drawing.Image.FromFile(oldImagePath);//oldImage.Save();
            //int newWidth = AdjustSize(width, height, oldImage.Width, oldImage.Height).Width;
            //int newHeight = AdjustSize(width, height, oldImage.Width, oldImage.Height).Height;
            //。。。。。。。。。。。
            // System.Drawing.Image thumbnailImage = oldImage.GetThumbnailImage(newWidth, newHeight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            System.Drawing.Image thumbnailImage = oldImage.GetThumbnailImage(width, height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(thumbnailImage);
            //处理JPG质量的函数
            System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
            if (ici != null)
            {
                using (System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters(1))
                {
                    ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                    bm.Save(newImagePath, ici, ep);
                    //释放所有资源，不释放，可能会出错误。
                    //ep.Dispose();
                    //ep = null;
                }
            }
            ici = null;
            bm.Dispose();
            bm = null;
            thumbnailImage.Dispose();
            thumbnailImage = null;
            oldImage.Dispose();
            oldImage = null;
        }
        // 设置APPID/AK/SK
        static string APP_ID = "11425770-";
        static string API_KEY = "MT094ThGMBcu0P2MUGidm81X"; // 13345158191
        static string SECRET_KEY = "QQmRQggCRYYW5UVhXMN6iQrtChdVaGdo"; // 13345158191

        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务///18521561581
        //  private static String clientId = "ZvrDVb6ndf478tEGEsn6seWu";
        // 百度云中开通对应服务应用的 Secret Key
        //   private static String clientSecret = "Y0SGApzLOFbyLPEoTqVWBNtSpPUfCAvW";

        // 13345158191
        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        private static String clientId = "MT094ThGMBcu0P2MUGidm81X";
        // 百度云中开通对应服务应用的 Secret Key
        private static String clientSecret = "QQmRQggCRYYW5UVhXMN6iQrtChdVaGdo";
        //

        /* 2849978760,15921124834*/
        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        //  private static String clientId = "rcCArR8S4zWdbz7fSGqocFeB";
        // 百度云中开通对应服务应用的 Secret Key
        //  private static String clientSecret = "1MZAYFpx8QhdNFap8PbPM3q9WOwwIiVq";

        // HjdLAeGmhlIxweeSCOggjqBk 18106385083
        // private static String clientId = "HjdLAeGmhlIxweeSCOggjqBk";
        // 百度云中开通对应服务应用的 Secret Key
        //  private static String clientSecret = "64aYytEj8V2FkqQ2cPnGF8fwh2XDRNyF";

        // HjdLAeGmhlIxweeSCOggjqBk 13708919431
        //  private static String clientId = "5GoFKjiWGDYo4WyCGZZHA8l6";
        // 百度云中开通对应服务应用的 Secret Key
        //  private static String clientSecret = "pPUD4YoQZLGYhFcBN8ahwyeUaF4GDLQw";
        public static void CaptureImage(string fromImagePath, int offsetX, int offsetY, string toImagePath, int width, int height)
        {
            //原图片文件
            Image fromImage = Image.FromFile(fromImagePath);
            var tempx = offsetX - 10;
            var x = tempx > 0 ? tempx : 0;
            var tempy = offsetY - 15;
            var y = tempy > 0 ? tempy : 0;

            var tempw = width + 20 + x;
            var w = tempw < fromImage.Width ? width + 20 : fromImage.Width - x;

            var temph = height + 30 + y;
            var h = temph < fromImage.Height ? height + 30 : fromImage.Height - y;

            //创建新图位图
            Bitmap bitmap = new Bitmap(w, h);
            //创建作图区域
            Graphics graphic = Graphics.FromImage(bitmap);
            //截取原图相应区域写入作图区
          

            graphic.DrawImage(fromImage, 0, 0, new Rectangle(x, y, w, h), GraphicsUnit.Pixel);
            //从作图区生成新图
            Image saveImage = Image.FromHbitmap(bitmap.GetHbitmap());
            //保存图片
            saveImage.Save(toImagePath, ImageFormat.Png);
            //释放资源   
            saveImage.Dispose();
            graphic.Dispose();
            bitmap.Dispose();
            fromImage.Dispose();
        }
        public static void DetectDemo(Face client, string file, string newfile,int pix)
        {
            var image = Convert.ToBase64String(File.ReadAllBytes(file));// "取决于image_type参数，传入BASE64字符串或URL字符串或FACE_TOKEN字符串";
            
            var imageType = "BASE64";
            try
            {
                // 调用人脸检测，可能会抛出网络等异常，请使用try/catch捕获
                var result = client.Detect(image, imageType);
                //  Console.WriteLine(result);
                // 如果有可选参数
                //   var options = new Dictionary<string, object>{
                // {"face_field", "age,beauty,glasses,expression,faceshape,gender,race,quality,facetype"},
                //   {"face_field", "age,beauty,glasses,race,faceshape,gender,facetype"},
                // {"max_face_num", 1},
                //   {"face_type", "LIVE"}
                // };
                // 带参数调用人脸检测
                // var result = client.Detect(image, imageType, options);
                //  Console.WriteLine(result);
                var e = result.GetValue("error_code");
                //  Console.WriteLine("error_code=" + e);
                if (e.ToString() == "0")
                {
                    var f = result.GetValue("result");
                    //  Console.WriteLine("result=" + f);
                    var fl = f.ToObject<facelist>();
                    if (fl.face_num > 0)
                    {
                        //  Console.WriteLine("face 0=" + fl.face_list.ToArray()[0].location.top);
                        var loc = fl.face_list.ToArray()[0].location;
                        var tempf = Path.GetTempFileName();
                        CaptureImage(file, (int)loc.left, (int)loc.top, tempf, loc.width, loc.height);
                        GenerateHighThumbnail(tempf, newfile, pix, pix);
                    }
                }
                else if (e.ToString() == "18")
                {
                    Thread.Sleep(400);
                    DetectDemo(client, file, newfile, pix);
                }
                else Console.WriteLine(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                Thread.Sleep(400);
                DetectDemo(client, file, newfile, pix);
            }
        }
        public class facelist
        {
            public short face_num { set; get; }
            public List<face> face_list { set; get; }
        }
        public class face
        {
            public string face_token { set; get; }
            public location location { set; get; }
        }
        public class location
        {
            public int width { set; get; }
            public int height { set; get; }
            public double left { set; get; }
            public double top { set; get; }
        }
        public class errcode
        {
            public long error_code { set; get; }
        }
        static void Main(string[] args)
        {            
            var source = args[0];
            var target = args[1];
            var pixels = int.Parse(args[2]);
            var stop = new Stopwatch();
            var client = new Baidu.Aip.Face.Face(API_KEY, SECRET_KEY);
            client.Timeout = 6000;  // 修改超时时间
            var sublist = new DirectoryInfo(source).GetDirectories();
          //  var first = true;
            foreach(var s in sublist)
            {
                var files = s.GetFiles("*.jpg");
                stop.Restart();
                foreach(var f in files)
                {
                    var p = Path.Combine(target, s.Name);
                    if (!Directory.Exists(p)) Directory.CreateDirectory(p);
                    var newf = Path.Combine(p, f.Name);
                    DetectDemo(client, f.FullName, newf,pixels);
                    Console.WriteLine("ok! {0},{1}",newf,f.Name);
                    //if(first)
                    //{
                    //    first = false;
                    //    Thread.Sleep(400);
                    //}
                }
                stop.Stop();
                if (stop.ElapsedMilliseconds < 1000)
                {
                    Console.WriteLine("sleep! {0}", stop.ElapsedMilliseconds);
                    Thread.Sleep(1000 -(int) stop.ElapsedMilliseconds);
                }
                else Console.WriteLine("elapsed! {0}", stop.ElapsedMilliseconds);
            }
           
            Console.WriteLine("Hello World!");
        }
    }
}
