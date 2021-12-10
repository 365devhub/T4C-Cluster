
using System;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.Authentification
{
    [DatagramType(DatagramTypeEnum.RegisterAccount, DatagramDirectionEnum.Out)]
    public class ResponseRegisterAccount : IResponse
    {
        public ResponseRegisterAccount() { }

        /// <summary>
        /// 1 : Identification Impossible
        /// 0 : Done
        /// </summary>
        [DataTypeInt8(0)]
        public byte? Code { get; set; }

        [DataTypeString16(1)]
        public string Message { get; set; }

        [DataTypeUInt32(2)]
        public uint? UniqueKey { get; set; }

    }
}
