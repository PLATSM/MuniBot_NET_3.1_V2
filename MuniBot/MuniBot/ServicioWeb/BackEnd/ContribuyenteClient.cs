using MuniBot.ServicioWeb.BackEnd.Entities;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace MuniBot.ServicioWeb.BackEnd
{
    public class ContribuyenteClient
    {
        public ResponseQuery InsertAsync(ContribuyenteDTO contribuyente)
        {
            ResponseQuery responseQuery = new ResponseQuery();

            var json = JsonConvert.SerializeObject(contribuyente);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                var responseTask = client.PostAsync("Contribuyente/InsertAsync", data);
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
        public ResponseQuery UpdatetAsync(ContribuyenteDTO contribuyente)
        {
            ResponseQuery responseQuery = new ResponseQuery();

            var json = JsonConvert.SerializeObject(contribuyente);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                var responseTask = client.PutAsync("Contribuyente/UpdateAsync", data);
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
        public ResponseQuery DeletetAsync(ContribuyenteDTO contribuyente)
        {
            ResponseQuery responseQuery = new ResponseQuery();

            var json = JsonConvert.SerializeObject(contribuyente);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                var responseTask = client.PutAsync("Contribuyente/DeleteAsync", data);
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
        public Response<ContribuyenteDTO> GetLoginAsync(int id_empresa, string co_documento_identidad, string nu_documento_identidad, string no_contrasena)
        {
            var response = new Response<ContribuyenteDTO>();

            ContribuyenteDTO contribuyente = new ContribuyenteDTO
            {
                id_empresa = id_empresa,
                co_documento_identidad = co_documento_identidad,
                nu_documento_identidad = nu_documento_identidad,
                no_contrasena = no_contrasena
            };

            var json = JsonConvert.SerializeObject(contribuyente);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                var responseTask = client.PostAsync("Contribuyente/GetLoginAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<Response<ContribuyenteDTO>>();
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
        public Response<ContribuyenteDTO> GetAsync(int id_contribuyente, string no_token)
        {
            var response = new Response<ContribuyenteDTO>();

            ContribuyenteDTO contribuyente = new ContribuyenteDTO
            {
                id_contribuyente = id_contribuyente,
                no_token = no_token
            };

            var json = JsonConvert.SerializeObject(contribuyente);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + contribuyente.no_token);
                var responseTask = client.PostAsync("Contribuyente/GetAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<Response<ContribuyenteDTO>>();
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
        public Response<DataJsonDTO> GetJsonAsync(int id_contribuyente, string no_token)
        {
            var response = new Response<DataJsonDTO>();

            ContribuyenteDTO contribuyente = new ContribuyenteDTO
            {
                id_contribuyente = id_contribuyente,
                no_token = no_token
            };

            var json = JsonConvert.SerializeObject(contribuyente);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + contribuyente.no_token);
                var responseTask = client.PostAsync("Contribuyente/GetJsonAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<Response<DataJsonDTO>>();
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
