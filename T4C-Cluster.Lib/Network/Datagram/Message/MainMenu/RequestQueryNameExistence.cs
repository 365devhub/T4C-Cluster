using System.Linq;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{

    [DatagramType(DatagramTypeEnum.QueryNameExistence, DatagramDirectionEnum.In)]
    public class RequestQueryNameExistence : IRequest
    {
        private static readonly string ValidChar = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ -äïëöüéèàùâîêôû";
        public RequestQueryNameExistence() { }


        [DataTypeString16(0)]
        public string Name { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) && Name.All(c => ValidChar.Any(cc => cc == c));
        }
    }
}
