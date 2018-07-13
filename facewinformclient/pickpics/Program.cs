using System;
using System.IO;
namespace pickpics
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = args[0];
            var count = int.Parse(args[1]);
            var files = new DirectoryInfo(source).GetFiles("*_2.jpg");
            var index = 0;
            foreach(var f in files)
            {
                var f1 = f.FullName.Replace("2.jpg", "1.jpg");
                if (File.Exists(f1))
                {
                    var tpath1 = "1pic"+args[1];
                    if (!Directory.Exists(tpath1)) Directory.CreateDirectory(tpath1);
                    File.Copy(f1, Path.Combine(tpath1, f.Name.Replace("2.jpg", "1.jpg")));

                    var tpath2 = "2pic"+args[1];
                    if (!Directory.Exists(tpath2)) Directory.CreateDirectory(tpath2);
                    File.Copy(f.FullName, Path.Combine(tpath2, f.Name));
                    index++;
                }
                if (index > count) break;
            }
            Console.WriteLine("Hello World!");
        }
    }
}
