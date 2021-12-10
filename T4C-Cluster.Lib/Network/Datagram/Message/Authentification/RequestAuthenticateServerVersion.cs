
using System;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.Authentification
{

    [DatagramType(DatagramTypeEnum.AuthenticateServerVersion, DatagramDirectionEnum.In)]
    public class RequestAuthenticateServerVersion : IRequest
    {
        public RequestAuthenticateServerVersion() { }

        [DataTypeUInt32(0)]
        public uint? Version { get; set; }

        public bool IsValid()
        {
            return Version.HasValue;
        }
    }
}
