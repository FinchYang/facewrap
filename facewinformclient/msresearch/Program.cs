using System;
using System.IO;

namespace msresearch
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fs = new FileStream(args[0], FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            try
            {
                string line = sr.ReadLine(); //读取文本行
                while (line != null)
                {
                  
                    var fields = line.Split('\t');
                    var fname =args[1]+ fields[0].Replace(" ", "") + fields[1].Replace(" ", "") + fields[2].Replace(" ", "")+".jpg";
                    Console.WriteLine(fname);
                    File.WriteAllBytes(fname, Convert.FromBase64String(fields[5]));

                    line = sr.ReadLine();
                  //  if (string.IsNullOrEmpty(line)) break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                sr.Close();
                fs.Close();
            }
            Console.WriteLine("Hello World!");
        }
    }
}
