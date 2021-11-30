
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message
{
    [DatagramType(DatagramTypeEnum.MessageOfTheDay, DatagramDirectionEnum.Out)]
    public class ResponseMessageOfTheDay : IResponse
    {
        public ResponseMessageOfTheDay()  { }

        [DataTypeString16(4)]
        public string Message { get; set; }

    }
}
