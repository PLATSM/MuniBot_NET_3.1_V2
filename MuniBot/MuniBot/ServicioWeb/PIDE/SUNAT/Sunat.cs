using MuniBot.ServicioWeb.BackEnd.Entities;

namespace MuniBot.ServicioWeb.PIDE.SUNAT
{
    public class Sunat:ISunat
    {
        public int VerificarRUC(ContribuyenteDTO contribuyenteDTO)
        {
            SunatCliente sunatClient = new SunatCliente();
            var result = sunatClient.GetSunatAsync(contribuyenteDTO.nu_documento_identidad);

            int error_number = 0;

            switch (result.coResultado)
            {
                case "0000":
                    if (result.sunatDatos.ddp_numruc is null)
                    {
                        error_number = -1;
                    }
                    else
                    {
                        if (contribuyenteDTO.no_razon_social.Trim().ToUpper() != result.sunatDatos.ddp_nombre.Trim().ToUpper())
                            error_number = -2;
                    }
                    break;

                case "9999":
                    error_number = 9999;
                    break;
            };
            return error_number;
        }

    }


}
