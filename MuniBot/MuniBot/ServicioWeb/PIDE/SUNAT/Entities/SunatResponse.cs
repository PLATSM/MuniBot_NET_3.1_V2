using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuniBot.ServicioWeb.PIDE.SUNAT.Entities
{
    public class SunatResponse
    {
        public SunatDatos sunatDatos { get; set; }
        public string coResultado { get; set; }
        public string deResultado { get; set; }

    }
}
