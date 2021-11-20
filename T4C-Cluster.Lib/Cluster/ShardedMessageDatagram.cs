using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T4C_Cluster.Lib.Cluster
{
    /// <summary>
    /// Représente un datagram devant être traité par le cluster
    /// </summary>
    public class ShardedMessageDatagram
    {
        /// <summary>
        /// Les données du datagramme
        /// </summary>
        public byte[] Datas { get; }
        
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="data">Les données</param>
        public ShardedMessageDatagram(byte[] data) {
            Datas = data;
        }
    }
}
