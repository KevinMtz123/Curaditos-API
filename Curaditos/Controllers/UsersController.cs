using Curaditos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;

namespace Curaditos.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService _service) : ControllerBase
    {
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            try
            {
                var usuarios = await _service.GetUsersAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(string id)
        {
            try
            {
                var usuario = await _service.GetUsuarioById(id);
                if (usuario is null)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Users/email/{email}
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetByEmail(string email)
        {
            try
            {
                var usuario = await _service.GetUsuarioByEmail(email);
                if (usuario is null)
                    return NotFound(new { message = "Usuario no encontrado" });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _service.AddUsuario(dto);
                return CreatedAtAction(nameof(GetById), new { id = resultado.Id }, resultado);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _service.UpdateUsuario(id, dto);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _service.DeleteUsuario(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Users/5/roles
        [HttpGet("{id}/roles")]
        public async Task<ActionResult<List<string>>> GetUserRoles(string id)
        {
            try
            {
                var roles = await _service.GetUserRoles(id);
                return Ok(roles);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/Users/5/roles
        [HttpPost("{id}/roles")]
        public async Task<IActionResult> AddRole(string id, [FromBody] AddRoleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _service.AddToRole(id, dto);
                return Ok(new { message = $"Rol {dto.RoleName} agregado correctamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/Users/5/roles/{roleName}
        [HttpDelete("{id}/roles/{roleName}")]
        public async Task<IActionResult> RemoveRole(string id, string roleName)
        {
            try
            {
                await _service.RemoveFromRole(id, roleName);
                return Ok(new { message = $"Rol {roleName} removido correctamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
