using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAutoresClase.DTOs;

namespace WebApiAutoresClase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        public CuentasController(UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager; 
            _configuration = configuration;
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

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["LlaveJWT"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims,
                expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion,
            };
        }
    }
}
