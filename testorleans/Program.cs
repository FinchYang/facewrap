using GrainInterfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testorleans
{
    class Program
    {
     
        static void Main(string[] args)
        {
            DoSomeClientWork();
            Console.ReadLine();
        }
        static void DoSomeClientWork()
        {
            // Orleans comes with a rich XML and programmatic configuration. Here we're just going to set up with basic programmatic config
            var config = Orleans.Runtime.Configuration.ClientConfiguration.LocalhostSilo(30000);
            GrainClient.Initialize(config);

            var friend = GrainClient.GrainFactory.GetGrain<IFaceCompare>("");
            var result = friend.SayHello("Goodbye","ha").Result;
            Console.WriteLine(result);

        }
    }
}
