using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace createbase64
{
    class Program
    {
        static void Main(string[] args)
        {
            // var s=  Assembly.LoadFrom(@"C:\Users\Administrator\Downloads\windows_c_sdk_x64_small_440hard_release_20180323\windows_c_sdk_x64_small_440hard_release_20180323\lib\x64\core_sdk.dll");
            var s = Assembly.LoadFrom(@"F:\dev\FaceServer\OCX\ClassLibrary1\bin\Debug\ClassLibrary1.dll");
            foreach (var a in s.ExportedTypes)
            {
                Console.WriteLine("exported type,{0}", a.FullName);
            }
            foreach (var b in s.DefinedTypes)
            {
                Console.WriteLine("DefinedTypes type,{0}", b.FullName);
            }
            Console.ReadLine();
            
            //if (args.Length < 1)
            //{
            //    return;
            //}

            //Console.WriteLine(args[0]);
            //System.IO.File.WriteAllText(args[0]+".txt",Convert.ToBase64String(System.IO.File.ReadAllBytes(args[0])));
        }
    }
}
