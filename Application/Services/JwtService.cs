using ControlAcceso.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ControlAcceso.Application.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService ( IConfiguration configuration )
        {
            _jwtSettings = configuration.GetSection ( "JwtSettings" ).Get<JwtSettings> ( );
        }

        public string GenerateToken ( string userId , bool isAdmin )
        {
            var claims = new []
            {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, isAdmin ? "Admin" : "User")
        };

            var key = new SymmetricSecurityKey ( Encoding.UTF8.GetBytes ( _jwtSettings.Secret ) );
            var creds = new SigningCredentials ( key , SecurityAlgorithms.HmacSha256 );

            var token = new JwtSecurityToken (
                issuer: _jwtSettings.Issuer ,
                audience: _jwtSettings.Audience ,
                claims: claims ,
                expires: DateTime.UtcNow.AddMinutes ( _jwtSettings.ExpirationMinutes ) ,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler ( ).WriteToken ( token );
        }
    }
}
