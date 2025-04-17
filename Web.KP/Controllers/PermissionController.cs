namespace Web.KP.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Web.KP.Models;
    using Web.Services.Helpers;
    using Web.Services.Interfaces;

    public class PermissionController : Controller
    {
        private readonly IPermissionService permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            this.permissionService = permissionService;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            Result<Web.Models.PagedResult<Web.Models.PermissionModel>> result = await this.permissionService.GetPagedPermissionsAsync(page, pageSize);

            if (result.State == ResultStateEnum.Faulted || result.Value == null)
            {
                ViewBag.Error = result.resulterror?.Message;
                return View();
            }

            Web.Models.PagedResult<Web.Models.PermissionModel> pagedResult = result.Value;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = pagedResult.TotalPages;

            GridViewModel<PermissionViewModel> model = new()
            {
                Items = pagedResult.Items.Select(x =>
                    new PermissionViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
               
                    }).ToList(),
                PropertiesToDisplay = new[] { "Name" },
            };

            return View(model);
        }
    }
}
