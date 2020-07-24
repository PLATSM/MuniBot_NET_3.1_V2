using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using MuniBot.Dialogs.LicenciaFuncionamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniBot.Dialogs.LicenciaFuncionamiento
{
    public class ListarTipoLicenciaDialog: ComponentDialog
    {
        private readonly string[] _TipoLicencia = new string[]
        {
            "Licencia Indeterminada", "Licencia Temporal", "SALIR"
        };
        public ListarTipoLicenciaDialog()
        {
            // AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
                {
                    MostrarOpciones,
                    FinDialogo
                }));

            InitialDialogId = nameof(WaterfallDialog);
        }
        private async Task<DialogTurnResult> MostrarOpciones(WaterfallStepContext stepContext,CancellationToken cancellationToken)
        {
            var options = _TipoLicencia.ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor seleccione un **Tipo de Licencia**:"),
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