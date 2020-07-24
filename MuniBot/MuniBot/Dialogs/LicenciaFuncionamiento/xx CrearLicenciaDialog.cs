using Microsoft.Bot.Builder.Dialogs;
using MuniBot.Dialogs.LicenciaFuncionamiento.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MuniBot.Dialogs.LicenciaFuncionamiento
{
    public class CrearLicenciaDialog:ComponentDialog
    {
        LicenciaFuncionamientoInfo _licenciaFuncionamientoInfo = new LicenciaFuncionamientoInfo();

        public CrearLicenciaDialog() : base(nameof(CrearLicenciaDialog))
        {

            // AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            //AddDialog(new NumberPrompt<int>(nameof(NumberPrompt<int>)));

            AddDialog(new ListarTipoLicenciaDialog());
            AddDialog(new ListarEstablecimientoClaseDialog());
            AddDialog(new ListarEstablecimientoSubclaseDialog());
            AddDialog(new ListarEstablecimientoCategoriaDialog());

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                SetTipoLicencia,
                SetEstablecimientoClase,
                SetEstablecimientoSubclase,
                SetEstablecimientoCategoria,
                EndDialog
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }
        private async Task<DialogTurnResult> SetTipoLicencia(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(ListarTipoLicenciaDialog), null, cancellationToken);
        }
        private async Task<DialogTurnResult> SetEstablecimientoClase(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            _licenciaFuncionamientoInfo.co_tipo_licencia = "hola";

            return await stepContext.BeginDialogAsync(nameof(ListarEstablecimientoClaseDialog), null, cancellationToken);
        }
        private async Task<DialogTurnResult> SetEstablecimientoSubclase(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(ListarEstablecimientoSubclaseDialog), null, cancellationToken);
        }
        private async Task<DialogTurnResult> SetEstablecimientoCategoria(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(ListarEstablecimientoCategoriaDialog), null, cancellationToken);
        }
        private async Task<DialogTurnResult> EndDialog(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var TipoLicencia = stepContext.Context.Activity.Text;

            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    }
}
