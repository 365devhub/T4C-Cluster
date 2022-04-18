using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{
    public enum ResponseQueryNameExistenceStatus : byte
    {
        Valid = 0,
        Invalid = 2,
        AlreadyUsed = 1
    };


    [DatagramType(DatagramTypeEnum.QueryNameExistence, DatagramDirectionEnum.Out)]
    public class ResponseQueryNameExistence : IResponse
    {
        public ResponseQueryNameExistence() { }

        [DataTypeInt8(0)]
        public ResponseQueryNameExistenceStatus? Valid { get; set; }

    }
}
