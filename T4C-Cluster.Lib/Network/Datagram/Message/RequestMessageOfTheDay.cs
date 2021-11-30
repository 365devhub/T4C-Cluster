
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message
{
    [DatagramType(DatagramTypeEnum.MessageOfTheDay, DatagramDirectionEnum.In)]
    public class RequestMessageOfTheDay : IRequest
    {
        public RequestMessageOfTheDay() { }

        public bool IsValid()
        {
            return true;
        }
    }
}
