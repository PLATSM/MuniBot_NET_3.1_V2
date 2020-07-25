using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using MuniBot.Cards;
using MuniBot.Common;
using MuniBot.ServicioWeb.BackEnd;
using MuniBot.ServicioWeb.BackEnd.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniBot.Dialogs.LicenciaFuncionamiento
{
    public class CrearLicenciaFuncionamientoDialog: ComponentDialog
    {
        private const string ExitOption = "Salir";

        List<string> CodigoList = new List<string>();
        List<string> DescripcionList = new List<string>();

        public CrearLicenciaFuncionamientoDialog() : base(nameof(CrearLicenciaFuncionamientoDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new NumberPrompt<decimal>(nameof(NumberPrompt<decimal>)));
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                SetTipoLicencia,
                SetEstablecimientoClase,
                SetEstablecimientoSubclase,
                SetEstablecimientoCategoria,
                SetNombreComercial,
                SetDistrito,
                SetDireccion,
                SetArea,
                SetConfirmarDatos,
                EndDialog
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> SetTipoLicencia(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            GeneralDetDTO generalDetDTO = new GeneralDetDTO()
            {
                nu_general_cab = 4,
                co_general_det = "",
                fl_inactivo = "0"
            };

            CodigoList.Clear();
            DescripcionList.Clear();

            GeneralDetClient generalDetClient = new GeneralDetClient();
            var result = generalDetClient.GetAllAsync(generalDetDTO);
            if (result.error_number == 0)
            {
                foreach (GeneralDetDTO item in result.Data)
                {
                    CodigoList.Add(item.co_general_det);
                    DescripcionList.Add(item.no_general_det);
                }
            }
            CodigoList.Add("0000");
            DescripcionList.Add(ExitOption);

            var options = DescripcionList.ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor seleccione un **Tipo de Licencia**:"),
                Choices = ChoiceFactory.ToChoices(options),
                Style = ListStyle.List
            };

            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> SetEstablecimientoClase(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = (FoundChoice)stepContext.Result;

            if (choice.Value == ExitOption)
            {
                await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, "En que te puedo ayudar?\n\n puedes utilizar los botones de la parte inferior."), cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }

            stepContext.Values["co_tipo_licencia"] = CodigoList[choice.Index];
            stepContext.Values["no_tipo_licencia"] = choice.Value;

            EstablecimientoClaseDTO establecimientoClaseDTO = new EstablecimientoClaseDTO()
            {
                co_establecimiento_clase = "",
                no_establecimiento_clase = "",
                fl_inactivo = "0"
            };

            CodigoList.Clear();
            DescripcionList.Clear();

            EstablecimientoClaseClient establecimientoClaseClient = new EstablecimientoClaseClient();
            var result = establecimientoClaseClient.GetAllAsync(establecimientoClaseDTO);
            if (result.error_number == 0)
            {
                foreach (EstablecimientoClaseDTO item in result.Data)
                {
                    CodigoList.Add(item.co_establecimiento_clase);
                    DescripcionList.Add(item.no_establecimiento_clase);
                }
            }
            CodigoList.Add("0");
            DescripcionList.Add(ExitOption);

            var options = DescripcionList.ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor seleccione una **Clase de Establecimiento**:"),
                Choices = ChoiceFactory.ToChoices(options),
                Style = ListStyle.List
            };

            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> SetEstablecimientoSubclase(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = (FoundChoice)stepContext.Result;

            if (choice.Value == ExitOption)
            {
                await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, "En que te puedo ayudar?\n\n puedes utilizar los botones de la parte inferior."), cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }

            stepContext.Values["co_establecimiento_clase"] = CodigoList[choice.Index];
            stepContext.Values["no_establecimiento_clase"] = choice.Value;


            EstablecimientoSubclaseDTO establecimientoSubClaseDTO = new EstablecimientoSubclaseDTO()
            {
                co_establecimiento_clase = CodigoList[choice.Index],
                co_establecimiento_subclase = "",
                no_establecimiento_subclase = "",
                fl_inactivo = "0"
            };

            CodigoList.Clear();
            DescripcionList.Clear();

            EstablecimientoSubClaseClient establecimientoSubClaseClient = new EstablecimientoSubClaseClient();
            var result = establecimientoSubClaseClient.GetAllAsync(establecimientoSubClaseDTO);
            if (result.error_number == 0)
            {
                foreach (EstablecimientoSubclaseDTO item in result.Data)
                {
                    CodigoList.Add(item.co_establecimiento_subclase);
                    DescripcionList.Add(item.no_establecimiento_subclase);
                }
            }
            CodigoList.Add("0");
            DescripcionList.Add(ExitOption);

            var options = DescripcionList.ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor seleccione una **Sub-Clase de Establecimiento**:"),
                Choices = ChoiceFactory.ToChoices(options),
                Style = ListStyle.List
            };

            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> SetEstablecimientoCategoria(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = (FoundChoice)stepContext.Result;

            if (choice.Value == ExitOption)
            {
                await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, "En que te puedo ayudar?\n\n puedes utilizar los botones de la parte inferior."), cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }

            stepContext.Values["co_establecimiento_subclase"] = CodigoList[choice.Index];
            stepContext.Values["no_establecimiento_subclase"] = choice.Value;

            EstablecimientoCategoriaDTO establecimientoCategoriaDTO = new EstablecimientoCategoriaDTO()
            {
                co_establecimiento_clase = Convert.ToString(stepContext.Values["co_establecimiento_clase"]),
                co_establecimiento_subclase = CodigoList[choice.Index],
                co_establecimiento_categoria = "",
                no_establecimiento_categoria = "",
                fl_inactivo = "0"
            };

            CodigoList.Clear();
            DescripcionList.Clear();

            EstablecimientoCategoriaClient establecimientoCategoriaClient = new EstablecimientoCategoriaClient();
            var result = establecimientoCategoriaClient.GetAllAsync(establecimientoCategoriaDTO);
            if (result.error_number == 0)
            {
                foreach (EstablecimientoCategoriaDTO item in result.Data)
                {
                    CodigoList.Add(item.co_establecimiento_categoria);
                    DescripcionList.Add(item.no_establecimiento_categoria);
                }
            }
            CodigoList.Add("0");
            DescripcionList.Add(ExitOption);

            var options = DescripcionList.ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor seleccione una **Categoria de Establecimiento**:"),
                Choices = ChoiceFactory.ToChoices(options),
                Style = ListStyle.List
            };

            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> SetNombreComercial(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = (FoundChoice)stepContext.Result;

            if (choice.Value == ExitOption)
            {
                await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, "En que te puedo ayudar?\n\n puedes utilizar los botones de la parte inferior."), cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }

            stepContext.Values["co_establecimiento_categoria"] = CodigoList[choice.Index];
            stepContext.Values["no_establecimiento_categoria"] = choice.Value;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("por favor ingresa el **Nombre Comercial del Establecimiento**:") };

            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> SetDistrito(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["no_comercial"] = (string)stepContext.Result;

            UbigeoDTO UbigeoDTO = new UbigeoDTO()
            {
                co_tipo = "3", // Distrito
                co_ubigeo = Globales.co_ubigeo,
                no_distrito = "",
                fl_inactivo = "0"
            };

            CodigoList.Clear();
            DescripcionList.Clear();

            UbigeoClient UbigeoClient = new UbigeoClient();
            var result = UbigeoClient.GetAllAsync(UbigeoDTO);
            if (result.error_number == 0)
            {
                foreach (UbigeoDTO item in result.Data)
                {
                    CodigoList.Add(item.co_ubigeo);
                    DescripcionList.Add(item.no_distrito);
                }
            }
            CodigoList.Add("0");
            DescripcionList.Add(ExitOption);

            var options = DescripcionList.ToList();

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("Por favor seleccione el **Distrito** donde se ubica el establecimiento:"),
                Choices = ChoiceFactory.ToChoices(options),
                Style = ListStyle.List
            };

            // Prompt the user for a choice.
            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> SetDireccion(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = (FoundChoice)stepContext.Result;

            if (choice.Value == ExitOption)
            {
                await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, "En que te puedo ayudar?\n\n puedes utilizar los botones de la parte inferior."), cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }

            stepContext.Values["co_ubigeo"] = CodigoList[choice.Index]; 
            stepContext.Values["no_distrito"] = choice.Value;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("por favor ingresa la **Dirección del Establecimiento**:") };

            return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> SetArea(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["no_direccion_solicitud"] = (string)stepContext.Result;

            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text("por favor ingresa el **Area m2**:") };

            return await stepContext.PromptAsync(nameof(NumberPrompt<decimal>), promptOptions, cancellationToken);
        }
        private async Task<DialogTurnResult> SetConfirmarDatos(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["nu_area"] = Convert.ToString(stepContext.Result);
            stepContext.Values["no_area"] = Convert.ToString(stepContext.Result);

            // Obtiene la información del contribuyente
            ContribuyenteClient contribuyenteClient = new ContribuyenteClient();
            var result = contribuyenteClient.GetAsync(Globales.id_contribuyente, Globales.no_token);

            if (result.error_number == 0)
            {
                stepContext.Values["no_contribuyente"] = result.Data.no_contribuyente;
                stepContext.Values["co_tipo_persona"] = result.Data.co_tipo_persona;
                stepContext.Values["no_tipo_persona"] = result.Data.no_tipo_persona;
                stepContext.Values["co_documento_identidad"] = result.Data.co_documento_identidad;
                stepContext.Values["nu_documento_identidad"] = result.Data.nu_documento_identidad;
                stepContext.Values["nu_telefono"] = result.Data.nu_telefono;
                stepContext.Values["no_direccion_contribuyente"] = result.Data.no_direccion;
                stepContext.Values["no_correo_electronico"] = result.Data.no_correo_electronico;

                var dataJson = JsonConvert.SerializeObject(stepContext.Values);

                // Mostrar Información ingresada
                AdaptiveCardList adaptiveCardLicencia = new AdaptiveCardList();
                var nameCard = adaptiveCardLicencia.CreateAttachment(6, dataJson);
                await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(nameCard), cancellationToken);
                await Task.Delay(500);

                // Mostrar Opciones
                string[] ListOptions = new string[]
                {
                    "Confirmar Información", "Rechazar Información",
                };

                var options = ListOptions.ToList();

                var promptOptions = new PromptOptions
                {
                    Prompt = MessageFactory.Text("Por favor revisa la información que has ingresado y luego selecciona una opción:"),
                    Choices = ChoiceFactory.ToChoices(options),
                    Style = ListStyle.List
                };

                return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
            }
            else
            {
                await Task.Delay(500);
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(result.error_message), cancellationToken);
                return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            }
        }
        private async Task<DialogTurnResult> EndDialog(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = (FoundChoice)stepContext.Result;

            if (choice.Index == 0) //Confirmar
            {

                SolicitudLicenciaDTO solicitudLicenciaDTO = new SolicitudLicenciaDTO();
                solicitudLicenciaDTO.id_contribuyente = Globales.id_contribuyente;
                solicitudLicenciaDTO.id_empresa = Globales.id_empresa;
                solicitudLicenciaDTO.co_tipo_licencia = Convert.ToString(stepContext.Values["co_tipo_licencia"]);
                solicitudLicenciaDTO.no_comercial = Convert.ToString(stepContext.Values["no_comercial"]);
                solicitudLicenciaDTO.co_establecimiento_clase = Convert.ToString(stepContext.Values["co_establecimiento_clase"]);
                solicitudLicenciaDTO.co_establecimiento_subclase = Convert.ToString(stepContext.Values["co_establecimiento_subclase"]);
                solicitudLicenciaDTO.co_establecimiento_categoria = Convert.ToString(stepContext.Values["co_establecimiento_categoria"]);
                solicitudLicenciaDTO.no_direccion_solicitud = Convert.ToString(stepContext.Values["no_direccion_solicitud"]);
                solicitudLicenciaDTO.co_ubigeo = Convert.ToString(stepContext.Values["co_ubigeo"]);
                solicitudLicenciaDTO.nu_area = Convert.ToDecimal(stepContext.Values["nu_area"]);
                solicitudLicenciaDTO.id_usuario_creacion = 2;// (2=Bot)

                SolicitudLicenciaClient solicitudLicenciaClient = new SolicitudLicenciaClient();
                var result = solicitudLicenciaClient.InsertAsync(solicitudLicenciaDTO);

                if (result.error_number == 0)
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Se ha creado su solicitud exitosamente.\n\n su número de solicitud es: **{DateTime.Now.Date.Year.ToString("0000-")}{result.id_identity.ToString("000000")}**\n\n se encuentra en estado **Pendiente de Aprobación**."), cancellationToken);
                else
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text($"{result.error_message}"), cancellationToken);
            }
            else //Rechazar
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("La información ingresada ha sido rechazada."), cancellationToken);
            }

            await Task.Delay(500);
            await stepContext.Context.SendActivityAsync(MenuBot.Buttons(0, "En que te puedo ayudar?\n\n puedes utilizar los botones de la parte inferior."), cancellationToken);
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
