using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using MuniBot.BackEnd;
using MuniBot.BackEnd.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniBot.Dialogs.LicenciaFuncionamiento
{
    public class ListarEstablecimientoCategoriaDialog : ComponentDialog
    {
        public ListarEstablecimientoCategoriaDialog()
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
            List<string> EstablecimientoCategoriaList = new List<string>();

            EstablecimientoCategoriaDTO establecimientoCategoriaDTO = new EstablecimientoCategoriaDTO()
            {
                co_establecimiento_clase = "",
                co_establecimiento_subclase = "",
                co_establecimiento_categoria = "",
                no_establecimiento_categoria = "",
                fl_inactivo = "0"
            };

            EstablecimientoCategoriaClient establecimientoCategoriaClient = new EstablecimientoCategoriaClient();
            var result = establecimientoCategoriaClient.GetAllAsync(establecimientoCategoriaDTO);
            if (result.error_number == 0)
            {
                foreach (EstablecimientoCategoriaDTO item in result.Data)
                {
                    EstablecimientoCategoriaList.Add(item.no_establecimiento_categoria);
                }
            }

            var options = EstablecimientoCategoriaList.ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor seleccione una **Categoria de Establecimiento**:"),
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
