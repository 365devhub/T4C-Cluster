using System;
using System.Collections;
using System.Linq;

namespace T4C_Cluster.Lib.Network.Datagram.Container
{
    /// <summary>
    /// Représente la parti falcultative d'un datagramme pour les datagramme séparé.
    /// </summary>
    public class DatagramSplittedHeader
    {
        /// <summary>
        /// Constructeur utilier pour la constructuion manuel d'un datagramme
        /// </summary>
        public DatagramSplittedHeader() { }

        /// <summary>
        /// Constructeur utilisé pour la lecture d'un datagramme
        /// </summary>
        /// <param name="_buffer"></param>
        public DatagramSplittedHeader(byte[] _buffer)
        {

            BitArray bits = new BitArray(_buffer.Take(4).ToArray());
            for (var i = 0; i < 6; i++)
            {
                if (bits[i])
                    SplitedGroupId += (byte)Math.Pow(2, i);
            }
            for (var i = 6; i < 11; i++)
            {
                if (bits[i])
                    SplitedPartNumber += (byte)Math.Pow(2, i-6);
            }
            for (var i = 11; i < 16; i++)
            {
                if (bits[i])
                    SplitedTotalPartNumber += (byte)Math.Pow(2, i-11);
            }
            for (var i = 16; i < 32; i++)
            {
                if (bits[i])
                    SplitedTotalDataSize += (ushort)Math.Pow(2, i-16);
            }
        }


        public byte[] GenerateBuffer()
        {
            byte[] bytes = new byte[4];
            BitArray bits = new BitArray(32);

            for (var i = 0; i < 6; i++) bits[i] = (SplitedGroupId & 1 << i) != 0; ;
            for (var i = 6; i < 11; i++) bits[i] = (SplitedPartNumber & (1 << (i - 6))) != 0;
            for (var i = 11; i < 16; i++) bits[i] = (SplitedTotalPartNumber & (1 << (i - 11))) != 0;
            for (var i = 16; i < 32; i++) bits[i] = (SplitedTotalDataSize & (1 << (i - 16))) != 0;

            bits.CopyTo(bytes, 0);
            return bytes;
        }

        /// <summary>
        /// Numéro unique de groupe
        /// </summary>
        /// BIT [0,5]
        public byte SplitedGroupId { get; set; }

        /// <summary>
        /// Numéro de morceau
        /// </summary>
        /// BIT [6,10]
        public byte SplitedPartNumber { get; set; }

        /// <summary>
        /// Nombre total de morceau
        /// </summary>
        /// BIT [11,15]
        public byte SplitedTotalPartNumber { get; set; }

        /// <summary>
        /// Taille total
        /// </summary>
        /// BIT [16,31]
        public ushort SplitedTotalDataSize { get; set; }
    }
}
