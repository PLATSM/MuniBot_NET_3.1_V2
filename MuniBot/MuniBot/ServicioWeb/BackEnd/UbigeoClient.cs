using MuniBot.ServicioWeb.BackEnd.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MuniBot.ServicioWeb.BackEnd
{
    public class UbigeoClient
    {
        public Response<UbigeoDTO> GetAsync(UbigeoDTO ubigeoDTO)
        {
            var response = new Response<UbigeoDTO>();

            var json = JsonConvert.SerializeObject(ubigeoDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                var responseTask = client.PostAsync("Ubigeo/GetAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<Response<UbigeoDTO>>();
                readTask.Wait();
                response = readTask.Result;

            }
            return response;
        }
        public Response<IEnumerable<UbigeoDTO>> GetAllAsync(UbigeoDTO ubigeoDTO)
        {
            var response = new Response<IEnumerable<UbigeoDTO>>();

            var json = JsonConvert.SerializeObject(ubigeoDTO);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:4020/api/");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ubigeoDTO.no_token);
                var responseTask = client.PostAsync("Ubigeo/GetAllAsync", data);
                responseTask.Wait();

                var result = responseTask.Result;
                var readTask = result.Content.ReadAsAsync<Response<IEnumerable<UbigeoDTO>>>();
                readTask.Wait();
                response = readTask.Result;
            }
            return response;
        }
    }
}
