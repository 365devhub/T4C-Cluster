using System;
using System.Collections.Generic;
using System.Linq;

namespace T4C_Cluster.Lib.Network.Datagram.Container
{
    /// <summary>
    /// Représentation logique d'un datagramme 172
    /// </summary>
    public class Datagram
    {

        /// <summary>
        /// Constructeur par défaut. Utilisé pour construire un datagramme à partire d'un DatagramBody172
        /// </summary>
        public Datagram()
        {
            IsValid = true;
            IsChecksumValid = true;

        }

        /// <summary>
        /// Constructeur
        /// Crée un datagram a partir d'un buffer
        /// </summary>
        /// <param name="_buffer"></param>
        public Datagram(byte[] buffer, int length)
        {
            IsValid = true;
            IsChecksumValid = true;
            if (length < 4)
            {
                IsValid = false;
                return;
            }

            Header = new DatagramHeader(buffer);

            if (length == 4)
            {
                IsAck = true;
                return;
            }

            IsChecksumValid = _IsChecksumFromClientValid(buffer, length);
            if (!IsChecksumValid)
            {
                IsValid = false;
                return;
            }

            if (length < TotalHeaderLenght)
            {
                IsValid = false;
                return;
            }

            if (Header.Splited && Header.Compressed)
            {
                HeaderSplitted = new DatagramSplittedHeader(buffer.Skip(4).ToArray());
                CompressHeader = new DatagramCompressHeader(buffer.Skip(8).ToArray());
            }
            else if (Header.Splited)
            {
                HeaderSplitted = new DatagramSplittedHeader(buffer.Skip(4).ToArray());
            }
            else if (Header.Compressed)
            {
                CompressHeader = new DatagramCompressHeader(buffer.Skip(4).ToArray());
            }

            if (length >= TotalHeaderLenght + 6)
                Body = new DatagramBody(buffer, TotalHeaderLenght, length - TotalHeaderLenght);
            else
                IsValid = false;



        }

        /// <summary>
        /// Les header de base d'un datagramme
        /// </summary>
        public DatagramHeader Header { get; set; }

        /// <summary>
        /// Le header pour les datagrame splités
        /// </summary>
        public DatagramSplittedHeader HeaderSplitted { get; set; }

        /// <summary>
        /// Le header pour les datagrame compressé
        /// </summary>
        public DatagramCompressHeader CompressHeader { get; set; }

        // <summary>
        // Le corps du datagramme
        // </summary>
        public DatagramBody Body { get; set; }

        /// <summary>
        /// La taille totale du header du datagramme
        /// </summary>
        public int TotalHeaderLenght => 4 + (Header.Splited ? 4 : 0) + (Header.Compressed ? 8 : 0);

        /// <summary>
        /// Est-ce que le datagramme est un hack
        /// </summary>
        public bool IsAck { get; private set; }

        /// <summary>
        /// Est-ce que le checksum est valid
        /// </summary>
        public bool IsChecksumValid { get; private set; }

        /// <summary>
        /// Est-ce que le datagramme est valide
        /// </summary>
        public bool IsValid { get; set; }


        /// <summary>
        /// Contruction d'un accusé de réception
        /// </summary>
        /// <returns>un tableau de bits contenant le ack</returns>
        public byte[] MakeAck()
        {

            var bytes = new DatagramHeader()
            {
                Id = Header.Id,
                PartNumber = 0,
                Compressed = false,
                NeedAck = false,
                Unknown = false,
                Splited = false,
                Crc8 = 0
            }.GenerateBuffer();



            bytes[2] = CalculateChecksum(bytes, bytes.Length);
            return bytes;
        }
        /// <summary>
        /// Génération de datagrammes
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="getDatagramId"></param>
        /// <param name="direction"></param>
        /// <param name="getGroupId"></param>
        /// <param name="needAck"></param>
        /// <returns></returns>
        public byte[] GenerateBuffer()
        {
            byte[] byteHeader = Header.GenerateBuffer();
            byte[] byteHeaderSplit = Header.Splited ? HeaderSplitted.GenerateBuffer() : new byte[0];
            byte[] byteHeaderCompress = Header.Compressed ? CompressHeader.GenerateBuffer() : new byte[0];

            List<byte> bytes = new List<byte>();
            bytes.AddRange(byteHeader);
            bytes.AddRange(byteHeaderSplit);
            bytes.AddRange(byteHeaderCompress);
            bytes.AddRange(Body.GetBufferCopy());
            bytes[2] = CalculateChecksum(bytes.ToArray(), bytes.Count);

            return bytes.ToArray();

        }


        public byte CalculateChecksum(byte[] _buffer, int length)
        {
            byte sumByte = 0xAA;
            for (var i = 0; i < length; i++)
            {
                sumByte += _buffer[i];
            }
            sumByte -= _buffer[2];
            sumByte = (byte)(0x100 - sumByte);

            return sumByte;
        }
        private bool _IsChecksumFromClientValid(byte[] _buffer, int length)
        {
            return CalculateChecksum(_buffer, length) == _buffer[2];
        }
    }

}
