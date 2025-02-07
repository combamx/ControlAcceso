using MongoDB.Bson.Serialization.Attributes;

namespace ControlAcceso.Domain.Entities
{
    public class LoginRequest
    {
        [BsonElement ( "email" )]
        public string Email { get; set; } = null!;

        [BsonElement ( "password" )]
        public string Password { get; set; } = null!;

        [BsonElement ( "fecha" )]
        [BsonRepresentation ( MongoDB.Bson.BsonType.DateTime )]
        [BsonIgnoreIfDefault]
        public DateTime Fecha { get; set; }

        // Propiedad para devolver el password encriptado
        [BsonIgnore]
        public string EncryptedPassword => BCrypt.Net.BCrypt.HashPassword ( Password );

        // Método para obtener el password encriptado
        public string GetEncryptedPassword ( ) => BCrypt.Net.BCrypt.HashPassword ( Password );
    }
}
