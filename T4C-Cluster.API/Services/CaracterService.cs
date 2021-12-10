using Grpc.Core;
using System.Threading.Tasks;

namespace T4C_Cluster.API.Services
{
    public class CaracterService : Caracter.CaracterBase
    {
        public override Task<GetCaractersReply> GetCaracters(GetCaractersRequest request, ServerCallContext context)
        {
            var r = new GetCaractersReply();
            r.Caracters.Add(new GetCaractersReply.Types.GetCaractersReply_Caracter()
            {
                Name = "Paul",
                Level = 1,
                Race = 10017,

            });
            return Task.FromResult( r);
        }

        public override Task<DeleteCaracterReply> DeleteCaracter(DeleteCaracterRequest request, ServerCallContext context)
        {
            return Task.FromResult(new DeleteCaracterReply() { Result = true });
        }
    }
}
