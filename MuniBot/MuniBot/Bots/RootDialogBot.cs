// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using MuniBot.Cards;
using MuniBot.Common;
using MuniBot.ServicioWeb.BackEnd;
using MuniBot.ServicioWeb.BackEnd.Entities;
using MuniBot.ServicioWeb.PIDE.RENIEC;
using MuniBot.ServicioWeb.PIDE.SUNAT;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MuniBot.Bots
{
    // This IBot implementation can run any type of Dialog. The use of type parameterization is to allows multiple different bots
    // to be run at different endpoints within the same project. This can be achieved by defining distinct Controller types
    // each with dependency on distinct IBot types, this way ASP Dependency Injection can glue everything together without ambiguity.
    // The ConversationState is used by the Dialog system. The UserState isn't, however, it might have been used in a Dialog implementation,
    // and the requirement is that all BotState objects are saved at the end of a turn.
    public class RootDialogBot<T> : ActivityHandler where T : Dialog
    {
        private readonly ISunat _sunatPIDE;
        private readonly IReniec _reniecPIDE;

        protected readonly BotState ConversationState;
        protected readonly Dialog Dialog;
        protected readonly ILogger Logger;
        protected readonly BotState UserState;

        ContribuyenteClient contribuyenteClient = new ContribuyenteClient();

        public RootDialogBot(ConversationState conversationState, UserState userState, T dialog, ILogger<RootDialogBot<T>> logger, IReniec reniecPIDE, ISunat sunatPIDE)
        {
            ConversationState = conversationState;
            UserState = userState;
            Dialog = dialog;
            Logger = logger;
            _reniecPIDE = reniecPIDE;
            _sunatPIDE = sunatPIDE;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default(CancellationToken))
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occurred during the turn.
            await ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await UserState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //Logger.LogInformation("Running dialog with Message Activity.");

            var GoMainDialog = true;

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                var activity = turnContext.Activity;

                if (activity.Text != null && activity.Value == null)
                {
                    switch (activity.Text)
                    {
                        case "Inicio":
                            GoMainDialog = false;
                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                            break;

                        case "Iniciar Sesion":
                            GoMainDialog = false;

                            AdaptiveCardList adaptiveCardLogin = new AdaptiveCardList();
                            var loginCard = adaptiveCardLogin.CreateAttachment(2, "");
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(loginCard), cancellationToken);
                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                            break;

                        case "Cerrar Sesion":
                            GoMainDialog = false;

                            string mensaje = Globales.no_contribuyente;
                            Globales.OnSesion = false;
                            Globales.id_contribuyente = 0;
                            Globales.no_token = string.Empty;
                            Globales.no_contribuyente = string.Empty;

                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"Has cerrado tu sesión {mensaje}, hasta pronto."), cancellationToken);
                            break;

                        case "Foto":
                            GoMainDialog = false;

                            var result = contribuyenteClient.GetAsync(Globales.id_contribuyente, Globales.no_token);
                            if (result.error_number == 0)
                            {
                                var DataJson = JsonConvert.SerializeObject(result.Data);

                                AdaptiveCardList adaptiveCardLicencia = new AdaptiveCardList();
                                var ContribuyenteCard = adaptiveCardLicencia.CreateAttachment(8, DataJson);
                                await turnContext.SendActivityAsync(MessageFactory.Attachment(ContribuyenteCard), cancellationToken);
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                            }
                            else
                            {
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                            }
                            break;

                        case "Crear una cuenta":
                            GoMainDialog = false;
                            await turnContext.SendActivityAsync(MenuBot.Buttons(5, "Seleccione Tipo de Persona:"), cancellationToken);
                            break;

                        case "Crear Cuenta Persona Natural":
                            GoMainDialog = false;
                            AdaptiveCardList adaptiveCardNatural = new AdaptiveCardList();
                            var PersonaNaturalCard = adaptiveCardNatural.CreateAttachment(1, "");
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(PersonaNaturalCard), cancellationToken);
                            await Task.Delay(500);
                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                            break;

                        case "Crear Cuenta Persona Juridica":
                            GoMainDialog = false;
                            AdaptiveCardList adaptiveCardJuridica = new AdaptiveCardList();
                            var PersonaJuridicaCard = adaptiveCardJuridica.CreateAttachment(4, "");
                            await turnContext.SendActivityAsync(MessageFactory.Attachment(PersonaJuridicaCard), cancellationToken);
                            await Task.Delay(500);
                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                            break;

                        case "Seleccionar Trámite":
                            GoMainDialog = false;
                            await turnContext.SendActivityAsync(MenuBot.Buttons(1, ""), cancellationToken);
                            break;

                        case "Trámite Licencia de Funcionamiento":
                            GoMainDialog = false;
                            await turnContext.SendActivityAsync(MenuBot.Buttons(2, ""), cancellationToken);
                            break;

                        case "Nuevo Trámite Licencia de Funcionamiento":
                            GoMainDialog = false;

                            if (Globales.OnSesion == false)
                            {
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"Debe **Iniciar Sesión** para realizar {activity.Text}"), cancellationToken);
                            }
                            break;

                        case "Consultar Licencias de Funcionamiento":
                            GoMainDialog = false;

                            if (Globales.OnSesion == false)
                            {
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"Debe **Iniciar Sesión** para realizar {activity.Text}"), cancellationToken);
                            }
                            else
                            {
                                // Obtiene la información del contribuyente
                                var resultado = contribuyenteClient.GetJsonAsync(Globales.id_contribuyente, Globales.no_token);

                                if (resultado.error_number == 0)
                                {
                                    AdaptiveCardList adaptiveCardLicencia = new AdaptiveCardList();
                                    var LicenciaCard = adaptiveCardLicencia.CreateAttachment(5, resultado.Data.no_data_json);
                                    await turnContext.SendActivityAsync(MessageFactory.Attachment(LicenciaCard), cancellationToken);

                                    await Task.Delay(500);
                                    await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                                }
                                else
                                {
                                    await Task.Delay(500);
                                    await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                                }
                            }
                            break;

                        case "Requisitos Licencia de Funcionamiento":
                            GoMainDialog = true;
                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                            break;

                        case "Trámite Impuesto de Alcabala":
                            GoMainDialog = false;
                            await turnContext.SendActivityAsync(MenuBot.Buttons(3, ""), cancellationToken);
                            break;

                        case "Nuevo Trámite Impuesto de Alcabala":
                            GoMainDialog = false;

                            if (Globales.OnSesion == false)
                            {
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"Debe **Iniciar Sesión** para realizar un {activity.Text}"), cancellationToken);
                            }
                            break;

                        case "Consultar Trámites Impuesto de Alcabala":
                            GoMainDialog = false;

                            if (Globales.OnSesion == false)
                            {
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"Debe **Iniciar Sesión** para realizar {activity.Text}"), cancellationToken);
                            }
                            break;

                        case "Requisitos Impuesto de Alcabala":
                            GoMainDialog = true;
                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                            break;

                        case "Trámite Impuesto Vehicular":
                            GoMainDialog = false;
                            await turnContext.SendActivityAsync(MenuBot.Buttons(4, ""), cancellationToken);
                            break;

                        case "Nuevo Trámite Impuesto Vehicular":
                            GoMainDialog = false;

                            if (Globales.OnSesion == false)
                            {
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"Debe **Iniciar Sesión** para realizar {activity.Text}"), cancellationToken);
                            }
                            break;

                        case "Consultar Trámites Impuesto Vehicular":
                            GoMainDialog = false;

                            if (Globales.OnSesion == false)
                            {
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"Debe **Iniciar Sesión** para realizar {activity.Text}"), cancellationToken);
                            }
                            break;

                        case "Requisitos Impuesto Vehicular":
                            GoMainDialog = true;
                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, ""), cancellationToken);
                            break;
                    }
                }

                if (string.IsNullOrWhiteSpace(activity.Text) && activity.Value != null)
                {
                    //activity.Text = JsonConvert.SerializeObject(activity.Value);

                    JObject InfoCard = JObject.Parse(activity.Value.ToString());
                    string idCard = (string)InfoCard["id"];

                    switch (idCard)
                    {
                        case "LoginCard":
                            GoMainDialog = false;

                            if (
                                string.IsNullOrEmpty(InfoCard.GetValue("cboTipoDocumento").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtNumeroDocumento").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtContrasena").ToString()))
                            {
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Ingrese documento de identidad/contraseña"), cancellationToken);
                            }
                            else
                            {
                                var co_documento_identidad = InfoCard.GetValue("cboTipoDocumento").ToString();
                                var nu_documento_identidad = InfoCard.GetValue("txtNumeroDocumento").ToString();
                                var no_contrasena = Funciones.GetSHA256(InfoCard.GetValue("txtContrasena").ToString());

                                var result = contribuyenteClient.GetLoginAsync(Globales.id_empresa, co_documento_identidad, nu_documento_identidad, no_contrasena);

                                if (result.error_number == 0)
                                {
                                    Globales.OnSesion = true;
                                    Globales.no_token = result.Data.no_token;
                                    Globales.id_contribuyente = result.Data.id_contribuyente;
                                    if (result.Data.co_tipo_persona == "0002") // 0002=Persona Juridica
                                    {
                                        Globales.no_contribuyente = result.Data.no_razon_social;
                                    }
                                    else
                                    {
                                        Globales.no_contribuyente = result.Data.no_nombres + ' ' + result.Data.no_apellido_paterno + ' ' + result.Data.no_apellido_materno;
                                    }

                                    await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"Hola {Globales.no_contribuyente}, en que te puedo ayudar?"), cancellationToken);
                                }
                                else
                                {
                                    Globales.OnSesion = false;
                                    Globales.id_contribuyente = 0;
                                    Globales.no_token = string.Empty;
                                    Globales.no_contribuyente = string.Empty;
                                    await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"{result.error_message}"), cancellationToken);
                                }
                            }
                            break;

                        case "PersonaNaturalNewCard":
                            GoMainDialog = false;
                            if
                            (
                                string.IsNullOrEmpty(InfoCard.GetValue("txtNombres").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtApellidoPaterno").ToString()) ||
                                // string.IsNullOrEmpty(InfoCard.GetValue("txtApellidoMaterno").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtFechaNacimiento").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("cboSexo").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("cboTipoDocumento").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtDocumentoIdentidad").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtCorreoElectronico").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtTelefono").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtDireccion").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtContrasena").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtVerificarContrasena").ToString())
                            )
                            {
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Todos los campos son obligatorios."), cancellationToken);
                            }
                            else
                            {
                                if (InfoCard.GetValue("txtContrasena").ToString() != InfoCard.GetValue("txtVerificarContrasena").ToString())
                                {
                                    await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Contraseña y Verificar Contraseña deben ser iguales."), cancellationToken);
                                }
                                else
                                {

                                    ContribuyenteDTO contribuyenteDTO = new ContribuyenteDTO();
                                    contribuyenteDTO.id_empresa = 1;
                                    contribuyenteDTO.no_nombres = InfoCard.GetValue("txtNombres").ToString();
                                    contribuyenteDTO.no_apellido_paterno = InfoCard.GetValue("txtApellidoPaterno").ToString();
                                    contribuyenteDTO.no_apellido_materno = InfoCard.GetValue("txtApellidoMaterno").ToString();
                                    contribuyenteDTO.fe_nacimiento = InfoCard.GetValue("txtFechaNacimiento").ToString();
                                    contribuyenteDTO.co_sexo = InfoCard.GetValue("cboSexo").ToString();
                                    contribuyenteDTO.co_documento_identidad = InfoCard.GetValue("cboTipoDocumento").ToString();
                                    contribuyenteDTO.nu_documento_identidad = InfoCard.GetValue("txtDocumentoIdentidad").ToString();
                                    contribuyenteDTO.no_correo_electronico = InfoCard.GetValue("txtCorreoElectronico").ToString();
                                    contribuyenteDTO.nu_telefono = InfoCard.GetValue("txtTelefono").ToString();
                                    contribuyenteDTO.no_direccion = InfoCard.GetValue("txtDireccion").ToString();
                                    contribuyenteDTO.no_contrasena = InfoCard.GetValue("txtContrasena").ToString();
                                    contribuyenteDTO.no_contrasena_sha256 = Funciones.GetSHA256(InfoCard.GetValue("txtContrasena").ToString());
                                    contribuyenteDTO.id_usuario_creacion = 2; // (2=Bot)

                                    // Verificar Informacion en RENIEC
                                    var numero = _reniecPIDE.VerificarDNI(contribuyenteDTO);

                                    switch (numero)
                                    {
                                        case 0:
                                            var result = contribuyenteClient.InsertAsync(contribuyenteDTO);

                                            if (result.error_number == 0)
                                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Se ha creado su cuenta exitosamente."), cancellationToken);
                                            else
                                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"{result.error_message}"), cancellationToken);
                                            break;

                                        case -1:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Los datos ingresados no coinciden con la información de RENIEC."), cancellationToken);
                                            break;

                                        case 999:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "No se ha encontrado información para el número de DNI."), cancellationToken);
                                            break;

                                        case 1000:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Uno o más datos de la petición no son válidos."), cancellationToken);
                                            break;

                                        case 1001:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "El DNI, RUC y contraseña no corresponden a un usuario válido."), cancellationToken);
                                            break;

                                        case 1002:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "La contraseña para el DNI y RUC está caducada."), cancellationToken);
                                            break;

                                        case 1003:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Se ha alcanzado el límite de consultas permitidas por día."), cancellationToken);
                                            break;

                                        case 1999:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Error desconocido / inesperado."), cancellationToken);
                                            break;

                                        default:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Sucedió un problema, intente otra vez."), cancellationToken);
                                            break;
                                    }
                                }
                            }
                            break;

                        case "PersonaJuridicaNewCard":
                            GoMainDialog = false;
                            if
                            (
                                string.IsNullOrEmpty(InfoCard.GetValue("txtRazonSocial").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtRepresentanteLegal").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("cboTipoDocumento").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtDocumentoIdentidad").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtCorreoElectronico").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtTelefono").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtDireccion").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtContrasena").ToString()) ||
                                string.IsNullOrEmpty(InfoCard.GetValue("txtVerificarContrasena").ToString())
                            )
                            {
                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Todos los campos son obligatorios."), cancellationToken);
                            }
                            else
                            {
                                if (InfoCard.GetValue("txtContrasena").ToString() != InfoCard.GetValue("txtVerificarContrasena").ToString())
                                {
                                    await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Contraseña y Verificar Contraseña deben ser iguales."), cancellationToken);
                                }
                                else
                                {
                                    ContribuyenteDTO contribuyenteDTO = new ContribuyenteDTO();
                                    contribuyenteDTO.id_empresa = 1;
                                    contribuyenteDTO.no_razon_social = InfoCard.GetValue("txtRazonSocial").ToString();
                                    contribuyenteDTO.no_representante_legal = InfoCard.GetValue("txtRepresentanteLegal").ToString();
                                    contribuyenteDTO.co_documento_identidad = InfoCard.GetValue("cboTipoDocumento").ToString();
                                    contribuyenteDTO.nu_documento_identidad = InfoCard.GetValue("txtDocumentoIdentidad").ToString();
                                    contribuyenteDTO.no_correo_electronico = InfoCard.GetValue("txtCorreoElectronico").ToString();
                                    contribuyenteDTO.nu_telefono = InfoCard.GetValue("txtTelefono").ToString();
                                    contribuyenteDTO.no_direccion = InfoCard.GetValue("txtDireccion").ToString();
                                    contribuyenteDTO.no_contrasena = InfoCard.GetValue("txtContrasena").ToString();
                                    contribuyenteDTO.no_contrasena_sha256 = Funciones.GetSHA256(InfoCard.GetValue("txtContrasena").ToString());
                                    contribuyenteDTO.id_usuario_creacion = 2; // (2=Bot)

                                    // Verificar Informacion en SUNAT
                                    var numero = _sunatPIDE.VerificarRUC(contribuyenteDTO);
                                    switch (numero)
                                    {
                                        case 0:
                                            var result = contribuyenteClient.InsertAsync(contribuyenteDTO);

                                            if (result.error_number == 0)
                                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Se ha creado su cuenta exitosamente."), cancellationToken);
                                            else
                                                await turnContext.SendActivityAsync(MenuBot.Buttons(0, $"{result.error_message}"), cancellationToken);
                                            break;

                                        case -1:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Número de RUC errado, no está registrado en SUNAT."), cancellationToken);
                                            break;

                                        case -2:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Los datos no coinciden con la información de SUNAT."), cancellationToken);
                                            break;

                                        case 999:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "No se ha encontrado información para el número RUC ingresado."), cancellationToken);
                                            break;
                                        default:
                                            await turnContext.SendActivityAsync(MenuBot.Buttons(0, "Sucedió un problema, intente otra vez."), cancellationToken);
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            // Run the Dialog with the new message Activity.
            if (GoMainDialog)
            {
                await Dialog.RunAsync(turnContext, ConversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken);
            }
                
        }
    }
}
