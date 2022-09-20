using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Authentication.Pages;

public class LoginModel : PageModel
{
    enum Role
    {
        User,
        Administrator,
    }

    static Dictionary<(string Username, string Password), Role> _users = new()
    {
        [("user", "test")] = Role.User,
        [("admin", "admin")] = Role.Administrator,
    };

    public void OnGet() { }

    public async Task<IActionResult> OnPost(string username, string password, string? returnUrl = null)
    {
        bool isValidUser = _users.TryGetValue((username, password), out Role role);
        if (!isValidUser)
            return Redirect($"{Request.Path}?Error=Credenziali+non+riconosciute.");

        List<Claim> claims = new() {
            new Claim(ClaimTypes.NameIdentifier, $"hardcoded:{username}"),
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role.ToString()),
        };

        ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        ClaimsPrincipal principal = new(identity);

        await HttpContext.SignInAsync(principal);

        // URL non locale? potrebbe essere un errore di sviluppo oppure un tentativo di hacking
        if (string.IsNullOrWhiteSpace(returnUrl) || !Url.IsLocalUrl(returnUrl)) {
            returnUrl = "/";
        }

        return Redirect(returnUrl);
    }
}
