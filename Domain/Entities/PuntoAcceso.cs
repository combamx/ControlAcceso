using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ControlAcceso.Domain.Entities
{
    public class PuntoAcceso
    {
        [BsonId]
        [BsonRepresentation ( BsonType.ObjectId )]
        public string Id { get; set; } = null!;
        public string Nombre { get; set; } = null!;
    }
}
