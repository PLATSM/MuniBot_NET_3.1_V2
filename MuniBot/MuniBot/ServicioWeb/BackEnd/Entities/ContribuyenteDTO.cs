﻿namespace MuniBot.ServicioWeb.BackEnd.Entities
{
    public class ContribuyenteDTO:EntidadBase
    {
        public int id_contribuyente { get; set; }
        public int id_empresa { get; set; }
        public string co_usuario { get; set; }
        public string co_tipo_persona { get; set; }
        public string co_documento_identidad { get; set; }
        public string nu_documento_identidad { get; set; }
        public string no_nombres { get; set; }
        public string no_apellido_paterno { get; set; }
        public string no_apellido_materno { get; set; }
        public string fe_nacimiento { get; set; }
        public string co_sexo { get; set; }
        public string no_razon_social { get; set; }
        public string no_representante_legal { get; set; }
        public string nu_telefono { get; set; }
        public string no_direccion { get; set; }
        public string no_correo_electronico { get; set; }
        public string no_contrasena { get; set; }
        public string no_contrasena_sha256 { get; set; }
        public string fl_resetear_pwd { get; set; }
        public string fl_bloqueado { get; set; }
        public string fe_bloqueado { get; set; }
        public int qt_login_intentos { get; set; }
        public string fe_cambio_contrasena { get; set; }
        public string foto { get; set; }
        public string no_contribuyente { get; set; }
        public string no_tipo_persona { get; set; }

    }
}
