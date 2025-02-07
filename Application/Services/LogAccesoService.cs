using ControlAcceso.Application.Interfaces;
using ControlAcceso.Domain.Entities;
using ControlAcceso.Infrastructure.Data;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ControlAcceso.Application.Services
{
    public class LogAccesoService : ILogAccesoService
    {
        private readonly IMongoCollection<LogAcceso> _logs;

        public LogAccesoService ( IOptions<MongoDBSettings> mongoSettings )
        {
            var settings = mongoSettings.Value;
            var client = new MongoClient ( settings.ConnectionString );
            var database = client.GetDatabase ( settings.DatabaseName );
            _logs = database.GetCollection<LogAcceso> ( settings.LogsCollection );
        }

        public async Task CreateAsync ( LogAcceso log ) => await _logs.InsertOneAsync ( log );

        public async Task<List<LogAcceso>> GetLogsAsync ( ) => await _logs.Find ( l => true ).ToListAsync ( );
    }
}
