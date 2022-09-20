using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Authentication.Controllers;

[Route("api")]
[ApiController]
public class ApiController : ControllerBase
{
    [Route("role")]
    public IActionResult Role()
    {
        string? role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        return new JsonResult(role);
    }

    [Route("public")]
    public IActionResult PublicApi()
    {
        return new JsonResult("OK");
    }

    [Authorize]
    [Route("authenticated")]
    public IActionResult AuthenticatedApi()
    {
        return new JsonResult("OK");
    }

    [Authorize(Roles = "Administrator")]
    [Route("admin")]
    public IActionResult AdminApi()
    {
        return new JsonResult("OK");
    }
}
