using Curaditos.Helpers;
using Curaditos.Infraestructure.Data.Data;
using Curaditos.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;
using System.Security.Claims;
using System.Text;

namespace Curaditos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtConfig _jwtConfig;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<JwtConfig> jwtConfig)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfig = jwtConfig.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Buscar usuario por email
            var user = await _userManager.FindByEmailAsync(dto.Correo);
            if (user == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            // Verificar contraseña con Identity
            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Contrasena, lockoutOnFailure: false);

            if (!result.Succeeded)
                return Unauthorized(new { message = "Credenciales inválidas" });

            // Obtener roles del usuario
            var roles = await _userManager.GetRolesAsync(user);

            // Generar token
            var token = GenerateToken(user, roles.ToList());

            // Preparar respuesta
            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles.ToList()
            };

            return Ok(new
            {
                Token = token,
                User = userDto
            });
        }

        private string GenerateToken(ApplicationUser user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Agregar roles como claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}