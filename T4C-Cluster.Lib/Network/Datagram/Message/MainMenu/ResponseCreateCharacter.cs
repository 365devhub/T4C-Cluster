using System;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{
    public enum ResponseCreateCharacterStatus : byte
    {
        OK = 0,
        TooManyChar = 4,
        NameAlreadyTaken = 5,
        NameInvalid = 8
    };
    [DatagramType(DatagramTypeEnum.CreatePlayer, DatagramDirectionEnum.Out)]
    public class ResponseCreateCharacter : IResponse
    {

        [DataTypeInt8(0)]
        public ResponseCreateCharacterStatus? Status { get; set; }

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
        public UInt32? MaximumHealthPoint { get; set; }

        [DataTypeUInt32(9)]
        public UInt32? HealthPoint { get; set; }

        [DataTypeUInt16(10)]
        public UInt16? MaximumManaPoint { get; set; }

        [DataTypeUInt16(11)]
        public UInt16? ManaPoint { get; set; }


    }
}
