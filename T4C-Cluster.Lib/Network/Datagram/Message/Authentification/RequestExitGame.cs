using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.Authentification
{

    [DatagramType(DatagramTypeEnum.ExitGame, DatagramDirectionEnum.In)]
    public class RequestExitGame : IRequest
    {
        public RequestExitGame() { }

        public bool IsValid()
        {
            return true;
        }
    }
}
