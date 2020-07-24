using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using MuniBot.Cards;
using MuniBot.Common;
using MuniBot.ServicioWeb.BackEnd;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MuniBot.Dialogs.Login
{
    public class LoginDialog : ComponentDialog
    {
        static string AdaptivePromptId = "LoginCard";

        public LoginDialog()
        {
            AddDialog(new AdaptiveCardPrompt(AdaptivePromptId));
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                SetLogin,
                EndDialog
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> SetLogin(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            AdaptiveCardList adaptiveCard = new AdaptiveCardList();
            var nameCard = adaptiveCard.CreateAttachment(2, "");

            var opts = new PromptOptions
            {
                Prompt = new Activity
                {
                    Attachments = new List<Attachment>() { nameCard },
                    Type = ActivityTypes.Message,
                    //Text = "Please fill this form",
                }
            };

            return await stepContext.PromptAsync(AdaptivePromptId, opts, cancellationToken);
        }
        private async Task<DialogTurnResult> EndDialog(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var stepContextResult = stepContext.Result.ToString();

            JObject InfoCard = JObject.Parse(stepContextResult);
            string id = (string)InfoCard["id"];

            if (id == "LoginCardAceptar")
            {
                string co_documento_identidad = (string)InfoCard["cboTipoDocumento"];
                string nu_documento_identidad = (string)InfoCard["txtNumeroDocumento"];
                string no_contrasena = Funciones.GetSHA256(InfoCard["txtContrasena"].ToString());

                ContribuyenteClient contribuyenteClient = new ContribuyenteClient();
                var result = contribuyenteClient.GetLoginAsync(Globales.id_empresa,co_documento_identidad, nu_documento_identidad, no_contrasena);

                if (result.error_number == 0)
                {
                    Globales.OnSesion = true;
                    Globales.id_contribuyente = result.Data.id_contribuyente;
                    Globales.no_contribuyente = result.Data.no_contribuyente;

                    await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, $"Hola {Globales.no_contribuyente}, en que te puedo ayudar?"), cancellationToken);
                }
                else
                {
                    Globales.OnSesion = false;
                    Globales.id_contribuyente = 0;
                    Globales.no_token = string.Empty;
                    Globales.no_contribuyente = string.Empty;

                    await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, $"{result.error_message}"), cancellationToken);
                }
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
            }

            /*
                        var promptOptions = new PromptOptions
                        {
                            Prompt = MessageFactory.Text(value)
                        };
                        return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
            */
            //await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
            //return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);

            //await stepContext.Context.SendActivityAsync($"INPUT: {stepContext.Result}");
            return await stepContext.NextAsync();
        }
    }
}
