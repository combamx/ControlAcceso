using ControlAcceso.Application.Interfaces;
using ControlAcceso.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ControlAcceso.Controllers
{
    [Route ( "api/[controller]" )]
    [ApiController]
    public class AccesosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogAccesoService _logAccesoService;

        public AccesosController ( IUsuarioService usuarioService , ILogAccesoService logAccesoService )
        {
            _usuarioService = usuarioService;
            _logAccesoService = logAccesoService;
        }

        [HttpPost ( "validar" )]
        public async Task<IActionResult> ValidarAcceso ( string credencial , PuntoAcceso puntoAcceso )
        {
            var usuario = await _usuarioService.GetByCredencialAsync ( credencial );
            if (usuario == null)
            {
                await _logAccesoService.CreateAsync ( new LogAcceso
                {
                    UsuarioId = "Desconocido" ,
                    PuntoAcceso = puntoAcceso ,
                    AccesoPermitido = false
                } );
                return Unauthorized ( );
            }

            var accesoPermitido = usuario.Permisos.Contains ( puntoAcceso );
            await _logAccesoService.CreateAsync ( new LogAcceso
            {
                UsuarioId = usuario.Id ,
                PuntoAcceso = puntoAcceso ,
                AccesoPermitido = accesoPermitido
            } );

            return accesoPermitido ? Ok ( )
            : new ObjectResult ( new { Mensaje = "Acceso denegado" } )
            {
                StatusCode = StatusCodes.Status403Forbidden
            };

        }
    }
}
