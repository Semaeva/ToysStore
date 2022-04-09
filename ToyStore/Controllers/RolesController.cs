using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToyStore.Models;
using ToyStore.ViewsModel;

namespace ToyStore.Controllers
{


    [Authorize(Roles = "admin")]
    public class RolesController : Controller
    {

        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;
        SignInManager<User> _signInManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: RolesController
        public ActionResult Index()
        {
            return View(_roleManager.Roles.ToList()); 
    }

    // GET: RolesController/Details/5
    public ActionResult Details(int id)
        {
            return View();
        }


        public IActionResult Create() {
            return View();
        }
        // GET: RolesController/Create
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }



        // GET: RolesController/Edit/5
        public async Task<ActionResult> Edit(string userId)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var model = new ChangeRoleModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                await _signInManager.RefreshSignInAsync(user);//смена роли , не выходя из аккаунта
                return View(model);
            }

            return NotFound();
        }

        // POST: RolesController/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);//роли пользователя
                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);// добавленные роли
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    

    public IActionResult UserList() => View(_userManager.Users.ToList());

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

    
    }
}
