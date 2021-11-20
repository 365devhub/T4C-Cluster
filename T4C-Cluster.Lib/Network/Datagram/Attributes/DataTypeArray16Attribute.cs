using System;
using System.Collections.Generic;
using System.Text;

namespace T4C_Cluster.Lib.Network.Datagram.Attributes
{
    /// <summary>
    /// Représente une prériété de type tableau dont la longeur est exprimé sur 16 bits
    /// Cette attribut est utilisé pour la désérialisation de datagrammes
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DataTypeArray16Attribute : Attribute
    {
        /// <summary>
        /// Position de la prépriété dans un datagramme
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="index">Position de la prépriété dans un datagramme</param>
        public DataTypeArray16Attribute(int index)
        {
            Index = index;
        }
    }
}
