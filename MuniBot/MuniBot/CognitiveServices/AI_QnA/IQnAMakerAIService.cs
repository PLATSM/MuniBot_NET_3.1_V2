using Microsoft.Bot.Builder.AI.QnA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuniBot.CognitiveServices.AI_QnA
{
    public interface IQnAMakerAIService
    {
        QnAMaker _qnaMakerResult { get; set; }
    }
}
