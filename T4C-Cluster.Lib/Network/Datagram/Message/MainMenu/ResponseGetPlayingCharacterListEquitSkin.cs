using System;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{
    [DatagramType(DatagramTypeEnum.GetPersonnalPClistEquitSkin, DatagramDirectionEnum.Out)]
    public class ResponseGetPlayingCharacterListEquitSkin : IResponse
    {
        public ResponseGetPlayingCharacterListEquitSkin() { }


        [DataTypeArray8(0)]
        public GetPlayingCharacterListEquitSkin_CharsEquipement[] Characters { get; set; }


    }

    public class GetPlayingCharacterListEquitSkin_CharsEquipement
    {
        public GetPlayingCharacterListEquitSkin_CharsEquipement() { }


        [DataTypeUInt16(0)]
        public ushort? Body { get; set; }

        [DataTypeUInt16(1)]
        public ushort? Feet { get; set; }

        [DataTypeUInt16(2)]
        public ushort? Gloves { get; set; }

        [DataTypeUInt16(3)]
        public ushort? Helm { get; set; }

        [DataTypeUInt16(4)]
        public ushort? Legs { get; set; }

        [DataTypeUInt16(5)]
        public ushort? Rings { get; set; }

        [DataTypeUInt16(6)]
        public ushort? Bracelets { get; set; }

        [DataTypeUInt16(7)]
        public ushort? Necklass { get; set; }

        [DataTypeUInt16(8)]
        public ushort? WeaponRight { get; set; }

        [DataTypeUInt16(9)]
        public ushort? WeaponLeft { get; set; }

        [DataTypeUInt16(10)]
        public ushort? TwoHands { get; set; }

        [DataTypeUInt16(11)]
        public ushort? Ring1 { get; set; }

        [DataTypeUInt16(12)]
        public ushort? Ring2 { get; set; }

        [DataTypeUInt16(13)]
        public ushort? Weapon { get; set; }

        [DataTypeUInt16(14)]
        public ushort? Belt { get; set; }

        [DataTypeUInt16(15)]
        public ushort? Cape { get; set; }

        [DataTypeUInt16(16)]
        public ushort? Orbe1 { get; set; }

        [DataTypeUInt16(17)]
        public ushort? HairColor { get; set; }
    }
}
