using PruebaTecnica.Test.Entidades;
using System.Data;

namespace PruebaTecnica.Test.DAL.Interfaces
{
    public interface ITurnosRepository
    {
        List<Turno> GenerarTurnos(DateTime fechaInicio, DateTime fechaFin, int idServicio);
    }
}
