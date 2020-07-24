// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using MuniBot.Cards;
using MuniBot.Common;
using MuniBot.ServicioWeb.PIDE.RENIEC;
using MuniBot.ServicioWeb.PIDE.SUNAT;

namespace MuniBot.Bots
{
    public class WelcomeBot<T> : RootDialogBot<T> where T : Dialog
    {
        // private readonly IMenuBot _menuBot;

        public WelcomeBot(ConversationState conversationState, UserState userState, T dialog, ILogger<RootDialogBot<T>> logger,IReniec reniecPIDE,ISunat sunatPIDE)
            : base(conversationState, userState, dialog, logger, reniecPIDE, sunatPIDE)
        {
            // _menuBot = menuBot;
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded,ITurnContext<IConversationUpdateActivity> turnContext,CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    Globales.OnSesion = false;
                    Globales.id_contribuyente = 0;
                    Globales.no_token = string.Empty;
                    Globales.no_contribuyente = string.Empty;

                    AdaptiveCardList adaptiveCard = new AdaptiveCardList();
                    var nameCard = adaptiveCard.CreateAttachment(0, "");
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(nameCard), cancellationToken);
                    await Task.Delay(500);

                    await turnContext.SendActivityAsync(MenuBot.Buttons(0,"En que te puedo ayudar?\n\n puedes utilizar los botones de la parte inferior."), cancellationToken);
                }
            }
        }
    }
}