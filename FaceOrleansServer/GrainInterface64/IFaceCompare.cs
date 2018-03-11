using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterface64
{
    public interface IFaceCompare : Orleans.IGrainWithIntegerKey
    {
        Task<string> SayHello(string msg);
    }
}
