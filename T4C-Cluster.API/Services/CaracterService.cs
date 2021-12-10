using Grpc.Core;
using System.Threading.Tasks;

namespace T4C_Cluster.API.Services
{
    public class CaracterService : Caracter.CaracterBase
    {
        public override Task<GetCharactersReply> GetCharacters(GetCharactersRequest request, ServerCallContext context)
        {
            var r = new GetCharactersReply();
            r.Characters.Add(new GetCharactersReply.Types.GetCharactersReply_Character()
            {
                Name = "Paul",
                Level = 1,
                Race = 10017,

            });
            return Task.FromResult( r);
        }
    }
}
