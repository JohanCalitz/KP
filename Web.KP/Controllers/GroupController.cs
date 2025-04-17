namespace Web.KP.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Web.KP.Models;
    using Web.Services.Helpers;
    using Web.Services.Interfaces;

    public class GroupController : Controller
    {
        private readonly IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(Guid groupId, Guid userId)
        {
            var result = await this.groupService.AddUserToGroupAsync(groupId, userId);

            if (result.State == ResultStateEnum.Faulted || result.Value == null)
            {
                ViewBag.Error = result.resulterror?.Message;
                return RedirectToAction("Edit", "User", new { id = userId });
            }
            return RedirectToAction("Edit", "User", new { id = userId });
        }
        [HttpPost]
        public async Task<IActionResult> Removeuser(Guid groupId, Guid userId)
        {
            var result = await this.groupService.RemoveUserFromGroupAsync(groupId, userId);

            if (result.State == ResultStateEnum.Faulted || result.Value == null)
            {
                ViewBag.Error = result.resulterror?.Message;
                return RedirectToAction("Edit", "User", new { id = userId });
            }
            return RedirectToAction("Edit", "User", new { id = userId });
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            Result<Web.Models.PagedResult<Web.Models.GroupModel>> result = await this.groupService.GetPagedGroupsAsync(page, pageSize);

            if (result.State == ResultStateEnum.Faulted || result.Value == null)
            {
                ViewBag.Error = result.resulterror?.Message;
                return View();
            }

            Web.Models.PagedResult<Web.Models.GroupModel> pagedResult = result.Value;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = pagedResult.TotalPages;

            GridViewModel<GroupViewModel> model = new()
            {
                Items = pagedResult.Items.Select(x =>
                    new GroupViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Permissions = x.GroupPermissions.Select(x=>x.Name).ToList()
                    }).ToList(),
                PropertiesToDisplay = new[] { "Name", "Description", "Permissions" },
            };

            return View(model);
        }
    }
}
