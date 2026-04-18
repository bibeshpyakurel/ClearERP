using Microsoft.AspNetCore.Mvc;

namespace ClearErp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "System")]
[Produces("application/json")]
public sealed class HealthController : ControllerBase
{
    /// <summary>
    /// Returns a lightweight health payload that can be used to confirm the API is running.
    /// </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "Healthy",
            service = "ClearERP API",
            utcTimestamp = DateTime.UtcNow
        });
    }
}
