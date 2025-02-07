using ControlAcceso.Application.Interfaces;
using ControlAcceso.Domain.Entities;
using ControlAcceso.Infrastructure.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ControlAcceso.Application.Services
{
    public class LoginRequestService : ILoginRequestService
    {
        private readonly IMongoCollection<LoginRequest> _logs;

        public LoginRequestService ( IOptions<MongoDBSettings> mongoSettings )
        {
            var settings = mongoSettings.Value;
            var client = new MongoClient ( settings.ConnectionString );
            var database = client.GetDatabase ( settings.DatabaseName );
            _logs = database.GetCollection<LoginRequest> ( settings.LoginRequest );
        }

        public async Task CreateAsync ( LoginRequest log ) => await _logs.InsertOneAsync ( log );

        public async Task<List<LoginRequest>> GetLogsAsync ( ) => await _logs.Find ( l => true ).ToListAsync ( );
    }
}
