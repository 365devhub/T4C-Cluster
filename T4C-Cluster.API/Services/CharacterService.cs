using Grpc.Core;
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
