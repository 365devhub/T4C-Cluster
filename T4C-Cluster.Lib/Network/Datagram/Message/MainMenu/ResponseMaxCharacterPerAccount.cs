

using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{
    [DatagramType(DatagramTypeEnum.MaxCharacterPerAccount, DatagramDirectionEnum.Out)]
    public class ResponseMaxCharacterPerAccount : IResponse
    {
        public ResponseMaxCharacterPerAccount() { }


        [DataTypeInt8(0)]
        public byte? NbMaxCharacter { get; set; }

    }
}
