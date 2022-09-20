using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers;
public class LogoutController : Controller
{
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        await HttpContext.SignOutAsync();

        return Redirect("/");
    }
}
