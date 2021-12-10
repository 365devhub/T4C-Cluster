using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{
    [DatagramType(DatagramTypeEnum.GetPersonnalPClist, DatagramDirectionEnum.In)]
    public class RequestGetPlayingCharacterList : IRequest
    {
        public RequestGetPlayingCharacterList() { }


        public bool IsValid()
        {
            return true;
        }
    }
}
