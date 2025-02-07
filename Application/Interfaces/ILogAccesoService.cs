using ControlAcceso.Domain.Entities;

namespace ControlAcceso.Application.Interfaces
{
    public interface ILogAccesoService
    {
        Task CreateAsync ( LogAcceso log );
        Task<List<LogAcceso>> GetLogsAsync ( );
    }
}
