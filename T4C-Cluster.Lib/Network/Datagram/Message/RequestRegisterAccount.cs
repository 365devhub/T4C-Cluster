
using System;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message
{

    [DatagramType(DatagramTypeEnum.RegisterAccount, DatagramDirectionEnum.In)]
    public class RequestRegisterAccount : IRequest
    {
        public RequestRegisterAccount() { }

        [DataTypeString8(0)]
        public string Account { get; set; }


        [DataTypeString8(1)]
        public string Password { get; set; }


        [DataTypeUInt16(2)]
        public UInt16? Version { get; set; }


        [DataTypeUInt16(3)]
        public UInt16? Langue { get; set; }


        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Account)
                && !string.IsNullOrWhiteSpace(Password)
                && Version.HasValue
                && Langue.HasValue;
        }
    }
}
