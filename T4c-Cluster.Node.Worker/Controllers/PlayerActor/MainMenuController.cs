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
using static T4C_Cluster.API.Character;
using static T4C_Cluster.API.Configuration;

namespace T4c_Cluster.Node.Worker.Controllers.PlayerActor
{
    public class MainMenuController : IControlerAction<RequestGetPlayingCharacterList, PlayerSession>,
                                      IControlerAction<RequestDeleteCharacter, PlayerSession>,
                                      IControlerAction<RequestCreateCharacter, PlayerSession>,
                                      IControlerAction<RequestQueryNameExistence, PlayerSession>
    {

        private ConfigurationClient _configurationClient;
        private CharacterClient _characterClient;
        private Random _local;

        public MainMenuController(ConfigurationClient configurationClient, CharacterClient characterClient)
        {
            _configurationClient = configurationClient;
            _characterClient = characterClient;
           _local = new Random();
        }

        [SaveSnapshot]
        [ValidatePlayerAuthenticated]
        [ValidatePlayerNotInGame]
        public void Action(RequestGetPlayingCharacterList data, PlayerSession session, IActorRef actor)
        {
            var chars = _characterClient.GetCharacters(new T4C_Cluster.API.GetCharactersRequest() { Username = session.Account });

            actor.Tell(new ResponseGetPlayingCharacterList() { Characters = chars.Caracters.Select(c => new ResponseGetPlayingCharacterList_CharsInfo() {
                Name = c.Name,
                Level = (ushort?)c.Level,
                Race = (ushort?)c.Race,
                Unknown = 0
            }).ToArray() });

            actor.Tell(new ResponseGetPlayingCharacterListEquitSkin()
            {
                Characters = chars.Caracters.Select(c => new GetPlayingCharacterListEquitSkin_CharsEquipement()
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

            session.IsCreatingCharacter = false;
            session.Caracters = chars.Caracters.Select(c => c.Name).ToList();
        }

        [SaveSnapshot]
        [ValidatePlayerAuthenticated]
        [ValidatePlayerNotInGame]
        [ValidatePlayerNotIsCreatingCharcater]
        public void Action(RequestDeleteCharacter data, PlayerSession session, IActorRef actor)
        {
            if(!session.Caracters.Contains(data.Name))
                actor.Tell(new ResponseDeleteCharacter() { ErrorCode = DeleteCharErrorCode.NotYourPlayer });


            var result = _characterClient.DeleteCaracter(new DeleteCharacterRequest() { Name = data.Name });

            if(result.Result == false)
                actor.Tell(new ResponseDeleteCharacter() { ErrorCode = DeleteCharErrorCode.OtherError });


            actor.Tell(new ResponseDeleteCharacter() { ErrorCode = DeleteCharErrorCode.OK });

            session.Caracters.RemoveAll(c=>c == data.Name);
        }

        [SaveSnapshot]
        [ValidatePlayerAuthenticated]
        [ValidatePlayerNotInGame]
        [ValidatePlayerNotIsCreatingCharcater]
        public void Action(RequestCreateCharacter data, PlayerSession session, IActorRef actor)
        {

            if (_characterClient.IsCharacterNameUsed(new IsCharacterNameUsedRequest() { Name = data.Name }).Used)
            {
                actor.Tell(new ResponseCreateCharacter() { Status = ResponseCreateCharacterStatus.NameAlreadyTaken });
            }
            else if (_characterClient.GetCharacters(new GetCharactersRequest() { Username = session.Account }).Caracters.Count > _configurationClient.GetNbCharacterMax(new NbCharacterMaxRequest()).NbMax)
            {
                actor.Tell(new ResponseCreateCharacter() { Status = ResponseCreateCharacterStatus.TooManyChar });
            } 
            else 
            {
                session.TemporaryObject = data;

                var response = new ResponseCreateCharacter();
                response.Status = ResponseCreateCharacterStatus.OK;                
                response.Strength = (byte)(_local.Next(0, 10) + (3.5 * data.AnswerQuestionWarrior) + (0.5 * data.AnswerQuestionMage) + (3.5 * data.AnswerQuestionThief) + (1.5 * data.AnswerQuestionPriest) + (1.5 * data.AnswerQuestionNormal)); 
                response.Agility = (byte)(_local.Next(0, 10) + (1.5 * data.AnswerQuestionWarrior) + (1.5 * data.AnswerQuestionMage) + (3.5 * data.AnswerQuestionThief) + (0.5 * data.AnswerQuestionPriest) + (1.5 * data.AnswerQuestionNormal));
                response.Endurence = (byte)(_local.Next(0, 10) + (3.5 * data.AnswerQuestionWarrior) + (0.5 * data.AnswerQuestionMage) + (1.5 * data.AnswerQuestionThief) + (1.5 * data.AnswerQuestionPriest) + (1.5 * data.AnswerQuestionNormal));
                response.Intelligence = (byte)(_local.Next(0, 10) + (0.5 * data.AnswerQuestionWarrior) + (3.5 * data.AnswerQuestionMage) + (1.5 * data.AnswerQuestionThief) + (1.5 * data.AnswerQuestionPriest) + (1.5 * data.AnswerQuestionNormal));
                response.Wisdom = (byte)(_local.Next(0, 10) + (0.5 * data.AnswerQuestionWarrior) + (1.5 * data.AnswerQuestionMage) + (0.5 * data.AnswerQuestionThief) + (3.5 * data.AnswerQuestionPriest) + (1.5 * data.AnswerQuestionNormal));
                response.Willpower = 100;
                response.Luck = 100;
                response.HealthPoint = (UInt32)(_local.Next(1, 5) + _local.Next(1, 5) + 48 + response.Endurence);
                response.MaximumHealthPoint = response.HealthPoint;
                response.ManaPoint = (UInt16)(10 + ((response.Intelligence * 2) / 3) + (response.Wisdom / 3) + _local.Next(0, 5));
                response.MaximumManaPoint = response.ManaPoint;

                /*_characterClient.CreateCharacter(new CreateCharacterRequest() {
                
                });*/

                actor.Tell(response);
            }


            session.IsCreatingCharacter = true;
        }

        public void Action(RequestQueryNameExistence data, PlayerSession session, IActorRef actor)
        {
            if (_characterClient.IsCharacterNameUsed(new IsCharacterNameUsedRequest() { Name = data.Name }).Used)
            {
                actor.Tell(new ResponseQueryNameExistence() { Valid = ResponseQueryNameExistenceStatus.AlreadyUsed });
            }
            else
            {
                actor.Tell(new ResponseQueryNameExistence() { Valid = ResponseQueryNameExistenceStatus.Valid });
            }
        }
    }
}
