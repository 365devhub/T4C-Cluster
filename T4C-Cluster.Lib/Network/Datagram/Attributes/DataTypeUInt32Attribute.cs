using System;
using System.Collections.Generic;
using System.Text;

namespace T4C_Cluster.Lib.Network.Datagram.Attributes
{
    /// <summary>
    /// Représente une prériété de type UInt32
    /// Cette attribut est utilisé pour la désérialisation de datagrammes
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DataTypeUInt32Attribute : Attribute
    {
        /// <summary>
        /// Position de la prépriété dans un datagramme
        /// </summary>
        public int Index { get; }
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="index">Position de la prépriété dans un datagramme</param>
        public DataTypeUInt32Attribute(int position)
        {
            Index = position;
        }
    }
}
