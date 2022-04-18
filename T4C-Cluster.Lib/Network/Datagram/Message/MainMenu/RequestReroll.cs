
using T4C_Cluster.Lib.Network.Datagram;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{
    [DatagramType(DatagramTypeEnum.Reroll, DatagramDirectionEnum.In)]
    public class RequestReroll : IRequest
    {

        public RequestReroll() { }



        public bool IsValid()
        {
            return true;
        }
    }
}
