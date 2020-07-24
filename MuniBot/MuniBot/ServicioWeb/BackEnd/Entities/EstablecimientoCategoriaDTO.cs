using System;

namespace MuniBot.ServicioWeb.BackEnd.Entities
{
	public class EstablecimientoCategoriaDTO:EntidadBase
	{
		public string co_establecimiento_clase { get; set; }
		public string co_establecimiento_subclase { get; set; }
		public string co_establecimiento_categoria { get; set; }
		public string no_establecimiento_categoria { get; set; }

	}
}