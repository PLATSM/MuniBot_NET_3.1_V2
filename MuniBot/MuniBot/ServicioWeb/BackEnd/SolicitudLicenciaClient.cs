using MuniBot.ServicioWeb.BackEnd.Entities;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace MuniBot.ServicioWeb.BackEnd
{
    public class SolicitudLicenciaClient
    {
        public ResponseQuery InsertAsync(SolicitudLicenciaDTO solicitudLicencia)
        {

            ResponseQuery responseQuery = new ResponseQuery();

            var json = JsonConvert.SerializeObject(solicitudLicencia);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                var responseTask = client.PostAsync("SolicitudLicencia/InsertAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<ResponseQuery>();
                readTask.Wait();
                if (result.IsSuccessStatusCode)
                {
                    responseQuery = readTask.Result;
                }
                else
                {
                    if ((int)result.StatusCode == 401)
                    {
                        responseQuery.error_number = (int)result.StatusCode;
                        responseQuery.error_message = "Su sesión ha expirado, vuelva a iniciar sesión";
                    }
                    else
                    {
                        responseQuery.error_number = -1;
                        responseQuery.error_message = "Sucedió un error, vuelva a intentarlo.";
                    }
                }
            }
            return responseQuery;
        }
        public ResponseQuery UpdatetAsync(SolicitudLicenciaDTO solicitudLicencia)
        {

            ResponseQuery responseQuery = new ResponseQuery();

            var json = JsonConvert.SerializeObject(solicitudLicencia);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                var responseTask = client.PutAsync("SolicitudLicencia/UpdateAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<ResponseQuery>();
                readTask.Wait();
                if (result.IsSuccessStatusCode)
                {
                    responseQuery = readTask.Result;
                }
                else
                {
                    if ((int)result.StatusCode == 401)
                    {
                        responseQuery.error_number = (int)result.StatusCode;
                        responseQuery.error_message = "Su sesión ha expirado, vuelva a iniciar sesión";
                    }
                    else
                    {
                        responseQuery.error_number = -1;
                        responseQuery.error_message = "Sucedió un error, vuelva a intentarlo.";
                    }
                }
            }
            return responseQuery;
        }
        public ResponseQuery DeletetAsync(SolicitudLicenciaDTO solicitudLicencia)
        {

            ResponseQuery responseQuery = new ResponseQuery();

            var json = JsonConvert.SerializeObject(solicitudLicencia);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                var responseTask = client.PutAsync("SolicitudLicencia/DeleteAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<ResponseQuery>();
                readTask.Wait();
                if (result.IsSuccessStatusCode)
                {
                    responseQuery = readTask.Result;
                }
                else
                {
                    if ((int)result.StatusCode == 401)
                    {
                        responseQuery.error_number = (int)result.StatusCode;
                        responseQuery.error_message = "Su sesión ha expirado, vuelva a iniciar sesión";
                    }
                    else
                    {
                        responseQuery.error_number = -1;
                        responseQuery.error_message = "Sucedió un error, vuelva a intentarlo.";
                    }
                }
            }
            return responseQuery;
        }
        public Response<SolicitudLicenciaDTO> GetAsync(SolicitudLicenciaDTO solicitudLicencia)
        {

            var response = new Response<SolicitudLicenciaDTO>();

            var json = JsonConvert.SerializeObject(solicitudLicencia);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                var responseTask = client.PostAsync("SolicitudLicencia/GetAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<Response<SolicitudLicenciaDTO>>();
                readTask.Wait();
                if (result.IsSuccessStatusCode)
                {
                    response = readTask.Result;
                }
                else
                {
                    switch ((int)result.StatusCode)
                    {
                        case 401:
                            response.error_number = (int)result.StatusCode;
                            response.error_message = "Su sesión ha expirado, vuelva a iniciar sesión";
                            break;

                        case 404:
                            response = readTask.Result;
                            response.error_number = (int)result.StatusCode;
                            response.error_message = response.error_message;
                            break;

                        default:
                            response.error_number = -1;
                            response.error_message = "Sucedió un error, vuelva a intentarlo.";
                            break;
                    }
                }
            }
            return response;
        }
    }
}