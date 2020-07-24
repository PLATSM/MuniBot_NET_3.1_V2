using MuniBot.ServicioWeb.BackEnd.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MuniBot.ServicioWeb.BackEnd
{
    public class EstablecimientoSubClaseClient
    {
        public Response<IEnumerable<EstablecimientoSubclaseDTO>> GetAllAsync(EstablecimientoSubclaseDTO establecimientoSubclaseDTO)
        {
            var response = new Response<IEnumerable<EstablecimientoSubclaseDTO>>();

            var json = JsonConvert.SerializeObject(establecimientoSubclaseDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + establecimientoSubclaseDTO.no_token);
                var responseTask = client.PostAsync("EstablecimientoSubclase/GetAllAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<Response<IEnumerable<EstablecimientoSubclaseDTO>>>();
                readTask.Wait();
                response = readTask.Result;

            }
            return response;
        }
    }
}
