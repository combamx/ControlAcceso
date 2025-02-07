using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ControlAcceso.Domain.Entities
{
    public class Usuario : LoginRequest
    {
        [BsonId]
        [BsonRepresentation ( BsonType.ObjectId )]
        [BsonIgnoreIfDefault]
        public string Id { get; set; } = null!;

        [BsonElement ( "nombre" )]
        public string Nombre { get; set; } = null!;

        [BsonElement ( "credencial" )]
        public string Credencial { get; set; } = null!; // RFID o QR

        [BsonElement ( "esAdmin" )]
        public bool EsAdmin { get; set; }

        [BsonElement ( "permisos" )]
        public List<PuntoAcceso> Permisos { get; set; } = new ( ); // IDs de áreas
    }
}
