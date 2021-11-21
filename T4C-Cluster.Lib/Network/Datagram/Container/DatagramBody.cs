using System;
using System.Collections.Generic;
using System.Net;

namespace T4C_Cluster.Lib.Network.Datagram.Container
{
    public class DatagramBody
    {
        private static int ByteLendth = 1;
        private static int UInt16Lendth = 2;
        private static int UInt32Lendth = 4;
        private static int UInt64Lendth = 8;

        private int _position;
        private byte[] _buffer;

        public DatagramBody()
        {
            _position = 0;
            _buffer = new byte[0];
        }

        /// <summary>
        /// Initialisation du corp d'un datagramme 
        /// </summary>
        /// <param name="buffer">Le buffer contenant les données</param>
        public DatagramBody(byte[] buffer, int offset, int length)
        {
            _position = 0;
            _buffer = new byte[length];
            Array.Copy(buffer, offset, _buffer, 0, length);
        }

        /// <summary>
        /// Mise à jour de la position du curseur.
        /// </summary>
        /// <param name="position">nouvelle position</param>
        public void SetCursorPosition(int position)
        {
            _position = position;
        }

        /// <summary>
        /// Lecture du prochain octet
        /// </summary>
        /// <returns>Retourne un octet</returns>
        public byte? ReadByte()
        {
            if (_position >= _buffer.Length)
            {
                return null;
            }

            var retour = _buffer[_position];
            _position += ByteLendth;

            return retour;
        }


        /// <summary>
        /// Lecture du prochain octet en tant que boolean
        /// </summary>
        /// <returns>Retourne une valeur boolean</returns>
        public bool? ReadBool()
        {

            if (_position >= _buffer.Length)
            {
                return null;
            }

            var retour = BitConverter.ToBoolean(_buffer, _position);
            _position += ByteLendth;
            return retour;
        }

        /// <summary>
        /// Retourne les 2 prochain octet convertient en UInt16
        /// </summary>
        /// <returns>Le Uint16 représentant les deux octet</returns>
        public ushort? ReadUInt16()
        {

            if (_position + (UInt16Lendth - 1) >= _buffer.Length)
            {
                return null;
            }

            var retour = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(_buffer, _position));
            _position += UInt16Lendth;
            return retour;
        }


