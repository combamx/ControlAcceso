namespace ControlAcceso.Infrastructure.Data
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string UsuariosCollection { get; set; } = null!;
        public string LogsCollection { get; set; } = null!;
        public string LoginRequest { get; set; } = null!;

    }
}
