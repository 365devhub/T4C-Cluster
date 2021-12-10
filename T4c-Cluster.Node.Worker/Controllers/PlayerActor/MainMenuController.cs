using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T4c_Cluster.Node.Worker.Attributes;
using T4c_Cluster.Node.Worker.Sessions.PlayerActor;
using T4C_Cluster.API;
using T4C_Cluster.Lib.Network.Datagram.Message.MainMenu;
using static T4C_Cluster.API.Account;
using static T4C_Cluster.API.Caracter;
using static T4C_Cluster.API.Configuration;

namespace T4c_Cluster.Node.Worker.Controllers.PlayerActor
{
    public class MainMenuController : IControlerAction<RequestGetPlayingCharacterList, PlayerSession>
    {

        private ConfigurationClient _configurationClient;
        private CaracterClient _caracterClient;

        public MainMenuController(ConfigurationClient configurationClient, CaracterClient caracterClient)
        {
            _configurationClient = configurationClient;
            _caracterClient = caracterClient;
        }


        [ValidatePlayerAuthenticated]
        [ValidatePlayerNotInGame]
        public void Action(RequestGetPlayingCharacterList data, PlayerSession session, IActorRef actor)
        {
            var chars = _caracterClient.GetCharacters(new T4C_Cluster.API.GetCharactersRequest() { Username = session.Account });

            actor.Tell(new ResponseGetPlayingCharacterList() { Characters = chars.Characters.Select(c=> new ResponseGetPlayingCharacterList_CharsInfo() {
                Name = c.Name,
                Level=(ushort?)c.Level,
                Race=(ushort?)c.Race,
                Unknown=0
            }).ToArray()});

            actor.Tell(new ResponseGetPlayingCharacterListEquitSkin()
            {
                Characters = chars.Characters.Select(c => new GetPlayingCharacterListEquitSkin_CharsEquipement()
                {
                    Belt = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Belt)?.SkinId),
                    Body = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Body)?.SkinId),
                    Bracelets = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Bracelets)?.SkinId),
                    Cape = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Cape)?.SkinId),
                    Feet = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Feet)?.SkinId),
                    Gloves = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Gloves)?.SkinId),
                    HairColor = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.HairColor)?.SkinId),
                    Helm = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Helm)?.SkinId),
                    Legs = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Legs)?.SkinId),
                    Necklass = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Necklass)?.SkinId),
                    Orbe1 = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Orbe1)?.SkinId),
                    Ring1 = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Ring1)?.SkinId),
                    Ring2 = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Ring2)?.SkinId),
                    Rings = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Rings)?.SkinId),
                    TwoHands = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.TwoHands)?.SkinId),
                    Weapon = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.Weapon)?.SkinId),
                    WeaponLeft = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.WeaponLeft)?.SkinId),
                    WeaponRight = (ushort?)(c.Equipments.SingleOrDefault(x => x.Position == EquipmentPosition.WeaponRight)?.SkinId),
                }).ToArray()
            });

            actor.Tell(new ResponseMaxCharacterPerAccount() { 
                NbMaxCharacter = (byte?)_configurationClient.GetNbCharacterMax(new T4C_Cluster.API.NbCharacterMaxRequest()).NbMax
            });
        }
    }
}
