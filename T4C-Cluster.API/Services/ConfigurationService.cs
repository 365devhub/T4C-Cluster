using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T4C_Cluster.API.Services
{
    public class ConfigurationService : Configuration.ConfigurationBase
    {
        /*private readonly ILogger<ConfigurationService> _logger;*/
        public ConfigurationService(/*ILogger<ConfigurationService> logger*/)
        {
            //_logger = logger;
        }

        public override Task<MessageOfTheDayReply> GetMessageOfTheDay(MessageOfTheDayRequest request, ServerCallContext context)
        {
            return Task.FromResult(new MessageOfTheDayReply() { Message = "aaaaa" }); 
        }

        public override Task<PatchServerInformationsReply> GetPatchServerInformations(PatchServerInformationsRequest request, ServerCallContext context)
        {
            return Task.FromResult(new PatchServerInformationsReply() { Lang = 1, Password = "" , Username = "", WebPatchIP = "", ImagePath = "" });
        }

        public override Task<TranslationReply> GetTranslation(TranslationRequest request, ServerCallContext context)
        {
            if (request.TranslationKey == "Authentication.Success") 
                return Task.FromResult(new TranslationReply() { Text = "Authentification Succedded" });
            else if (request.TranslationKey == "Authentication.Failed") 
                return Task.FromResult(new TranslationReply() { Text = "Authentification failed" });
            else
                return Task.FromResult(new TranslationReply() { Text = request.TranslationKey });
        }
    }
}
