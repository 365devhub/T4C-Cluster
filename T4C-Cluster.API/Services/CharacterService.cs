using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T4C_Cluster.API.Services
{
    public class CharacterService : Character.CharacterBase
    {
        private class TmpChar {
            public string Name;
            public uint Level;
            public uint Race;
            public UInt32 Agility { get; set; }
            public UInt32 Endurence { get; set; }
            public UInt32 Intelligence { get; set; }           
            public UInt32 Luck { get; set; }
            public UInt32 Strength { get; set; }
            public UInt32 Willpower { get; set; }
            public UInt32 Wisdom { get; set; }
            public UInt32 MaximumHealthPoint { get; set; }
            public UInt32 HealthPoint { get; set; }
            public UInt32 MaximumManaPoint { get; set; }
            public UInt32 ManaPoint { get; set; }
        }
        private static List<TmpChar> chars = new List<TmpChar>();

        public override Task<GetCharactersReply> GetCharacters(GetCharactersRequest request, ServerCallContext context)
        {
            var r = new GetCharactersReply();
            r.Caracters.Add(new GetCharactersReply.Types.GetCharactersReply_Character()
            {
                Name = "Paul",
                Level = 1,
                Race = 10017,

            });
            return Task.FromResult( r);
        }

        public override Task<CreateCharacterReply> CreateCharacter(CreateCharacterRequest request, ServerCallContext context)
        {
            chars.Add(new TmpChar()
            {
                Agility = request.Agility,
                Endurence = request.Endurence,
                HealthPoint = request.HealthPoint,
                Intelligence = request.Intelligence,
                Level = request.Wisdom,
                Luck = request.Luck,
                ManaPoint = request.ManaPoint,
                MaximumHealthPoint = request.MaximumHealthPoint,
                MaximumManaPoint = request.MaximumManaPoint,
                Name = request.Name,
                Race = request.Race,
                Strength= request.Strength,
                Willpower= request.Willpower,
                Wisdom = request.Wisdom,

            });
            return Task.FromResult(new CreateCharacterReply() { Result = true });
        }

        public override Task<UpdateCharacterReply> UpdateCharacter(UpdateCharacterRequest request, ServerCallContext context)
        {
            chars.RemoveAll(w => w.Name == request.Name);
            chars.Add(new TmpChar()
            {
                Agility = request.Agility,
                Endurence = request.Endurence,
                HealthPoint = request.HealthPoint,
                Intelligence = request.Intelligence,
                Level = request.Wisdom,
                Luck = request.Luck,
                ManaPoint = request.ManaPoint,
                MaximumHealthPoint = request.MaximumHealthPoint,
                MaximumManaPoint = request.MaximumManaPoint,
                Name = request.Name,
                Race = request.Race,
                Strength = request.Strength,
                Willpower = request.Willpower,
                Wisdom = request.Wisdom,

            });
            return Task.FromResult(new UpdateCharacterReply() { Result = true });
        }

        public override Task<DeleteCharacterReply> DeleteCaracter(DeleteCharacterRequest request, ServerCallContext context)
        {
            chars.RemoveAll(c=>c.Name == request.Name);
            return Task.FromResult(new DeleteCharacterReply() { Result = true });
        }

        public override Task<IsCharacterNameUsedReply> IsCharacterNameUsed(IsCharacterNameUsedRequest request, ServerCallContext context)
        {
            return Task.FromResult( new IsCharacterNameUsedReply() { Used = chars.Any(c=>c.Name == request.Name) });
        }
    }
}
