using System;
using System.Text.RegularExpressions;

namespace facedata
{
    class Program
    {
       
        static void Main(string[] args)
        {
            int count65 = 0;
            int count68 = 0;
            int count71 = 0;
            int count74 = 0;
            int count62 = 0;
            int count77 = 0;
            double count = 0;
          var files=  System.IO.Directory.GetFiles(args[0]);
            foreach(var f in files)
            {
                if (f.Contains("_1.jpg"))
                {
                    var a = new System.Diagnostics.Process();
                    a.StartInfo.UseShellExecute = false;
                    a.StartInfo.RedirectStandardOutput = true;
                    a.StartInfo.CreateNoWindow = true;
                    a.StartInfo.FileName = "compare.exe";
                    a.StartInfo.Arguments = string.Format(" \"{0}\"  \"{1}\"", f, f.Replace("_1.jpg", "_2.jpg"));
                    a.Start();
                    var output = a.StandardOutput.ReadToEnd();
                    a.WaitForExit();
                    var ret = a.ExitCode;
                    var reg = @"(?<=terminate)0\.[\d]{4,}";
                    var m = Regex.Match(output, reg);
                    count++;
                    if (m.Success)
                    {
                        var score = double.Parse(m.Value);                        
                        if (score > 0.77) count77++;
                        if (score > 0.74) count74++;
                        if (score > 0.71) count71++;
                        if (score > 0.68) count68++;
                        if (score > 0.65) count65++;
                        if (score > 0.62) count62++;
                        Console.WriteLine("score={0},file={1},{2},77={3},74={4},71={5},68={6},65={7},62={8},count={9}",
                             score, f, "", count77, count74 , count71 , count68 , count65, count62, count);
                    }
                    else Console.WriteLine("error");
                }
            }
            Console.WriteLine("相似度大于77%的占比：{0}", count77*100.0 / count);
            Console.WriteLine("相似度大于74%的占比：{0}", count74 * 100.0 / count);
            Console.WriteLine("相似度大于71%的占比：{0}", count71 * 100.0 / count);
            Console.WriteLine("相似度大于68%的占比：{0}", count68 * 100.0 / count);
            Console.WriteLine("相似度大于65%的占比：{0}", count65 * 100.0 / count);
            Console.WriteLine("相似度大于62%的占比：{0}", count62 * 100.0 / count);
        }
     

    }
}
