using MuniBot.ServicioWeb.BackEnd.Entities;

namespace MuniBot.ServicioWeb.PIDE.SUNAT
{
    public interface ISunat
    {
        int VerificarRUC(ContribuyenteDTO contribuyenteDTO);
    }
}