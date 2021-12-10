using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.Authentification
{
    [DatagramType(DatagramTypeEnum.MessageOfTheDay, DatagramDirectionEnum.Out)]
    public class ResponseMessageOfTheDay : IResponse
    {
        public ResponseMessageOfTheDay() { }

        [DataTypeString16(0)]
        public string Message { get; set; }

    }
}
