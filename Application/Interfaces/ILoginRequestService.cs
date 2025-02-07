using ControlAcceso.Domain.Entities;

namespace ControlAcceso.Application.Interfaces
{
    public interface ILoginRequestService
    {
        Task CreateAsync ( LoginRequest log );
        Task<List<LoginRequest>> GetLogsAsync ( );
    }
}
