using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AccountController(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        // kullanici dogrulama
        if (email == "test@example.com" && password == "password")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim("Permission", "product_view"),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = true });

            HttpContext.Response.Cookies.Append("RememberUsername", email, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMonths(1), // 1 ay boyunca sakla
                HttpOnly = false, // tarayicidan erisim
                Secure = true, // HTTPS
                IsEssential = true // yasal veri korumasi icin
            });

            // Cookie kontrol
            string cookieTest = HttpContext.Request.Cookies["RememberUsername"];
            var user = _httpContextAccessor.HttpContext.User;

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Invalid credentials";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
