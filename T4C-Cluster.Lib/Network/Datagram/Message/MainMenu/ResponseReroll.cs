

using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{
    [DatagramType(DatagramTypeEnum.Reroll, DatagramDirectionEnum.Out)]
    public class ResponseReroll : IResponse
    {

        [DataTypeInt8(1)]
        public byte? Agility { get; set; }

        [DataTypeInt8(2)]
        public byte? Endurence { get; set; }

        [DataTypeInt8(3)]
        public byte? Intelligence { get; set; }

        [DataTypeInt8(4)]
        public byte? Luck { get; set; }

        [DataTypeInt8(5)]
        public byte? Strength { get; set; }


        [DataTypeInt8(6)]
        public byte? Willpower { get; set; }

        [DataTypeInt8(7)]
        public byte? Wisdom { get; set; }

        [DataTypeUInt32(8)]
        public uint? MaximumHealthPoint { get; set; }

        [DataTypeUInt32(9)]
        public uint? HealthPoint { get; set; }

        [DataTypeUInt16(10)]
        public ushort? MaximumManaPoint { get; set; }

        [DataTypeUInt16(11)]
        public ushort? ManaPoint { get; set; }

    }
}
