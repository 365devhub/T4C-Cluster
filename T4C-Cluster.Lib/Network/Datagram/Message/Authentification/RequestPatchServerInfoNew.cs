using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.Authentification
{

    [DatagramType(DatagramTypeEnum.PatchServerInfoNew, DatagramDirectionEnum.In)]
    public class RequestPatchServerInfoNew : IRequest
    {
        public RequestPatchServerInfoNew() { }

        public bool IsValid()
        {
            return true;
        }
    }
}
