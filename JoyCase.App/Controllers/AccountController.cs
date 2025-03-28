using JoyCase.App.Models.UserModel;
using JoyCase.App.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApiService _apiService;
    public AccountController(IHttpContextAccessor httpContextAccessor, ApiService apiService)
    {
        _httpContextAccessor = httpContextAccessor;
        _apiService = apiService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LoginUser(string email, string password)
    {
        // kullanici dogrulama
        var result = await _apiService.LoginAsync(new LoginUserModel() { Username = email, Password = password });

        HttpContext.Response.Cookies.Append("RememberUsername", email, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMonths(1), // 1 ay boyunca sakla
            HttpOnly = false, // tarayicidan erisim
            Secure = true, // HTTPS
            IsEssential = true // yasal veri korumasi icin
        });

        HttpContext.Response.Cookies.Append("Token", result.token, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddMonths(1), // 1 ay boyunca sakla
            HttpOnly = false, // tarayicidan erisim
            Secure = true, // HTTPS
            IsEssential = true // yasal veri korumasi icin
        });

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Account");
    }
}
