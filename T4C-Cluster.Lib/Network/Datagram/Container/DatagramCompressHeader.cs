using System;

namespace T4C_Cluster.Lib.Network.Datagram.Container
{
    /// <summary>
    /// Représente  un header complémentaire pour la compression
    /// </summary>

    public class DatagramCompressHeader
    {
        /// <summary>
        /// Constructeur utilier pour la constructuion manuel d'un datagramme
        /// </summary>
        public DatagramCompressHeader() : base()
        {

        }

        /// <summary>
        /// Constructeur utilisé pour la lecture d'un datagramme
        /// </summary>
        /// <param name="_buffer"></param>
        public DatagramCompressHeader(byte[] _buffer)
        {
            CompressedDataLenght = BitConverter.ToUInt32(_buffer, 0);
            UncompressedDataLenght = BitConverter.ToUInt32(_buffer, 4);
        }

        public byte[] GenerateBuffer()
        {
            byte[] bytes = new byte[8];
            BitConverter.GetBytes(CompressedDataLenght).CopyTo(bytes, 0);
            BitConverter.GetBytes(UncompressedDataLenght).CopyTo(bytes, 4);
            return bytes;
        }

        /// <summary>
        /// Taille compressé
        /// </summary>
        public uint CompressedDataLenght { get; set; }

        /// <summary>
        ///  Taille décompressé
        /// </summary>
        public uint UncompressedDataLenght { get; set; }

    }
}
