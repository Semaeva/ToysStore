using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToyStore.Models;
using ToyStore.ViewsModel;

namespace ToyStore.Controllers
{
    public class ProfileController : Controller
    {

        private ApplicationContext db;
        private IndexViewModel model = new IndexViewModel();
        private readonly UserManager<User> _userManager;

        public ProfileController( ApplicationContext context,UserManager<User> userManager)
        {
            db = context;
            _userManager = userManager;

        }
        // GET: Profile
        public ActionResult Index()
        {
            var category = model.Searching(db);
            ViewBag.category = category;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier );

            var list_profile = db.Users.Where(a=>a.Id==userId)
                .Include(x => x.toysusers).ThenInclude(d => d.toys).ToList();

            
            return View(list_profile);
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
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.category = model.Searching(db);

            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id.ToString());
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            var result = db.Users.SingleOrDefault(b => b.Id == user.Id);
            if (result != null)
            {
                result.Street =user.Street;
                result.City =user.City;
                result.Area =user.Area;
                result.UserName =user.UserName;
                result.House =user.House;
                result.Phone =user.Phone;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
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
