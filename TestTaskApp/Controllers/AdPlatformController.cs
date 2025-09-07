using Microsoft.AspNetCore.Mvc;
using TestTaskApp.Services;

namespace TestTaskApp.Controllers
{
    [ApiController]
    [Route("api")]
    public class AdPlatformController : ControllerBase
    {
        private readonly IAdPlatformsService _adPlatformService;

        public AdPlatformController(IAdPlatformsService adPlatformService)
        {
            _adPlatformService = adPlatformService;
        }
        [HttpPost("upload")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> UploadFile([FromForm]IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Файла нет или он пустой");

            if (!Path.GetExtension(file.FileName).Contains(".txt", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Неверное расширение файла");

            try
            {
                await _adPlatformService.LoadNewAdPlatforms(file);
                return Ok("Файл обработан успешно");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Внутренняя серверная ошибка при обработке файла: " + ex.ToString());
            }
        }
        [HttpGet("search")]
        public IActionResult GetPlatformsByLocation([FromQuery] string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return BadRequest("Необходимо указать параметр локации");
            try
            {
                var platforms = _adPlatformService.GetAdPlatformsByLocation(location);
                if (platforms == null || !platforms.Any())
                    return NotFound("Для указанной локации не надены рекламные площадки");
                return Ok(platforms);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Произошла внутренняя ошибка сервера: " + ex.ToString());
            }
        }
    }
}
