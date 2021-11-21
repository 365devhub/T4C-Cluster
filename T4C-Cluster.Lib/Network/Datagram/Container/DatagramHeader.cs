using System;
using System.Collections;
using System.Linq;

namespace T4C_Cluster.Lib.Network.Datagram.Container
{
    /// <summary>
    /// Représentation logique d'un header générale pour un datagramme 172
    /// </summary>
    public class DatagramHeader
    {
        /// <summary>
        /// Constructeur utilisé pour la construction à partir de zéro
        /// </summary>
        public DatagramHeader()
        {
        }

        /// <summary>
        /// Constructeur utilisé pour la lecture d'un datagramme 172
        /// </summary>
        /// <param name="_buffer"></param>
        public DatagramHeader(byte[] _buffer)
        {
            BitArray bits = new BitArray(_buffer.Take(4).ToArray());
            for (var i = 0; i < 12; i++) Id += bits[i] ? (ushort)Math.Pow(2, i) : (ushort)0;
            NeedAck = bits[12];
            Compressed = bits[13];
            Splited = bits[14];
            Unknown = bits[15];
            for (var i = 16; i < 24; i++) Crc8 += bits[i] ? (byte)Math.Pow(2, i - 16) : (byte)0;
            for (var i = 24; i < 32; i++) PartNumber += bits[i] ? (byte)Math.Pow(2, i - 24) : (byte)0;
        }

        public byte[] GenerateBuffer()
        {
            byte[] bytes = new byte[4];
            BitArray bits = new BitArray(32);

            for (var i = 0; i < 12; i++) bits[i] = (Id & 1 << i) != 0;
            bits[12] = NeedAck;
            bits[13] = Compressed;
            bits[14] = Splited;
            bits[15] = Unknown;
            for (var i = 16; i < 24; i++) bits[i] = (Crc8 & 1 << i - 16) != 0;
            for (var i = 24; i < 32; i++) bits[i] = (PartNumber & 1 << i - 24) != 0;

            bits.CopyTo(bytes, 0);

            return bytes;
        }

        /// <summary>
        /// Identifiant unique du datagramme
        /// </summary>
        /// BIT [0,11]
        public ushort Id { get; set; }

        /// <summary>
        /// Est-ce que le datagramme nécéssite un accusé de réception ?
        /// </summary>
        /// BIT [12]
        public bool NeedAck { get; set; }

        /// <summary>
        /// Est-ce que le datagramme est compressé ?
        /// </summary>
        /// BIT [13]
        public bool Compressed { get; set; }

        /// <summary>
        /// Est-ce que le datagramme est séparé ?
        /// </summary>
        /// BIT [14]
        public bool Splited { get; set; }

        /// <summary>
        /// Est-ce que le datagramme possède un bloc proxy
        /// </summary>
        /// BIT [15]
        public bool Unknown { get; set; }

        /// <summary>
        /// sum de controle
        /// </summary>
        /// BIT [16,23]
        public byte Crc8 { get; set; }

        /// <summary>
        /// Identifiant d'un morceau de datagramme
        /// </summary>
        /// BIT [24,31]
        public byte PartNumber { get; set; }

    }
}
