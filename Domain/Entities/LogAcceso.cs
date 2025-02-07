using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ControlAcceso.Domain.Entities
{
    public class LogAcceso
    {
        [BsonId]
        [BsonRepresentation ( BsonType.ObjectId )]
        public string Id { get; set; } = null!;

        public string UsuarioId { get; set; } = null!;
        public PuntoAcceso PuntoAcceso { get; set; } = null!;
        public DateTime FechaHora { get; set; } = DateTime.UtcNow;
        public bool AccesoPermitido { get; set; }
    }
}
