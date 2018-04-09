using GrainInterface;
using System;
using System.Threading.Tasks;

namespace GrainCollection
{
    public class HelloGrain : Orleans.Grain, IHello
    {
        Task<string> SayHello(string greeting)
        {
            return Task.FromResult($"You said: '{greeting}', I say: Hello!");
        }
    }
}
