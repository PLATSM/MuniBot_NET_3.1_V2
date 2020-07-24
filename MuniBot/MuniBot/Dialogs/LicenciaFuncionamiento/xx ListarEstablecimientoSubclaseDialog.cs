using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using MuniBot.BackEnd;
using MuniBot.BackEnd.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniBot.Dialogs.LicenciaFuncionamiento
{
    public class ListarEstablecimientoSubclaseDialog : ComponentDialog
    {
        public ListarEstablecimientoSubclaseDialog()
        {
            //AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {
                    MostrarOpciones,
                    FinDialogo
                }));

            InitialDialogId = nameof(WaterfallDialog);
        }
        private async Task<DialogTurnResult> MostrarOpciones(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            List<string> EstablecimientoSubclaseList = new List<string>();

            EstablecimientoSubclaseDTO establecimientoSubClaseDTO = new EstablecimientoSubclaseDTO()
            {
                co_establecimiento_clase = "",
                co_establecimiento_subclase = "",
                no_establecimiento_subclase = "",
                fl_inactivo = "0"
            };

            EstablecimientoSubClaseClient establecimientoSubClaseClient = new EstablecimientoSubClaseClient();
            var result = establecimientoSubClaseClient.GetAllAsync(establecimientoSubClaseDTO);
            if (result.error_number == 0)
            {
                foreach (EstablecimientoSubclaseDTO item in result.Data)
                {
                    EstablecimientoSubclaseList.Add(item.no_establecimiento_subclase);
                }
            }

            var options = EstablecimientoSubclaseList.ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor seleccione una **Sub-Clase de Establecimiento**:"),
                Choices = ChoiceFactory.ToChoices(options),
                Style = ListStyle.List
            };

            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> FinDialogo(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = (FoundChoice)stepContext.Result;

            return await stepContext.EndDialogAsync(choice.Value, cancellationToken);
        }
    }
}
