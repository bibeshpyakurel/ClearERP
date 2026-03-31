using Microsoft.AspNetCore.Mvc;

namespace MiniErp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "Healthy",
            service = "Mini ERP API",
            utcTimestamp = DateTime.UtcNow
        });
    }
}
