using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{

    [DatagramType(DatagramTypeEnum.GetPersonnalPClist, DatagramDirectionEnum.Out)]
    public class ResponseGetPlayingCharacterList : IResponse
    {
        public ResponseGetPlayingCharacterList() { }


        [DataTypeArray8(0)]
        public ResponseGetPlayingCharacterList_CharsInfo[] Characters { get; set; }
    }

    public class ResponseGetPlayingCharacterList_CharsInfo
    {
        public ResponseGetPlayingCharacterList_CharsInfo() { }

        [DataTypeString8(0)]
        public string Name { get; set; }

        [DataTypeUInt16(1)]
        public UInt16? Race { get; set; }

        [DataTypeUInt16(2)]
        public UInt16? Level { get; set; }

        [DataTypeUInt16(3)]
        public UInt16? Unknown { get; set; }
    }
}
