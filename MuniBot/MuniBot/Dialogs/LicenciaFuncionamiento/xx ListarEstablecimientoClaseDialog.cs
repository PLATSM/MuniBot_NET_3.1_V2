using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using MuniBot.BackEnd;
using MuniBot.BackEnd.Entities;
using MuniBot.Dialogs.LicenciaFuncionamiento.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniBot.Dialogs.LicenciaFuncionamiento
{
    
    public class ListarEstablecimientoClaseDialog:ComponentDialog
    {
        private const string LicenciaInfo = "value-LicenciaInfo";
        List<string> NombreClaseList = new List<string>();
        List<string> CodigoClaseList = new List<string>();

        //Response<IEnumerable<EstablecimientoClaseDTO>> result = new Response<IEnumerable<EstablecimientoClaseDTO>>();


        public ListarEstablecimientoClaseDialog()
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
            stepContext.Values[LicenciaInfo] = new LicenciaFuncionamientoInfo();

            EstablecimientoClaseDTO establecimientoClaseDTO = new EstablecimientoClaseDTO()
            {
                co_establecimiento_clase = "",
                no_establecimiento_clase = "",
                fl_inactivo = "0"
            };

            EstablecimientoClaseClient establecimientoClaseClient = new EstablecimientoClaseClient();
            var result = establecimientoClaseClient.GetAllAsync(establecimientoClaseDTO);
            if (result.error_number == 0)
            {
                foreach (EstablecimientoClaseDTO item in result.Data)
                {
                    NombreClaseList.Add(item.no_establecimiento_clase);
                    CodigoClaseList.Add(item.co_establecimiento_clase);
                }
            }

            var options = NombreClaseList.ToList();



            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor seleccione una **Clase de Establecimiento**:"),
                Choices = ChoiceFactory.ToChoices(options),
                Style = ListStyle.List
            };

            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> FinDialogo(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var licenciaFuncionamientoInfo = (LicenciaFuncionamientoInfo)stepContext.Values[LicenciaInfo];

            var choice = (FoundChoice)stepContext.Result;



            //licenciaFuncionamientoInfo.no_establecimiento_clase = choice.Value;

            licenciaFuncionamientoInfo.co_establecimiento_clase = CodigoClaseList[choice.Index];

            var x = CodigoClaseList[choice.Index];


            return await stepContext.EndDialogAsync(stepContext.Values[LicenciaInfo], cancellationToken);
        }
    }
}