using System.Linq;
using T4C_Cluster.Lib.Network.Datagram.Attributes;
using T4C_Cluster.Lib.Network.Datagram.Enums;

namespace T4C_Cluster.Lib.Network.Datagram.Message.MainMenu
{
    public enum Sex
    {
        Men = 0,
        Femme = 1
    };
    [DatagramType(DatagramTypeEnum.CreatePlayer, DatagramDirectionEnum.In)]
    public class RequestCreateCharacter : IRequest
    {
        private static readonly string ValidChar = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ -äïëöüéèàùâîêôû";

        public RequestCreateCharacter() { }


        [DataTypeInt8(0)]
        public byte? AnswerQuestionWarrior { get; set; }

        [DataTypeInt8(1)]
        public byte? AnswerQuestionMage { get; set; }

        [DataTypeInt8(2)]
        public byte? AnswerQuestionThief { get; set; }

        [DataTypeInt8(3)]
        public byte? AnswerQuestionPriest { get; set; }

        [DataTypeInt8(4)]
        public byte? AnswerQuestionNormal { get; set; }

        [DataTypeInt8(5)]
        public Sex? Sex { get; set; }

        [DataTypeString8(6)]
        public string Name { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name)
                && Sex.HasValue
                && AnswerQuestionMage.HasValue
                && AnswerQuestionNormal.HasValue
                && AnswerQuestionPriest.HasValue
                && AnswerQuestionThief.HasValue
                && AnswerQuestionWarrior.HasValue
                && AnswerQuestionMage + AnswerQuestionNormal + AnswerQuestionPriest + AnswerQuestionThief + AnswerQuestionWarrior == 4
                && Name.All(c => ValidChar.Any(cc => cc == c));
        }
    }
}
