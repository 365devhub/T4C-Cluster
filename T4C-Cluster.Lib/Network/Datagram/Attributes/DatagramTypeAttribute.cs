
using System;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Attributes
{
    /// <summary>
    /// Représente une prériété de type tableau dont la longeur est exprimé sur 16 bits
    /// Cette attribut est utilisé pour la désérialisation de datagrammes
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DatagramTypeAttribute : Attribute
    {
        /// <summary>
        /// Position de la prépriété dans un datagramme
        /// </summary>
        public DatagramTypeEnum DatagramType { get; set; }
        public DatagramDirectionEnum Direction { get; set; }
        public bool NeedAck { get; set; }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="type">TypeDu datagram</param>
        public DatagramTypeAttribute(DatagramTypeEnum type, DatagramDirectionEnum direction, bool needAck = false)
        {
            DatagramType = type;
            Direction = direction;
            NeedAck = needAck;
        }
    }
}
