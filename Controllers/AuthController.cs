using ControlAcceso.Application.Interfaces;
using ControlAcceso.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILoginRequestService _loginRequest;
        //private readonly JwtService _jwtService;

        public AuthController ( IUsuarioService usuarioService , ILoginRequestService loginRequest /*, JwtService jwtService*/)
        {
            _usuarioService = usuarioService;
            _loginRequest = loginRequest;
            //_jwtService = jwtService;
        }

        [HttpPost ( "login" )]
        public async Task<IActionResult> Login ( [FromBody] LoginRequest request )
        {
            var usuario = await _usuarioService.GetByEmailAsync ( request.Email );
            if (usuario == null || !BCrypt.Net.BCrypt.Verify ( request.Password , usuario.Password ))
            {
                return Unauthorized ( "Credenciales incorrectas" );
            }

            var login = new LoginRequest { Email = request.Email , Password = request.EncryptedPassword , Fecha = DateTime.Now };

            await _loginRequest.CreateAsync ( login );


            //var token = _jwtService.GenerateToken ( usuario.Id , usuario.EsAdmin );
            //return Ok ( new { Token = token } );
            return Ok ( "Credenciales correctas" );
        }
    }
}
