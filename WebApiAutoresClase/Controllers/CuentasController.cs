using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebApiAutoresClase.DTOs;

namespace WebApiAutoresClase.Controllers
{
    [ApiController]
    [Route("api/Cuentas")]
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        public CuentasController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager; 
        }

        //EndPoint para registrar usuarios
        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacion>> Regsitrar(CredencialesUsuario credencialesUsuario)
        {
            var usuario = new IdentityUser { UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };
            var resultado = await _userManager.CreateAsync(usuario, credencialesUsuario.Password);

            if (resultado.Succeeded)
            {
                return await ContruirToken(credencialesUsuario);
            }
            else
            {
                return BadRequest(resultado.Errors);    
            }
        }

        private async Task<RespuestaAutenticacion> ContruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim("email", credencialesUsuario.Email),
            };

            //Agregar claim
            //claims.AddRange()

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration))
        }
    }
}
