using System;
using System.Collections.Generic;
using System.Text;

namespace T4C_Cluster.Lib.Network.Datagram.Attributes
{

    /// <summary>
    /// Représente une prériété de type Int64
    /// Cette attribut est utilisé pour la désérialisation de datagrammes
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DataTypeInt64Attribute : Attribute
    {
        /// <summary>
        /// Position de la prépriété dans un datagramme
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="index">Position de la prépriété dans un datagramme</param>
        public DataTypeInt64Attribute(int index)
        {
            Index = index;
        }
    }
}
