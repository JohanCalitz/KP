namespace Web.KP.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;
    using Web.KP.Models;
    using Web.Services.Helpers;
    using Web.Services.Interfaces;

    public class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await this.userService.GetDashboardAsync();

            if (result.State == ResultStateEnum.Faulted || result.Value == null)
            {
                ViewBag.Error = result.resulterror?.Message;
                return View();
            }

            var data = result.Value;

            DashboardViewModel model = new()
            {
                Groups = data.Groups,
                Permissions = data.Permissions,
                Users = data.Users
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
