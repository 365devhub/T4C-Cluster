using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4C_Cluster.Lib.Network.Datagram
{
    public interface IRequest
    {
        public bool IsValid();

        public bool NeedAuthentification();
    }
}
