using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuniBot.CognitiveServices.AI_QnA
{
    public class QnAMakerAIService: IQnAMakerAIService
    {
        public QnAMaker _qnaMakerResult { get; set; }

        public QnAMakerAIService(IConfiguration configuration)
        {
            _qnaMakerResult = new QnAMaker(new QnAMakerEndpoint { 
            
                KnowledgeBaseId = configuration["QnAMakerBaseID"],
                EndpointKey = configuration["QnAMakerKey"],
                Host = configuration["QnAMakerHostName"]

            });
        }
    }
}
