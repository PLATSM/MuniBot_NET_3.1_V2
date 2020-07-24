using MuniBot.ServicioWeb.BackEnd.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MuniBot.ServicioWeb.BackEnd
{
    public class EstablecimientoCategoriaClient
    {
        public Response<IEnumerable<EstablecimientoCategoriaDTO>> GetAllAsync(EstablecimientoCategoriaDTO establecimientoCategoriaDTO)
        {
            var response = new Response<IEnumerable<EstablecimientoCategoriaDTO>>();

            var json = JsonConvert.SerializeObject(establecimientoCategoriaDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + establecimientoCategoriaDTO.no_token);
                var responseTask = client.PostAsync("EstablecimientoCategoria/GetAllAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<Response<IEnumerable<EstablecimientoCategoriaDTO>>>();
                readTask.Wait();
                response = readTask.Result;
            }
            return response;
        }
    }
}
