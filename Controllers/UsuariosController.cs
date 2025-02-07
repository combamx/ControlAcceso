using ControlAcceso.Application.Interfaces;
using ControlAcceso.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Controllers
{
    //[Authorize] // Este controlador requiere autenticación
    [Route ( "api/[controller]" )]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController ( IUsuarioService usuarioService )
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> Create ( Usuario request )
        {
            var usuario = new Usuario
            {
                Email = request.Email ,
                Password = request.EncryptedPassword , // Usamos el password encriptado
                Credencial = request.Credencial ,
                EsAdmin = request.EsAdmin ,
                Permisos = request.Permisos ,
                Id = request.Id ,
                Nombre = request.Nombre
            };

            await _usuarioService.CreateAsync ( usuario );
            return CreatedAtAction ( nameof ( Create ) , new { id = usuario.Id } , usuario );
        }

        [HttpPut ( "{id}/permisos" )]
        public async Task<IActionResult> UpdatePermisos ( string id , [FromBody] List<PuntoAcceso> permisos )
        {
            await _usuarioService.UpdatePermisosAsync ( id , permisos );
            return NoContent ( );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll ( ) => Ok ( await _usuarioService.GetAllAsync ( ) );
    }
}
