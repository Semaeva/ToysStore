using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToyStore.Models;
using ToyStore.ViewsModel;

namespace ToyStore.Controllers
{
    public class Profile : Controller
    {

        private ApplicationContext db;
        private IndexViewModel model = new IndexViewModel();
        private readonly UserManager<User> _userManager;

        public Profile( ApplicationContext context,UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;

        }
        // GET: Profile
        public ActionResult Index()
        {
            var category = model.Searching(db);
            ViewBag.category = category;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier );// will give the user's userId

           var list_profile = db.Users.Where(x => x.Id ==userId)
                .Include(s=>s.toy)
                .ToList();
          return View(list_profile);
         //  return View();
        }

        // GET: Profile/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Profile/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User model)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(model);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Profile/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Profile/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Profile/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
