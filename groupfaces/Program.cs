using System;
using System.IO;

namespace groupfaces
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = new DirectoryInfo(args[0]).GetFiles("*_1.jpg");
            var index = 0;
            foreach(var f in files)
            {
                var fbase = f.Name.Replace("_1.jpg", "");
                var f2 = f.FullName.Replace("_1.jpg", "_2.jpg");
                if (File.Exists(f2))
                {
                    var subpath = Path.Combine(args[1], fbase);
                    Directory.CreateDirectory(subpath);
                    File.Move(f.FullName, Path.Combine(subpath, f.Name));
                    File.Move(f2, Path.Combine(subpath, f.Name.Replace("_1.jpg", "_2.jpg")));
                }
                index++;
                Console.WriteLine(index+"Hello World!"+fbase);
            }
           
        }
    }
}
