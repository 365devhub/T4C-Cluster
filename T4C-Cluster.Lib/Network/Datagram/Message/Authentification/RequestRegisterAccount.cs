
using System;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.Authentification
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
        public ushort? Version { get; set; }


        [DataTypeUInt16(3)]
        public ushort? Langue { get; set; }


        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Account)
                && !string.IsNullOrWhiteSpace(Password)
                && Version.HasValue
                && Langue.HasValue;
        }
    }
}
