﻿using Microsoft.Bot.Builder.AI.Luis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuniBot.CognitiveServices.AI_Luis
{
    public interface ILuisService
    {
        LuisRecognizer _luisRecognizer { get; set; }
    }
}
