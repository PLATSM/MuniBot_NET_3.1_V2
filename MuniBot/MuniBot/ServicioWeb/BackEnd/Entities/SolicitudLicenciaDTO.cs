using System;

namespace MuniBot.ServicioWeb.BackEnd.Entities
{
    public class SolicitudLicenciaDTO: EntidadBase
    {
		public int id_solicitud_licencia { get; set; }
		public int id_empresa { get; set; }
		public int id_contribuyente { get; set; }
		public string nu_solicitud_licencia { get; set; }
		public string co_tipo_licencia { get; set; }
		public string no_comercial { get; set; }
		public string co_establecimiento_clase { get; set; }
		public string co_establecimiento_subclase { get; set; }
		public string co_establecimiento_categoria { get; set; }
		public string no_direccion_solicitud { get; set; }
		public string co_ubigeo { get; set; }
		public string no_departamento { get; set; }
		public string no_provincia { get; set; }
		public string no_distrito { get; set; }
		public decimal nu_area { get; set; }
		public string no_imagen_croquis { get; set; }
		public string co_estado { get; set; }
		public string nu_resolucion { get; set; }
		public string fe_resolucion { get; set; }
		public int id_autorizador { get; set; }
		public string fe_proceso_ini { get; set; }
		public string fe_proceso_fin { get; set; }
		public string no_tipo_persona { get; set; }
		public string no_establecimiento_clase { get; set; }
		public string no_establecimiento_subclase { get; set; }
		public string no_establecimiento_categoria { get; set; }
		public string no_tipo_licencia { get; set; }
		public string no_estado { get; set; }
		public string no_contribuyente { get; set; }
		public string co_tipo_persona { get; set; }
		public string co_documento_identidad { get; set; }
		public string nu_documento_identidad { get; set; }
		public string no_nombres { get; set; }
		public string no_apellido_paterno { get; set; }
		public string no_apellido_materno { get; set; }
		public string no_razon_social { get; set; }
		public string no_direccion_contribuyente { get; set; }
		public string nu_telefono { get; set; }
		public string no_correo_electronico { get; set; }
		public string no_autorizador { get; set; }
		public string no_area { get; set; }

	}
}
