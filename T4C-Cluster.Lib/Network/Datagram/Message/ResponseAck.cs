using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message
{
    [DatagramType(DatagramTypeEnum.Ack, DatagramDirectionEnum.Out)]
    public class ResponseAck : IResponse
    {
        public ResponseAck() : base() { }


    }
}
