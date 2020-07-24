using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuniBot.Common
{
    public static class MenuBot
    {
        public static Activity Buttons(int index,string message)
        {
            var reply = MessageFactory.Text($"{message}");

            switch (index)
            {
                case 0: //Inicio

                    reply.SuggestedActions = new SuggestedActions()
                    {
                        Actions = new List<CardAction>()
                        {
                            new CardAction() { Title = "Trámites", Type = ActionTypes.ImBack, Value = "Seleccionar Trámite", DisplayText = "SEL" },
                            new CardAction() { Title = Globales.OnSesion ? "Cerrar Sesion" : "Iniciar Sesion", Type = ActionTypes.ImBack, Value = Globales.OnSesion ? "Cerrar Sesion" : "Iniciar Sesion" },
                            new CardAction() { Title = "Crear una cuenta", Type = ActionTypes.ImBack, Value = "Crear una cuenta" },
                            new CardAction() { Title = "Contactos", Type = ActionTypes.ImBack, Value = "Contactos" },
                            new CardAction() { Title = "Foto", Type = ActionTypes.ImBack, Value = "Foto" },
                        },
                    };
                    break;

                case 1: // Tipo Trámite
                    reply.SuggestedActions = new SuggestedActions()
                    {
                        Actions = new List<CardAction>()
                        {
                            new CardAction() { Title = "Inicio", Type = ActionTypes.ImBack, Value = "Inicio" },
                            new CardAction() { Title = "Licencia Funcionamiento", Type = ActionTypes.ImBack, Value = "Trámite Licencia de Funcionamiento" },
                            new CardAction() { Title = "Impuesto Alcabala", Type = ActionTypes.ImBack, Value = "Trámite Impuesto de Alcabala" },
                            new CardAction() { Title = "Impuesto Vehicular", Type = ActionTypes.ImBack, Value = "Trámite Impuesto Vehicular"},
                        },
                    };
                    break;
                
                case 2: //Licencia de Funcionamiento
                    reply.SuggestedActions = new SuggestedActions()
                    {
                        Actions = new List<CardAction>()
                        {
                            new CardAction() { Title = "Inicio", Type = ActionTypes.ImBack, Value = "Inicio" },
                            new CardAction() { Title = "Nueva Licencia", Type = ActionTypes.ImBack, Value = "Nuevo Trámite Licencia de Funcionamiento" },
                            new CardAction() { Title = "Consultar Licencias", Type = ActionTypes.ImBack, Value = "Consultar Licencias de Funcionamiento" },
                            new CardAction() { Title = "Requisitos", Type = ActionTypes.ImBack, Value = "Requisitos Licencia de Funcionamiento" },
                        },
                    };
                    break;

                case 3: //Impuesto de Alcabala
                    reply.SuggestedActions = new SuggestedActions()
                    {
                        Actions = new List<CardAction>()
                        {
                            new CardAction() { Title = "Inicio", Type = ActionTypes.ImBack, Value = "Inicio" },
                            new CardAction() { Title = "Nuevo Trámite", Type = ActionTypes.ImBack, Value = "Nuevo Trámite Impuesto de Alcabala" },
                            new CardAction() { Title = "Consultar Trámites", Type = ActionTypes.ImBack, Value = "Consultar Trámites Impuesto de Alcabala" },
                            new CardAction() { Title = "Requisitos", Type = ActionTypes.ImBack, Value = "Requisitos Impuesto de Alcabala" },
                        },
                    };
                    break;

                case 4: //Impuesto Vehicular
                    reply.SuggestedActions = new SuggestedActions()
                    {
                        Actions = new List<CardAction>()
                        {
                            new CardAction() { Title = "Inicio", Type = ActionTypes.ImBack, Value = "Inicio" },
                            new CardAction() { Title = "Nuevo Trámite", Type = ActionTypes.ImBack, Value = "Nuevo Trámite Impuesto Vehicular" },
                            new CardAction() { Title = "Consultar Trámites", Type = ActionTypes.ImBack, Value = "Consultar Trámites Impuesto Vehicular" },
                            new CardAction() { Title = "Requisitos", Type = ActionTypes.ImBack, Value = "Requisitos Impuesto Vehicular" },
                        },
                    };
                    break;

                case 5: //Tipo de Persona
                    reply.SuggestedActions = new SuggestedActions()
                    {
                        Actions = new List<CardAction>()
                        {
                            new CardAction() { Title = "Inicio", Type = ActionTypes.ImBack, Value = "Inicio" },
                            new CardAction() { Title = "Persona Natural", Type = ActionTypes.ImBack, Value = "Crear Cuenta Persona Natural" },
                            new CardAction() { Title = "Persona Juridica", Type = ActionTypes.ImBack, Value = "Crear Cuenta Persona Juridica" },
                        },
                    };
                    break;

            }

            return reply;
        }

    }
}
