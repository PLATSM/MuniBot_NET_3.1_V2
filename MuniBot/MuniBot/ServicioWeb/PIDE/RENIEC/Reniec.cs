using MuniBot.ServicioWeb.BackEnd.Entities;

namespace MuniBot.ServicioWeb.PIDE.RENIEC
{
    public class Reniec:IReniec
    {
        public int VerificarDNI(ContribuyenteDTO contribuyenteDTO)
        {
            ReniecClient reniecClient = new ReniecClient();
            var result = reniecClient.GetReniecAsync(contribuyenteDTO.nu_documento_identidad);

            int error_number = 0;

            switch (result.coResultado)
            {
                case "0000":

                    if (result.reniecDatos.prenombres != null)
                    {
                        if (contribuyenteDTO.no_nombres.Trim().ToUpper() != result.reniecDatos.prenombres.Trim().ToUpper())
                            error_number = -1;
                    }

                    if (result.reniecDatos.apPrimer != null)
                    {
                        if (contribuyenteDTO.no_apellido_paterno.Trim().ToUpper() != result.reniecDatos.apPrimer.Trim().ToUpper())
                            error_number = -1;
                    }

                    if (result.reniecDatos.apSegundo != null)
                    {
                        if (contribuyenteDTO.no_apellido_materno.Trim().ToUpper() != result.reniecDatos.apSegundo.Trim().ToUpper())
                            error_number = -1;
                    }

                    if (error_number == 0)
                    {
                        if (result.reniecDatos.foto != null)
                            contribuyenteDTO.foto = result.reniecDatos.foto;
                    }
                    break;

                case "0001":
                    error_number = 1;
                    break;

                case "0999":
                    error_number = 999;
                    break;

                case "1000":
                    error_number = 1000;
                    break;

                case "1001":
                    error_number = 1001;
                    break;

                case "1002":
                    error_number = 1002;
                    break;

                case "1003":
                    error_number = 1003;
                    break;

                case "1999":
                    error_number = 1999;
                    break;
            };

            return error_number;
        }
    }
}
