﻿
using System;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message
{
    [DatagramType(DatagramTypeEnum.AuthenticateServerVersion, DatagramDirectionEnum.Out)]
    public class ResponseAuthenticateServerVersion : IResponse
    {
        public ResponseAuthenticateServerVersion()  { }

        [DataTypeUInt32(0)]
        public UInt32? ServerVersion { get; set; }

    }
}
