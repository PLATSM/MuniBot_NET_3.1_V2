// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EmptyBot v4.9.2

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MuniBot
{
    static class Globales
    {
        public static bool OnSesion = false;
        public static int id_empresa = 1;
        public static int id_contribuyente = 0;
        public static string co_ubigeo = "0701";
        public static string no_token = string.Empty;
        public static string no_contribuyente = string.Empty;
        public static string idCard = string.Empty;
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
