using MuniBot.ServicioWeb.BackEnd.Entities;

namespace MuniBot.ServicioWeb.PIDE.RENIEC
{
    public interface IReniec
    {
        int VerificarDNI(ContribuyenteDTO contribuyenteDTO);
    }
}