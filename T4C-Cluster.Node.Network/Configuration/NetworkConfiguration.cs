using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4C_Cluster.Node.Network.Configuration
{
    /// <summary>
    /// Configuration object for the Network section
    /// </summary>
    public class NetworkConfiguration
    {
        public string Address { get; set; }
        public ushort Port { get; set; }
    }
}
