using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using MuniBot.ServicioWeb.PIDE.SUNAT.Entities;

namespace MuniBot.ServicioWeb.PIDE.SUNAT
{
    public class SunatCliente
    {
        public SunatResponse GetSunatAsync(string nuRUC)
        {
            var responseSunat = new SunatResponse();

            SunatRUC sunatRUC = new SunatRUC
            {
                ruc = nuRUC
            };

            var json = JsonConvert.SerializeObject(sunatRUC);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.municallao.gob.pe/pide/public/v1/consulta-ruc");
                var result = client.PostAsync(client.BaseAddress, data).Result;

                if (result.IsSuccessStatusCode)
                {
                    string resultContent = result.Content.ReadAsStringAsync().Result;

                    // var listado = JsonConvert.DeserializeObject<Rootobject>(resultContent);

                    JObject objResult = JObject.Parse(resultContent);

                    SunatDatos personaJuridica = new SunatDatos();

                    string x = string.Concat("\"ddp_ubigeo\":", ":{\"@type\":\"xsd:string\",\"$\"");

                    if (resultContent.IndexOf(string.Concat("\"ddp_ubigeo\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_ubigeo = objResult["datos"]["list"]["multiRef"]["ddp_ubigeo"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"cod_dep\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.cod_dep = objResult["datos"]["list"]["multiRef"]["cod_dep"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_dep\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_dep = objResult["datos"]["list"]["multiRef"]["desc_dep"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"cod_prov\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.cod_prov = objResult["datos"]["list"]["multiRef"]["cod_prov"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_prov\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_prov = objResult["datos"]["list"]["multiRef"]["desc_prov"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"cod_dist\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.cod_dist = objResult["datos"]["list"]["multiRef"]["cod_dist"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_dist\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_dist = objResult["datos"]["list"]["multiRef"]["desc_dist"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_ciiu\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_ciiu = objResult["datos"]["list"]["multiRef"]["ddp_ciiu"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_ciiu\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_ciiu = objResult["datos"]["list"]["multiRef"]["desc_ciiu"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_estado\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_estado = objResult["datos"]["list"]["multiRef"]["ddp_estado"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_estado\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_estado = objResult["datos"]["list"]["multiRef"]["desc_estado"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_fecact\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_fecact = objResult["datos"]["list"]["multiRef"]["ddp_fecact"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_fecalt\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_fecalt = objResult["datos"]["list"]["multiRef"]["ddp_fecalt"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_fecbaj\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_fecbaj = objResult["datos"]["list"]["multiRef"]["ddp_fecbaj"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_identi\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_identi = objResult["datos"]["list"]["multiRef"]["ddp_identi"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_identi\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_identi = objResult["datos"]["list"]["multiRef"]["desc_identi"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_lllttt\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_lllttt = objResult["datos"]["list"]["multiRef"]["ddp_lllttt"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_nombre\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_nombre = objResult["datos"]["list"]["multiRef"]["ddp_nombre"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_nomvia\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_nomvia = objResult["datos"]["list"]["multiRef"]["ddp_nomvia"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_numer1\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_numer1 = objResult["datos"]["list"]["multiRef"]["ddp_numer1"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_inter1\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_inter1 = objResult["datos"]["list"]["multiRef"]["ddp_inter1"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_nomzon\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_nomzon = objResult["datos"]["list"]["multiRef"]["ddp_nomzon"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_refer1\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_refer1 = objResult["datos"]["list"]["multiRef"]["ddp_refer1"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_flag22\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_flag22 = objResult["datos"]["list"]["multiRef"]["ddp_flag22"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_flag22\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_flag22 = objResult["datos"]["list"]["multiRef"]["desc_flag22"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_numreg\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_numreg = objResult["datos"]["list"]["multiRef"]["ddp_numreg"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_numreg\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_numreg = objResult["datos"]["list"]["multiRef"]["desc_numreg"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_numruc\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_numruc = objResult["datos"]["list"]["multiRef"]["ddp_numruc"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_tipvia\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_tipvia = objResult["datos"]["list"]["multiRef"]["ddp_tipvia"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_tipvia\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_tipvia = objResult["datos"]["list"]["multiRef"]["desc_tipvia"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_tipzon\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_tipzon = objResult["datos"]["list"]["multiRef"]["ddp_tipzon"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_tipzon\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_tipzon = objResult["datos"]["list"]["multiRef"]["desc_tipzon"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_tpoemp\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_tpoemp = objResult["datos"]["list"]["multiRef"]["ddp_tpoemp"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"desc_tpoemp\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.desc_tpoemp = objResult["datos"]["list"]["multiRef"]["desc_tpoemp"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"ddp_secuen\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.ddp_secuen = objResult["datos"]["list"]["multiRef"]["ddp_secuen"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"esActivo\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.esActivo = objResult["datos"]["list"]["multiRef"]["esActivo"]["$"].ToString();

                    if (resultContent.IndexOf(string.Concat("\"esHabido\"", ":{\"@type\":\"xsd:string\",\"$\"")) >= 0)
                        personaJuridica.esHabido = objResult["datos"]["list"]["multiRef"]["esHabido"]["$"].ToString();
/*
                    if (resultContent.IndexOf("getDomicilioLegalReturn") >= 0)
                        personaJuridica.getDomicilioLegalReturn = objResult["datos"]["dlegal"]["list"]["getDomicilioLegalResponse"]["getDomicilioLegalReturn"]["$"].ToString();

                    if (resultContent.IndexOf("getRepLegalesReturn") >= 0)
                        personaJuridica.getRepLegalesReturn = objResult["datos"]["replegal"]["list"]["getRepLegalesResponse"]["getRepLegalesReturn"]["$"].ToString();
*/
                    responseSunat.sunatDatos = personaJuridica;
                    responseSunat.coResultado = "0000";
                    responseSunat.deResultado = "Ok";
                }
                else
                {
                    responseSunat.coResultado = "9999";
                    responseSunat.deResultado = "Bad";
                }
            }
            return responseSunat;
        }

    }
}
