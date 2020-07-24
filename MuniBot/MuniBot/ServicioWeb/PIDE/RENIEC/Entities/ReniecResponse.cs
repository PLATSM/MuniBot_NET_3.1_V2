using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MuniBot.ServicioWeb.PIDE.RENIEC.Entities
{
    public class ReniecResponse
    {
        public ReniecDatos reniecDatos {get; set;}
        public string coResultado { get; set; }
        public string deResultado { get; set; }
    }
}
