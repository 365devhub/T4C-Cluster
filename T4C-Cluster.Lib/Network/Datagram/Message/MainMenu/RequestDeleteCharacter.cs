using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{
    [DatagramType(DatagramTypeEnum.DeletePlayer, DatagramDirectionEnum.In)]
    public class RequestDeleteCharacter : IRequest
    {

        public RequestDeleteCharacter() { }


        [DataTypeString8(0)]
        public string Name { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
