using System;
using System.Net.Http;
using System.Text;
using MuniBot.ServicioWeb.PIDE.RENIEC.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MuniBot.ServicioWeb.PIDE.RENIEC
{
    public class ReniecClient
    {
        public ReniecResponse GetReniecAsync(string nuDNI)
        {
            var responseReniec = new ReniecResponse();

            ReniecDNI reniecDNI = new ReniecDNI
            {
                dni = nuDNI
            };

            var json = JsonConvert.SerializeObject(reniecDNI);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            //    var content = new FormUrlEncodedContent(new[]
            //    {
            //     new KeyValuePair<string, string>("dni", "25629432")
            //});

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.municallao.gob.pe/pide/public/v1/consulta-dni");
                var result = client.PostAsync(client.BaseAddress, data).Result;
                //var result = client.PostAsync("", content).Result;
                string resultContent = result.Content.ReadAsStringAsync().Result;

                JObject DNISearch = JObject.Parse(resultContent);

                // Obtener la propiedades result en una lista 
                //IList<JToken> results = DNISearch["consultarResponse"]["return"]["datosPersona"].Children().ToList();

                ReniecDatos personaNatural = new ReniecDatos();

                if (resultContent.IndexOf("prenombres") >= 0)
                    personaNatural.prenombres = DNISearch["consultarResponse"]["return"]["datosPersona"]["prenombres"].ToString();

                if (resultContent.IndexOf("apPrimer") >= 0)
                    personaNatural.apPrimer = DNISearch["consultarResponse"]["return"]["datosPersona"]["apPrimer"].ToString();

                if (resultContent.IndexOf("apSegundo") >= 0)
                    personaNatural.apSegundo = DNISearch["consultarResponse"]["return"]["datosPersona"]["apSegundo"].ToString();

                if (resultContent.IndexOf("direccion") >= 0)
                    personaNatural.direccion = DNISearch["consultarResponse"]["return"]["datosPersona"]["direccion"].ToString();

                if (resultContent.IndexOf("estadoCivil") >= 0)
                    personaNatural.estadoCivil = DNISearch["consultarResponse"]["return"]["datosPersona"]["estadoCivil"].ToString();

                if (resultContent.IndexOf("foto") >= 0)
                    personaNatural.foto = DNISearch["consultarResponse"]["return"]["datosPersona"]["foto"].ToString();

                if (resultContent.IndexOf("restriccion") >= 0)
                    personaNatural.restriccion = DNISearch["consultarResponse"]["return"]["datosPersona"]["restriccion"].ToString();

                if (resultContent.IndexOf("ubigeo") >= 0)
                    personaNatural.ubigeo = DNISearch["consultarResponse"]["return"]["datosPersona"]["ubigeo"].ToString();

                responseReniec.reniecDatos = personaNatural;
                responseReniec.coResultado = DNISearch["consultarResponse"]["return"]["coResultado"].ToString();
                responseReniec.deResultado = DNISearch["consultarResponse"]["return"]["deResultado"].ToString();

            }
            return responseReniec;
        }
    }
}
