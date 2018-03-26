using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace createbase64
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                return;
            }

            Console.WriteLine(args[0]);
            System.IO.File.WriteAllText(args[0]+".txt",Convert.ToBase64String(System.IO.File.ReadAllBytes(args[0])));
        }
    }
}
