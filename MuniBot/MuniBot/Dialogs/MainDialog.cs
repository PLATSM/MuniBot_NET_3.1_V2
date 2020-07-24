using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using MuniBot.Cards;
using MuniBot.CognitiveServices.AI_Luis;
using MuniBot.CognitiveServices.AI_QnA;
using MuniBot.Common;
using MuniBot.Dialogs.LicenciaFuncionamiento;
using MuniBot.Dialogs.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniBot.Dialogs
{
    public class MainDialog: ComponentDialog
    {
        private readonly ILuisService _luisService;
        private readonly IQnAMakerAIService _qnaMakerAIService;
        private readonly UserState _userState;

        public MainDialog(ILuisService luisService, IQnAMakerAIService qnaMakerAIService, UserState userState)
        {
            _luisService = luisService;
            _qnaMakerAIService = qnaMakerAIService;
            _userState = userState;

            var waterfallSteps = new WaterfallStep[]
            {
                InitialProcess,
                FinalProcess
            };
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            InitialDialogId = nameof(WaterfallDialog);
            AddDialog(new CrearLicenciaFuncionamientoDialog());
            AddDialog(new LoginDialog());
        }

        private async Task<DialogTurnResult> InitialProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var luisResult = await _luisService._luisRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
            return await ManageIntentions(stepContext, luisResult, cancellationToken);
        }

        private async Task<DialogTurnResult> ManageIntentions(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            var topIntent = luisResult.GetTopScoringIntent();

            if (string.IsNullOrEmpty(stepContext.Context.Activity.Text) == false)
            {
                string activityText = stepContext.Context.Activity.Text.ToLower();

                if (activityText.IndexOf("requisito", 0) > -1)
                {
                    topIntent.score = 0;
                }
            }

            if (topIntent.score < 0.5)
            {
                await IntentNone(stepContext, cancellationToken);
            }
            else
            {
                switch (topIntent.intent)
                {
                    case "Saludar":
                        await IntentSaludar(stepContext, cancellationToken);
                        break;
                    case "Agradecer":
                        await IntentAgradecer(stepContext, cancellationToken);
                        break;
                    case "Despedir":
                        await IntentDespedir(stepContext, cancellationToken);
                        break;
                    case "RealizarTramite":
                        await IntentRealizarTramite(stepContext, cancellationToken);
                        break;
                    case "RealizarTramiteLicencia":
                        await IntentRealizarTramiteLicencia(stepContext, cancellationToken);
                        break;
                    case "RealizarTramiteAlcabala":
                        await IntentRealizarTramiteAlcabala(stepContext, cancellationToken);
                        break;
                    case "RealizarTramiteVehicular":
                        await IntentRealizarTramiteVehicular(stepContext, cancellationToken);
                        break;
                    case "ConsultarTramite":
                        await IntentConsultarTramite(stepContext, cancellationToken);
                        break;
                    case "CrearCuenta":
                        await IntentCrearCuenta(stepContext, cancellationToken);
                        break;
                    case "Login":
                        await IntentLogin(stepContext, cancellationToken);
                        break;
                    case "CrearTramiteLicencia":
                        return await IntentCrearTramiteLicencia(stepContext, cancellationToken);
                    case "None":
                        await IntentNone(stepContext, cancellationToken);
                        break;
                    default:
                        break;

                }
            }

            return await stepContext.NextAsync(cancellationToken: cancellationToken); // para que salte al siguiente método
        }

        #region IntentLuis;
        private async Task IntentLogin(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        //private async Task<DialogTurnResult> IntentLogin(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            AdaptiveCardList adaptiveCardLogin = new AdaptiveCardList();
            var loginCard = adaptiveCardLogin.CreateAttachment(2, "");
            await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(loginCard), cancellationToken);
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
            //return await stepContext.BeginDialogAsync(nameof(LoginDialog), cancellationToken: cancellationToken);
        }
        private async Task IntentCrearCuenta(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(5, "Selecciona un tipo de persona"), cancellationToken);
        }
        private async Task<DialogTurnResult> IntentCrearTramiteLicencia(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(CrearLicenciaFuncionamientoDialog), cancellationToken: cancellationToken);
        }
        private async Task IntentConsultarTramite(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(1, "Que trámite deseas realizar?"), cancellationToken);
        }
        private async Task IntentRealizarTramite(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(1, "Que trámite deseas realizar?"), cancellationToken); 
        }
        private async Task IntentRealizarTramiteLicencia(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(2, "Selecciona una opción"), cancellationToken);
        }
        private async Task IntentRealizarTramiteAlcabala(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(3, "Selecciona una opción"), cancellationToken);
        }
        private async Task IntentRealizarTramiteVehicular(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(4, "Selecciona una opción"), cancellationToken);
        }
        private async Task IntentSaludar(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, "Hola, que gusto verte, en que te puedo ayudar?"), cancellationToken);
        }

        private async Task IntentAgradecer(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, "gracias a ti, en que te puedo ayudar?"), cancellationToken);
        }

        private async Task IntentDespedir(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, $"Espero verte pronto {Globales.no_contribuyente}."), cancellationToken);
        }

        private async Task IntentNone(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var resultQnA = await _qnaMakerAIService._qnaMakerResult.GetAnswersAsync(stepContext.Context);

            var score = resultQnA.FirstOrDefault()?.Score; // Capturo el puntaje
            string response = resultQnA.FirstOrDefault()?.Answer; // Capturo la respuesta que devuelve Qna Maker

            if (score >= 0.5)
            {
                await stepContext.Context.SendActivityAsync(response, cancellationToken: cancellationToken);
                await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, "En que te puedo ayudar?"), cancellationToken);
            }
            else
            {
                await Task.Delay(500);
                await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, "No entiendo lo que me dices, puedes utilizar los botones de la parte inferior."), cancellationToken);
            }
        }

        #endregion


        private async Task<DialogTurnResult> FinalProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    }
}
