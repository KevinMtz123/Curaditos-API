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
    public class PromotionProductsController(IPromotionProductService service) : ControllerBase
    {
        private readonly IPromotionProductService _service = service;


        // GET: api/Vacaciones
        [HttpGet]
        public async Task<ActionResult<List<PromotionProductDto>>> GetAll()
        {
            try
            {
                var uniformes = await _service.GetPromotionProduct();
                return Ok(uniformes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Vacaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionProductDto>> GetById(int id)
        {
            try
            {
                var uniforme = await _service.GetPromotionProductById(id);
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
        public async Task<ActionResult<PromotionProductDto>> Create([FromBody] PromotionProductCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _service.AddPromotionProduct(dto);
                return CreatedAtAction(nameof(GetById), new { id = resultado.Id }, resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/Vacaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PromotionProductCreateDto dto)
        {
            try
            {
                await _service.UpdatePromotionProduct(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
