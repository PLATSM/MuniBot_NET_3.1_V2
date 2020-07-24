using MuniBot.ServicioWeb.BackEnd.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MuniBot.ServicioWeb.BackEnd
{
    public class EstablecimientoClaseClient
    {
        public Response<IEnumerable<EstablecimientoClaseDTO>> GetAllAsync(EstablecimientoClaseDTO establecimientoClaseDTO)
        {
            var response = new Response<IEnumerable<EstablecimientoClaseDTO>>();

            var json = JsonConvert.SerializeObject(establecimientoClaseDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + establecimientoClaseDTO.no_token);
                var responseTask = client.PostAsync("EstablecimientoClase/GetAllAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<Response<IEnumerable<EstablecimientoClaseDTO>>>();
                readTask.Wait();
                response = readTask.Result;
            }
            return response;
        }
    }
}
