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
    public class PromotionsController(IPromotionService service) : ControllerBase
    {
        private readonly IPromotionService _service = service;


        // GET: api/Vacaciones
        [HttpGet]
        public async Task<ActionResult<List<PromotionDto>>> GetAll()
        {
            try
            {
                var uniformes = await _service.GetPromotion();
                return Ok(uniformes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // GET: api/Vacaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PromotionDto>> GetById(int id)
        {
            try
            {
                var uniforme = await _service.GetPromotionById(id);
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
        public async Task<ActionResult<PromotionDto>> Create([FromBody] PromotionCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var resultado = await _service.AddPromotion(dto);
                return CreatedAtAction(nameof(GetById), new { id = resultado.Id }, resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // PUT: api/Vacaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PromotionCreateDto dto)
        {
            try
            {
                await _service.UpdatePromotion(id, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
