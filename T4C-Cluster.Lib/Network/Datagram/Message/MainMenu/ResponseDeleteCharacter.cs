using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{
    public enum DeleteCharErrorCode : byte
    {
        OK = 0,
        PlayerNotFound = 1,
        NotYourPlayer = 2,
        OtherError = 3,

    };
    [DatagramType(DatagramTypeEnum.DeletePlayer, DatagramDirectionEnum.Out)]
    public class ResponseDeleteCharacter : IResponse
    {

        [DataTypeInt8(0)]
        public DeleteCharErrorCode? ErrorCode { get; set; }


    }
}
