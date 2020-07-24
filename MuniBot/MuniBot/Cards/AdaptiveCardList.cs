using AdaptiveCards.Templating;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MuniBot.Cards
{
    public class AdaptiveCardList
    {
        private readonly string[] _path =
        {
            Path.Combine(".", "Cards/json", "WelcomeCard.json"),
            Path.Combine(".", "Cards/json", "PersonaNaturalNewCard.json"),
            Path.Combine(".", "Cards/json", "LoginCard.json"),
            Path.Combine(".", "Cards/json", "SolicitudLicenciaNewCard.json"),
            Path.Combine(".", "Cards/json", "PersonaJuridicaNewCard.json"),
            Path.Combine(".", "Cards/json", "SolicitudLicenciaList.json"),
            Path.Combine(".", "Cards/json", "SolicitudLicenciaGetCard.json"),
            Path.Combine(".", "Cards/json", "SolicitudLicenciaAddCard.json"),
            Path.Combine(".", "Cards/json", "ContribuyenteGetCard.json"),
        };

        public Attachment CreateAttachment(int Index,string dataJson)
        {
            if (string.IsNullOrEmpty(dataJson))
            {
                var adaptiveCardJson = File.ReadAllText(_path[Index]);
                var adaptiveCardAttachment = new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JsonConvert.DeserializeObject(adaptiveCardJson),
                };
                return adaptiveCardAttachment;
            }
            else
            {
                var CardJson = File.ReadAllText(_path[Index]);

                var transformer = new AdaptiveTransformer();
                var cardJson = transformer.Transform(CardJson, dataJson);

                var adaptiveCardAttachment = new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = JsonConvert.DeserializeObject(cardJson),
                };
                return adaptiveCardAttachment;
            }
        }
    }
}