        /// <summary>
        /// Retourne les 4 prochain octet convertient en UInt32
        /// </summary>
        /// <returns>Le Uint32 représentant les deux octet</returns>
        public uint? ReadUInt32()
        {

            if (_position + (UInt32Lendth - 1) >= _buffer.Length)
            {
                return null;
            }

            var retour = (uint)IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(_buffer, _position));
            _position += UInt32Lendth;
            return retour;
        }

        /// <summary>
        /// Lectue des prochains 8 octet convertit en UInt64 
        /// </summary>
        /// <returns>La valeur des 8 octet suivant</returns>
        public ulong? ReadUInt64()
        {
            if (_position + (UInt64Lendth - 1) >= _buffer.Length)
            {
                return null;
            }

            var retour = (ulong)IPAddress.NetworkToHostOrder((long)BitConverter.ToUInt64(_buffer, _position));
            _position += UInt64Lendth;
            return retour;
        }

        /// <summary>
        /// Retourne les 2 prochain octet convertient en Int16
        /// </summary>
        /// <returns>Le Uint16 représentant les deux octet</returns>
        public short? ReadInt16()
        {

            if (_position + (UInt16Lendth - 1) >= _buffer.Length)
            {
                return null;
            }

            var retour = IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(_buffer, _position));
            _position += UInt16Lendth;
            return retour;
        }


        /// <summary>
        /// Retourne les 4 prochain octet convertient en Int32
        /// </summary>
        /// <returns>Le Uint32 représentant les deux octet</returns>
        public int? ReadInt32()
        {

            if (_position + (UInt32Lendth - 1) >= _buffer.Length)
            {
                return null;
            }

            var retour = IPAddress.NetworkToHostOrder((int)BitConverter.ToUInt32(_buffer, _position));
            _position += UInt32Lendth;
            return retour;
        }

        /// <summary>
        /// Lectue des prochains 8 octet convertit en Int64 
        /// </summary>
        /// <returns>La valeur des 8 octet suivant</returns>
        public long? ReadInt64()
        {
            if (_position + (UInt64Lendth - 1) >= _buffer.Length)
            {
                return null;
            }

            var retour = IPAddress.NetworkToHostOrder((long)BitConverter.ToUInt64(_buffer, _position));
            _position += UInt64Lendth;
            return retour;
        }

        /// <summary>
        /// Retoune le prochain text dont la longueur est exprimé sur 1 octet
        /// </summary>
        /// <returns>Le texte</returns>
        public string ReadString8()
        {
            var longueur = ReadByte();
            if (!longueur.HasValue)
                return null;

            if (_position + (longueur.Value - 1) > _buffer.Length - 1)
            {
                return null;
            }

            string texte = System.Text.Encoding.UTF8.GetString(_buffer, _position, longueur.Value);
            _position += longueur.Value;

            return texte;
        }

        /// <summary>
        /// Retoune le prochain text dont la longueur est exprimé sur 2 octet
        /// </summary>
        /// <returns>Les texte</returns>
        public string ReadString16()
        {
            var longueur = ReadUInt16();
            if (!longueur.HasValue)
                return null;

            if (_position + (longueur.Value - 1) > _buffer.Length - 1)
            {
                return null;
            }

            string texte = System.Text.Encoding.UTF8.GetString(_buffer, _position, longueur.Value);
            _position += longueur.Value;

            return texte;
        }




        /// <summary>
        /// Inscrit un octet
        /// </summary>
        public void WriteByte(byte? value)
        {
            var retour = value.GetValueOrDefault(0);

            Array.Resize(ref _buffer, _buffer.Length + ByteLendth);
            _buffer[_position] = retour;
            _position += ByteLendth;
        }


        /// <summary>
        /// Inscrit un boolean
        /// </summary>
        public void WriteBool(bool? value)
        {
            var retour = (byte)(value.GetValueOrDefault(false) ? 1 : 0);

            Array.Resize(ref _buffer, _buffer.Length + ByteLendth);
            _buffer[_position] = retour;
            _position += ByteLendth;
        }

        /// <summary>
        /// Inscrit un Int16
        /// </summary>
        public void WriteInt16(short? value)
        {
            var retour = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value.GetValueOrDefault(0)));

            Array.Resize(ref _buffer, _buffer.Length + UInt16Lendth);
            Array.Copy(retour, 0, _buffer, _position, UInt16Lendth);

            _position += UInt16Lendth;
        }


        /// <summary>
        /// Inscrit un Int32
        /// </summary>
        public void WriteInt32(int? value)
        {
            var retour = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value.GetValueOrDefault(0)));

            Array.Resize(ref _buffer, _buffer.Length + UInt32Lendth);
            Array.Copy(retour, 0, _buffer, _position, UInt32Lendth);

            _position += UInt32Lendth;
        }

        /// <summary>
        /// Inscrit un Int64 
        /// </summary>
        public void WriteInt64(long? value)
        {
            var retour = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(value.GetValueOrDefault(0)));

            Array.Resize(ref _buffer, _buffer.Length + UInt64Lendth);
            Array.Copy(retour, 0, _buffer, _position, UInt64Lendth);

            _position += UInt64Lendth;
        }

        /// <summary>
        /// Inscrit un UInt16
        /// </summary>
        public void WriteUInt16(ushort? value)
        {
            var retour = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)value.GetValueOrDefault(0)));

            Array.Resize(ref _buffer, _buffer.Length + UInt16Lendth);
            Array.Copy(retour, 0, _buffer, _position, UInt16Lendth);

            _position += UInt16Lendth;
        }


        /// <summary>
        /// Inscrit un UInt32
        /// </summary>
        public void WriteUInt32(uint? value)
        {
            var retour = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((int)value.GetValueOrDefault(0)));

            Array.Resize(ref _buffer, _buffer.Length + UInt32Lendth);
            Array.Copy(retour, 0, _buffer, _position, UInt32Lendth);

            _position += UInt32Lendth;
        }

        /// <summary>
        /// Inscrit un UInt64 
        /// </summary>
        public void WriteUInt64(ulong? value)
        {
            var retour = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)value.GetValueOrDefault(0)));

            Array.Resize(ref _buffer, _buffer.Length + UInt64Lendth);
            Array.Copy(retour, 0, _buffer, _position, UInt64Lendth);

            _position += UInt64Lendth;
        }

        /// <summary>
        /// Inscrit le prochain text dont la longueur est exprimé sur 1 octet
        /// </summary>
        public void WriteString8(string data)
        {
            if (data == null)
            {
                WriteByte(0);
                return;
            }
            WriteByte((byte)data.Length);

            Array.Resize(ref _buffer, _buffer.Length + data.Length);
            Array.Copy(System.Text.Encoding.UTF8.GetBytes(data), 0, _buffer, _position, data.Length);

            _position += data.Length;
        }

        /// <summary>
        /// Inscrit le prochain text dont la longueur est exprimé sur 2 octet
        /// </summary>
        public void WriteString16(string data)
        {
            if (data == null)
            {
                WriteUInt16(0);
                return;
            }
            WriteUInt16((ushort)data.Length);

            Array.Resize(ref _buffer, _buffer.Length + data.Length);
            Array.Copy(System.Text.Encoding.UTF8.GetBytes(data), 0, _buffer, _position, data.Length);

            _position += data.Length;
        }




        public void PushBuffer(byte[] buffer)
        {
            Array.Resize(ref _buffer, _buffer.Length + buffer.Length);
            Array.Copy(buffer, 0, _buffer, _buffer.Length - buffer.Length, buffer.Length);
        }

        public int GetBufferLength()
        {
            return _buffer.Length;
        }
        /// <summary>
        /// Permet de décompresser le buffer s'il est compressé
        /// Doit-être fait une fois que tous les partie du datagramme sont récupérées
        /// </summary>
        public void DecompressBuffer()
        {
            var tbuffer = Ionic.Zlib.ZlibStream.UncompressBuffer(_buffer);
            _buffer = new byte[tbuffer.Length];
            tbuffer.CopyTo(_buffer, 0);

        }


        /// <summary>
        /// Obtention d'une copy du buffer de corp de datagrame
        /// </summary>
        /// <returns></returns>
        public byte[] GetBufferCopy()
        {
            byte[] newbuffer = new byte[_buffer.Length];
            Array.Copy(_buffer, 0, newbuffer, 0, _buffer.Length);
            return newbuffer;
        }

        /// <summary>
        /// Obtention d'une copy du buffer de corp de datagrame sous sa forme compressé
        /// </summary>
        /// <returns></returns>
        public byte[] GetBufferCopyCompresssed()
        {
            /*using (MemoryStream ms = new MemoryStream())
            using (var zlib = new Ionic.Zlib.ZlibStream(ms, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestCompression))
            {
                ms.Write(this.GetBufferCopy());
                return ms.ToArray();
            }*/
            var tbuffer = Ionic.Zlib.ZlibStream.CompressBuffer(GetBufferCopy());
            return tbuffer;
        }

        public List<Datagram> GenerateDatagrams(bool useCompression, bool needAck, Func<ushort> getDatagramId, Func<byte> getGroupId)
        {
            List<Datagram> datagrams = new List<Datagram>();
            var maxPackLength = 1024 - 16;
            var bufferUnComp = GetBufferCopy();
            var bufferComp = useCompression ? GetBufferCopyCompresssed() : bufferUnComp;

            var compressed = bufferUnComp.Length > bufferComp.Length;
            var realBuffer = compressed ? bufferComp : bufferUnComp;

            byte nbPart = (byte)(realBuffer.Length / maxPackLength + 1);
            bool splitted = nbPart > 1;
            byte groupId = splitted ? getGroupId() : (byte)0;
            for (byte i = 0; i < nbPart; i++)
            {
                Datagram datagram = new Datagram();
                datagram.Header = new DatagramHeader();
                datagram.Header.Id = getDatagramId();
                datagram.Header.PartNumber = i;
                datagram.Header.Compressed = compressed;
                datagram.Header.Unknown = false;
                datagram.Header.Splited = splitted;
                datagram.Header.NeedAck = needAck;

                if (datagram.Header.Splited)
                {
                    datagram.HeaderSplitted = new DatagramSplittedHeader();
                    datagram.HeaderSplitted.SplitedGroupId = groupId;
                    datagram.HeaderSplitted.SplitedPartNumber = i;
                    datagram.HeaderSplitted.SplitedTotalDataSize = (ushort)realBuffer.Length;
                    datagram.HeaderSplitted.SplitedTotalPartNumber = nbPart;
                }

                if (datagram.Header.Compressed)
                {
                    datagram.CompressHeader = new DatagramCompressHeader();
                    datagram.CompressHeader.CompressedDataLenght = (uint)realBuffer.Length;
                    datagram.CompressHeader.UncompressedDataLenght = (uint)bufferUnComp.Length;
                }

                var offset = i * maxPackLength;
                var length = Math.Min(maxPackLength, realBuffer.Length - offset);

                datagram.Body = new DatagramBody(realBuffer, offset, length);

                datagrams.Add(datagram);
            }


            return datagrams;
        }
    }
}
