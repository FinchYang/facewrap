using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public interface IFaceCompare : Orleans.IGrainWithStringKey
    {
        Task<int> SayHello(string file1, string file2);
    }
}
