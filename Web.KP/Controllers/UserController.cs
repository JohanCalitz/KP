namespace Web.KP.Controllers
{
    using Web.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Web.KP.Models;
    using Web.Services.Helpers;

    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IGroupService groupService;
        public UserController(IUserService userService, IGroupService groupService)
        {
            this.userService = userService;
            this.groupService = groupService;
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            Result<Web.Models.PagedResult<Web.Models.UserModel>> result = await this.userService.GetPagedUsersAsync(page, pageSize);

            if (result.State == ResultStateEnum.Faulted || result.Value == null)
            {
                ViewBag.Error = result.resulterror?.Message;
                return View();
            }

            Web.Models.PagedResult<Web.Models.UserModel> pagedResult = result.Value;

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = pagedResult.TotalPages;

            GridViewModel<UserViewModel> model = new()
            {
                Items = pagedResult.Items.Select(x=>
                    new UserViewModel()
                    {
                        CreatedAt = x.CreatedAt,
                        Email = x.Email,
                        FullName = x.FullName,
                        UserName = x.UserName,
                        UserId = x.Id,
                    }).ToList(),
                PropertiesToDisplay = new[] { "UserName", "Email", "FullName", "CreatedAt" },
                EditAction = "Edit",
                EditController = "User",
                DeleteAction = "Delete",
                DeleteController = "User",
                KeyProperty = "UserId",
            };

            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Web.Models.UserModel userModel = new()
            {
                Id = model.UserId,
                Password = model.Password,
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            Result<bool> result = await userService.CreateUserAsync(userModel);

            if (result.State == ResultStateEnum.Faulted || result.Value == false)
            {
                ViewBag.Error = result.resulterror?.Message;
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            Result<bool> result = await userService.DeleteUserAsync(id);
            if (result.State == ResultStateEnum.Faulted || result.Value == null)
            {
                ViewBag.Error = result.resulterror?.Message;
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Result<Web.Models.UserModel> result = await this.userService.GetUserByIdAsync(id);
           
            if (result.State == ResultStateEnum.Faulted || result.Value == null)
            {
                ViewBag.Error = result.resulterror?.Message;
                return View();
            }
            var hasNotGroups = await groupService.GetHasNotGroupsAsync(id);
            if (hasNotGroups.State == ResultStateEnum.Faulted || hasNotGroups.Value == null)
            {
                ViewBag.Error = hasNotGroups.resulterror?.Message;
                return View();
            }
            var hasGroups = await groupService.GetHasGroupsAsync(id);
            if (hasGroups.State == ResultStateEnum.Faulted || hasNotGroups.Value == null)
            {
                ViewBag.Error = hasNotGroups.resulterror?.Message;
                return View();
            }
            Web.Models.UserModel user = result.Value;

            UserViewModel model = new()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                Password = user.Password,
                HasGroup = hasGroups.Value.Select(x =>
                    new GroupViewModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList(),
                HasNotGroup = hasNotGroups.Value.Select(x =>
                  new GroupViewModel()
                  {
                      Id = x.Id,
                      Name = x.Name,
                  }).ToList(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Web.Models.UserModel userModel = new()
            {
                Id = model.UserId,
                Password = model.Password,
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                CreatedAt = model.CreatedAt
            };

            Result<bool> result = await this.userService.UpdateUserAsync(model.UserId, userModel);

            if (result.State == ResultStateEnum.Faulted || result.Value == false)
            {
                ViewBag.Error = result.resulterror?.Message;
                return View();
            }

            return RedirectToAction("Index");
        }
    }
}
