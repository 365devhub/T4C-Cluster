using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4C_Cluster.Lib.Network.Datagram.Container;

namespace T4c_Cluster.Node.Worker.Sessions.PlayerActor
{
    public class PlayerSessionDatagramToRelaunch
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PlayerSessionDatagramToRelaunch()
        {
            ElapsedTime = new Stopwatch();
            ElapsedTime.Start();
            RelaunchCount = 0;

        }


        /// <summary>
        /// Datagram's data
        /// </summary>
        public Datagram Datagram;

        /// <summary>
        /// Watch interval
        /// </summary>
        public Stopwatch ElapsedTime;

        /// <summary>
        /// re send interval
        /// </summary>
        public int RelaunchInterval;
        
        /// <summary>
        /// Current re send count
        /// </summary>
        public int RelaunchCount;

        /// <summary>
        /// Numbner of time to send
        /// </summary>
        public int RelaunchCountTotal;
    }

    public class PlayerSession
    {
        public PlayerSession() 
        {

            LastDatagramElapsedTime = new Stopwatch();
            LastDatagramElapsedTime.Start();
        }

        public static ushort MaximumAlreadyTreatedDatagramIndex = 100;

        public ushort AlreadyTreatedDatagramIndex = 0;
        public ushort[] AlreadyTreatedDatagram = new ushort[MaximumAlreadyTreatedDatagramIndex];
        public List<PlayerSessionDatagramToRelaunch> DatagramsToRelaunch = new List<PlayerSessionDatagramToRelaunch>();


        /// <summary>
        /// Is the client Authentificated ?
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// Last time we received a datagram from a client
        /// </summary>
        public Stopwatch LastDatagramElapsedTime { get; }
    }
}
