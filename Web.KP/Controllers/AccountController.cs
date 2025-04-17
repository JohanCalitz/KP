namespace Web.KP.Controllers
{
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Mvc;
    using System.Security.Claims;
    using Web.KP.Models;
    using Web.Services.Interfaces;
    using Microsoft.AspNetCore.Http;

    public class AccountController : Controller
    {
        private readonly IAccountSerivce accountSerivce;

        public AccountController(IAccountSerivce accountSerivce)
        {
            this.accountSerivce = accountSerivce;
        }

        public async Task<IActionResult> Login()
        {
            ISession? session = HttpContext?.Session;

            if (session != null && session.TryGetValue("AccessToken", out byte[]? tokenBytes))
            {
                return View();
            }
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Web.Models.LoginModel login = new()
            {
                UserName = model.Username,
                Password = model.Password
            };

            Services.Helpers.Result<Web.Models.TokenModel> token = await accountSerivce.GetToken(login);
            if (token.State == Services.Helpers.ResultStateEnum.Faulted)
            {
                ModelState.AddModelError("", token.resulterror.Message);
                return View(model);
            }

            if (!string.IsNullOrEmpty(token.Value.Token.ToString()))
            {

                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.Name, model.Username),
                };

                ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new(identity);

                HttpContext.Session.SetString("AccessToken", token.Value.Token.ToString());

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }
    }
}
