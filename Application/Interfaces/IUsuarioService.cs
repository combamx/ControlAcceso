using ControlAcceso.Domain.Entities;

namespace ControlAcceso.Application.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> GetAllAsync ( );
        Task<Usuario?> GetByEmailAsync ( string email );
        Task<Usuario?> GetByCredencialAsync ( string credencial );
        Task CreateAsync ( Usuario usuario );
        Task UpdatePermisosAsync ( string id , List<PuntoAcceso> permisos );
    }
}
