using MuniBot.ServicioWeb.BackEnd.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MuniBot.ServicioWeb.BackEnd
{
    public class GeneralDetClient
    {
        public Response<IEnumerable<GeneralDetDTO>> GetAllAsync(GeneralDetDTO GeneralDetDTO)
        {
            var response = new Response<IEnumerable<GeneralDetDTO>>();

            var json = JsonConvert.SerializeObject(GeneralDetDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GeneralDetDTO.no_token);
                var responseTask = client.PostAsync("GeneralDet/GetAllAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<Response<IEnumerable<GeneralDetDTO>>>();
                readTask.Wait();
                response = readTask.Result;
            }
            return response;
        }
    }
}
