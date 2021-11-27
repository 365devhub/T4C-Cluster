
using System;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message
{
    [DatagramType(DatagramTypeEnum.PatchServerInfoNew, DatagramDirectionEnum.Out)]
    public class ResponsePatchServerInfoNew : IResponse
    {
        public ResponsePatchServerInfoNew() { }

        [DataTypeUInt32(0)]
        public UInt32? ServerVersion { get; set; }

        [DataTypeString16(1)]
        public string WebPatchIP { get; set; }

        [DataTypeString16(2)]
        public string ImagePath { get; set; }

        [DataTypeString16(3)]
        public string Username { get; set; }

        [DataTypeString16(4)]
        public string Password { get; set; }

        [DataTypeUInt16(5)]
        public UInt16? Lang { get; set; }

    }
}
