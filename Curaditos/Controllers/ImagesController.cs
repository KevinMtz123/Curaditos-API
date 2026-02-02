using Curaditos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Curaditos.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/images")]
    public class ImagesController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;

        public ImagesController(ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var url = await _cloudinaryService.UploadImageAsync(
                file, "aguardientes/productos");

            return Ok(new { url });
        }
    }

}
