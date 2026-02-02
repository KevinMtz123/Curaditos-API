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
    public class RolesController(IRoleService _service) : ControllerBase
    {

        // GET: api/roles
        [HttpGet]
        public async Task<ActionResult<List<RoleDto>>> GetAll()
        {
            try
            {
                var roles = await _service.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/roles/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetById(string id)
        {
            try
            {
                var role = await _service.GetRoleByIdAsync(id);

                if (role == null)
                    return NotFound(new { message = "Rol no encontrado" });

                return Ok(role);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/roles/name/{name}
        [HttpGet("name/{name}")]
        public async Task<ActionResult<RoleDto>> GetByName(string name)
        {
            try
            {
                var role = await _service.GetRoleByNameAsync(name);

                if (role == null)
                    return NotFound(new { message = "Rol no encontrado" });

                return Ok(role);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/roles
        [HttpPost]
        public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _service.CreateRoleAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = resultado.Id },
                    resultado
                );
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // DELETE: api/roles/{name}
        [HttpDelete("{name}")]
        public async Task<ActionResult> Delete(string name)
        {
            try
            {
                var eliminado = await _service.DeleteRoleAsync(name);

                if (!eliminado)
                    return NotFound(new { message = "Rol no encontrado" });

                return Ok(new { message = "Rol eliminado correctamente" });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/roles/exists/{name}
        [HttpGet("exists/{name}")]
        public async Task<ActionResult<bool>> Exists(string name)
        {
            try
            {
                var existe = await _service.RoleExistsAsync(name);
                return Ok(new { exists = existe });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
