using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ControlAcceso.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secret;

        public JwtMiddleware ( RequestDelegate next , IConfiguration configuration )
        {
            _next = next;
            _secret = configuration.GetSection ( "JwtSettings:Secret" ).Value!;
        }

        public async Task Invoke ( HttpContext context )
        {
            var token = context.Request.Headers [ "Authorization" ].FirstOrDefault ( )?.Split ( " " ).Last ( );
            if (token != null)
            {
                AttachUserToContext ( context , token );
            }

            await _next ( context );
        }

        private void AttachUserToContext ( HttpContext context , string token )
        {
            var tokenHandler = new JwtSecurityTokenHandler ( );
            var key = Encoding.UTF8.GetBytes ( _secret );

            try
            {
                tokenHandler.ValidateToken ( token , new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true ,
                    IssuerSigningKey = new SymmetricSecurityKey ( key ) ,
                    ValidateIssuer = false ,
                    ValidateAudience = false ,
                    ClockSkew = TimeSpan.Zero
                } , out SecurityToken validatedToken );

                var jwtToken = (JwtSecurityToken) validatedToken;
                var userId = jwtToken.Claims.First ( x => x.Type == ClaimTypes.NameIdentifier ).Value;

                context.Items [ "User" ] = userId;
            }
            catch
            {
                // Si el token es inválido, no se agrega el usuario al contexto
            }
        }
    }
}
