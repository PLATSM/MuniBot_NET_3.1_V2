using System;
using System.Collections.Generic;
using System.Text;

namespace MuniBot.ServicioWeb.BackEnd.Entities
{
    public class EntidadBase
    {
        public int id_row_number { get; set; }
        public string fl_inactivo { get; set; }
        public int id_usuario_creacion { get; set; }
        public DateTime? fe_creacion { get; set; }
        public int id_usuario_modificacion { get; set; }
        public DateTime? fe_modificacion { get; set; }
        public string no_token { get; set; }
        public int error_number { get; set; }
        public string error_message { get; set; }


    }
}
