using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T4C_Cluster.API.Services
{
    public class AccountService : Account.AccountBase
    {
        public override Task<AuthenticateReply> Authenticate(AuthenticateRequest request, ServerCallContext context)
        {
            return Task.FromResult(new AuthenticateReply() { Result = true });
        }
    }
}
