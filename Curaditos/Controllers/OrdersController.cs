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
    public class OrdersController(IOrderService service) : ControllerBase
    {
        private readonly IOrderService _service = service;


        // GET: api/Vacaciones
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetAll()
        {
            try
            {
                var uniformes = await _service.GetOrder();
                return Ok(uniformes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Vacaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            try
            {
                var uniforme = await _service.GetOrderById(id);
                if (uniforme is null)
                    return NotFound();

                return Ok(uniforme);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // POST: api/Vacaciones
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] OrderCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _service.AddOrder(dto);
                return CreatedAtAction(nameof(GetById), new { id = resultado.Id }, resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/Vacaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OrderCreateDto dto)
        {
            try
            {
                await _service.UpdateOrder(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
