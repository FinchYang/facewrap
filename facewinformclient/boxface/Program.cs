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
            try { 
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
            catch (Exception ex)
            {
                Console.Error.WriteLine("exception in GenerateHighThumbnail:" + ex.Message);
                Console.WriteLine("exception in GenerateHighThumbnail:" + ex.Message);
            }
        }
        // 设置APPID/AK/SK
        //static string APP_ID = "11425770-";
        //static string API_KEY = "MT094ThGMBcu0P2MUGidm81X"; // 13345158191
        //static string SECRET_KEY = "QQmRQggCRYYW5UVhXMN6iQrtChdVaGdo"; // 13345158191

       // static string API_KEY = "ZvrDVb6ndf478tEGEsn6seWu"; // 18521561581
       // static string SECRET_KEY = "Y0SGApzLOFbyLPEoTqVWBNtSpPUfCAvW"; // 18521561581

     //  static string API_KEY = "rcCArR8S4zWdbz7fSGqocFeB"; // 15921124834
     //   static string SECRET_KEY = "1MZAYFpx8QhdNFap8PbPM3q9WOwwIiVq"; // 15921124834

     //   static string API_KEY = "HjdLAeGmhlIxweeSCOggjqBk"; // 18106385083
      //  static string SECRET_KEY = "64aYytEj8V2FkqQ2cPnGF8fwh2XDRNyF"; // 18106385083

       // static string API_KEY = "5GoFKjiWGDYo4WyCGZZHA8l6"; // 13708919431
       //static string SECRET_KEY = "pPUD4YoQZLGYhFcBN8ahwyeUaF4GDLQw"; // 13708919431

        static string[] API_KEY = { "5GoFKjiWGDYo4WyCGZZHA8l6", "HjdLAeGmhlIxweeSCOggjqBk",
            "rcCArR8S4zWdbz7fSGqocFeB" ,"ZvrDVb6ndf478tEGEsn6seWu","MT094ThGMBcu0P2MUGidm81X"}; // 13708919431
        static string[] SECRET_KEY = { "pPUD4YoQZLGYhFcBN8ahwyeUaF4GDLQw", "64aYytEj8V2FkqQ2cPnGF8fwh2XDRNyF" ,
            "1MZAYFpx8QhdNFap8PbPM3q9WOwwIiVq" ,"Y0SGApzLOFbyLPEoTqVWBNtSpPUfCAvW", "QQmRQggCRYYW5UVhXMN6iQrtChdVaGdo"}; // 13708919431

        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务///18521561581
        //  private static String clientId = "ZvrDVb6ndf478tEGEsn6seWu";
        // 百度云中开通对应服务应用的 Secret Key
        //   private static String clientSecret = "Y0SGApzLOFbyLPEoTqVWBNtSpPUfCAvW";

        // 13345158191
        // 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
        // private static String clientId = "MT094ThGMBcu0P2MUGidm81X";
        // 百度云中开通对应服务应用的 Secret Key
        //  private static String clientSecret = "QQmRQggCRYYW5UVhXMN6iQrtChdVaGdo";
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
            try
            {
                //原图片文件
                Image fromImage = Image.FromFile(fromImagePath);
                var xmargin = 10;
                var ymargin = 15;
                var tempx = offsetX - xmargin;
                var x = tempx > 0 ? tempx : 0;
                var tempy = offsetY - ymargin;
                var y = tempy > 0 ? tempy : 0;

                var tempw = width + xmargin*2 + x;
                var w = tempw < fromImage.Width ? width + xmargin*2 : fromImage.Width - x;

                var temph = height + ymargin*2 + y;
                var h = temph < fromImage.Height ? height + ymargin*2 : fromImage.Height - y;

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
            catch(Exception ex)
            {
                Console.Error.WriteLine("exception in capture:" + ex.Message);
                Console.WriteLine("exception in capture:" + ex.Message);
            }
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
                  Console.WriteLine(result);
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
                        if (Math.Abs(loc.rotation) > 9)
                        {
                            Console.Error.WriteLine("{0},rotation={1}",file , loc.rotation);
                            return;
                        }
                        var angle = fl.face_list.ToArray()[0].angle;
                        if (Math.Abs(angle.yaw) > 9)
                        {
                            Console.Error.WriteLine("{0},yaw={1}", file, angle.yaw);
                            return;
                        }
                        if (Math.Abs(angle.pitch) > 9)
                        {
                            Console.Error.WriteLine("{0},pitch={1}", file, angle.pitch);
                            return;
                        }
                        if (Math.Abs(angle.roll) > 9)
                        {
                            Console.Error.WriteLine("{0},roll={1}", file, angle.roll);
                            return;
                        }
                        var tempf = Path.GetTempFileName();
                        CaptureImage(file, (int)loc.left, (int)loc.top, tempf, loc.width, loc.height);
                        GenerateHighThumbnail(tempf, newfile, pix, pix);
                        File.Delete(tempf);
                    }
                }
                else if (e.ToString() == "18")
                {
                    Console.WriteLine(result);
                  //  Console.Error.WriteLine(result);
                    Thread.Sleep(400);
                    DetectDemo(client, file, newfile, pix);
                }
                else Console.Error.WriteLine(file+result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
              //  Console.Error.WriteLine("exception in main:"+ex);
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
            public angle angle { set; get; }
        }
        public class angle
        {
            public double yaw { set; get; }
            public double pitch { set; get; }//rotation
            public double roll { set; get; }
        }
        public class location
        {
            public int width { set; get; }
            public int height { set; get; }
            public double left { set; get; }
            public double top { set; get; }//rotation
            public int rotation { set; get; }
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
            var sourcefile = args[3];
           // var last =int.Parse( args[3]);
              var keynum = int.Parse(args[4])-1;
            //  var stop = new Stopwatch();
            var client = new Baidu.Aip.Face.Face(API_KEY[keynum], SECRET_KEY[keynum]);
            client.Timeout = 6000;  // 修改超时时间
                                    // var index = 0;
                                    //var sublist = new DirectoryInfo(source).GetDirectories();
                                    //foreach (var s in sublist)
                                    //{              
                                    //  //  stop.Restart();
                                    //    var files = s.GetFiles("*_2.jpg");
                                    //    foreach (var f in files)
                                    //    {
                                    //        if (f.Name.Replace("_2.jpg", "").CompareTo(last) < 0)//|| s.Name.CompareTo(end) > 0)
                                    //        {
                                    //            continue;
                                    //        }
                                    //        var f2 = f.FullName.Replace("_2.jpg", "_1.jpg");
                                    //       // Path.Combine(args[2], f.Name.Replace("_2.jpg", "_1.jpg"));
                                    //        if (File.Exists(f2))
                                    //        {
                                    //            var p = Path.Combine(target, f.Name.Replace("_2.jpg", ""));
                                    //            if (!Directory.Exists(p)) Directory.CreateDirectory(p);
                                    //            var newf = Path.Combine(p, f.Name);
                                    //            DetectDemo(client, f.FullName, newf, pixels);
                                    //            Console.WriteLine("ok! {0},{1}", newf, f.Name);

            //            var newf2 = Path.Combine(p, f.Name.Replace("_2.jpg", "_1.jpg"));
            //            DetectDemo(client, f2, newf2, pixels);
            //            Console.WriteLine("ok! {0},{1}", newf2, f.Name.Replace("_2.jpg", "_1.jpg"));
            //            index++;
            //            GC.Collect(3, GCCollectionMode.Forced);
            //        }
            //        else
            //        {
            //            //   Console.WriteLine(f.Name + rret.error_code + rret.error_msg);
            //            Console.Error.WriteLine(f.Name + "--no 1.jpg");
            //        }
            //        if (index > 4800) {
            //            Console.WriteLine("I want to rest.");
            //            break; }
            //        //var p = Path.Combine(target, s.Name);
            //        //if (!Directory.Exists(p)) Directory.CreateDirectory(p);
            //        //var newf = Path.Combine(p, f.Name);
            //        //DetectDemo(client, f.FullName, newf, pixels);
            //        //Console.WriteLine("ok! {0},{1}", newf, f.Name);
            //        //if(first)
            //        //{
            //        //    first = false;
            //        //    Thread.Sleep(400);
            //        //}
            //    }
            //    //stop.Stop();
            //    //if (stop.ElapsedMilliseconds < 1010)
            //    //{
            //    //    Console.WriteLine("sleep! {0}", stop.ElapsedMilliseconds);
            //    //    Thread.Sleep(1000 - (int)stop.ElapsedMilliseconds);
            //    //}
            //    //else Console.WriteLine("elapsed! {0}", stop.ElapsedMilliseconds);
            //}
            var stop = new Stopwatch();
            stop.Start();
            Console.WriteLine("before getfiles");
          //  var files2 = new DirectoryInfo(source).GetFiles("*_2.jpg");
            stop.Stop();
            //  var stop = new Stopwatch();
            // var timeindex = 0;
          //  var splitindex = 0;
            //var max = (last + 1) * 4800;
            //var min = last * 4800;
            //  Console.WriteLine("begin,min={0},max-{1},{2}", min, max,stop.ElapsedMilliseconds);
            var fff = File.ReadAllLines(sourcefile);
            foreach (var ff in fff)
            {
               
                //if (f.Name.Replace("_2.jpg", "").CompareTo(last) < 0)//|| s.Name.CompareTo(end) > 0)
                //{
                //    continue;
                //}
                //if(splitindex>max)
                //{
                //    break;
                //}
                //if (splitindex++ <min)
                //{
                //    continue;
                //}
                try
                {
                    //var f2 = f.FullName.Replace("_2.jpg", "_1.jpg");// Path.Combine(args[2], f.Name.Replace("_2.jpg", "_1.jpg"));
                    //if (File.Exists(f2))
                    //{

                        // stop.Restart();
                        var p = Path.Combine(target, ff);
                        if (!Directory.Exists(p)) Directory.CreateDirectory(p);
                    {
                        var file1 = ff + "_1.jpg";
                        var sourcepic = Path.Combine(source, file1);

                        var newf = Path.Combine(p, file1);
                        DetectDemo(client, sourcepic, newf, pixels);
                        Console.WriteLine("ok! {0},{1}", newf, sourcepic);
                    }
                    {
                        var file2 = ff + "_2.jpg";
                        var sourcepic2 = Path.Combine(source, file2);
                        var newf2 = Path.Combine(p, file2);
                        DetectDemo(client, sourcepic2, newf2, pixels);
                        Console.WriteLine("ok! {0},{1}", newf2, sourcepic2);
                    }
                    //stop.Stop();
                    //if (stop.ElapsedMilliseconds < 1010)
                    //{
                    //    Console.WriteLine("sleep! {0}", stop.ElapsedMilliseconds);
                    //    Thread.Sleep(1010 - (int)stop.ElapsedMilliseconds);
                    //}
                    //else Console.WriteLine("elapsed! {0}", stop.ElapsedMilliseconds);
                    // index++;
                    // }
                    //else
                    //{
                    //    //   Console.WriteLine(f.Name + rret.error_code + rret.error_msg);
                    //    Console.Error.WriteLine(f.Name + "--no 1.jpg");
                    //}
                    //if (index > 4800)
                    //{
                    //    Console.WriteLine(f.Name+"---I want to rest.");
                    //    break;
                    //}
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                    Console.WriteLine(ex);
                }
            }
           // Console.WriteLine("ok,min={0},max-{1}",min,max);
        }
    }
}
