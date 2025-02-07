using ControlAcceso.Application.Interfaces;
using ControlAcceso.Domain.Entities;
using ControlAcceso.Infrastructure.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ControlAcceso.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public UsuarioService ( IOptions<MongoDBSettings> mongoSettings )
        {
            var settings = mongoSettings.Value;
            var client = new MongoClient ( settings.ConnectionString );
            var database = client.GetDatabase ( settings.DatabaseName );
            _usuarios = database.GetCollection<Usuario> ( settings.UsuariosCollection );
        }

        public async Task<List<Usuario>> GetAllAsync ( ) => await _usuarios.Find ( u => true ).ToListAsync ( );

        public async Task<Usuario?> GetByEmailAsync ( string email ) =>
            await _usuarios.Find ( u => u.Email == email ).FirstOrDefaultAsync ( );

        public async Task<Usuario?> GetByCredencialAsync ( string credencial ) =>
            await _usuarios.Find ( u => u.Credencial == credencial ).FirstOrDefaultAsync ( );

        public async Task CreateAsync ( Usuario usuario ) => await _usuarios.InsertOneAsync ( usuario );

        public async Task UpdatePermisosAsync ( string id , List<PuntoAcceso> permisos ) =>
            await _usuarios.UpdateOneAsync (
                u => u.Id == id ,
                Builders<Usuario>.Update.Set ( u => u.Permisos , permisos )
            );


    }
}
