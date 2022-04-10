using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToyStore.Models;

namespace ToyStore.Controllers
{

    [Authorize(Roles = "admin")]
    public class ToysController : Controller
    {

        private ApplicationContext db;
        public ToysController(ApplicationContext context)
        {
            db = context;

        }
        // GET: ToysController
        public ActionResult Index()
        {
            return View(db.toys.Include(c=>c.category).ToList());
        }

        // GET: ToysController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ToysController/Create
        public ActionResult Create()
        {
            var categories= db.categories.ToList().Select(x => new SelectListItem
            {
                Text = x.category_name,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.listCategories = categories;
            return View();
        }

        // POST: ToysController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Toys toys, string categories)
        {
            if (!string.IsNullOrEmpty(toys.toy_name))
            {
              await db.toys.AddAsync(new Toys { categoryID= toys.categoryID, toy_name = toys.toy_name, description=toys.description, price= toys.price});
             var res=   db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // GET: ToysController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var categories = db.categories.ToList().Select(x => new SelectListItem
            {
                Text = x.category_name,
                Value = x.Id.ToString()
            }).ToList();
            ViewBag.listCategories = categories;

            if (id != null)
            {
                var toys = await db.toys.FirstOrDefaultAsync(p => p.Id == id);
                if (toys != null)
                    return View(toys);
            }
            return View();
        }

        // POST: ToysController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Toys model)
        {

            var result = db.toys.SingleOrDefault(b => b.Id == model.Id);
            if (result != null)
            {
                result.toy_name = model.toy_name;
                result.description = model.description;
                result.price = model.price;
                result.categoryID = model.categoryID;
                
                db.SaveChanges();
            }
            return RedirectToAction("Index");   
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Toys toy = await db.toys.FirstOrDefaultAsync(p => p.Id == id);
                if (toy != null)
                    db.toys.Remove(toy);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
