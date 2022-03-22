using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using ToyStore.Models;
using ToyStore.ViewsModel;

namespace ToyStore.Controllers
{
    public class HomeController : Controller
    {

        ApplicationContext db;
        private IndexViewModel model = new IndexViewModel();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
           
            _logger = logger;
            db = context;
        }

   
        public IActionResult Index(int? categoryId)
        {
            ViewBag.category = model.Searching(db);
            var toysModel = db.toys
                .Select(x => new Toys {Id =x.Id,toy_name = x.toy_name, description= x.description })
                .ToList();
                IndexViewModel toys = new IndexViewModel { Toys = toysModel };

                return View(toys);
        }

        [HttpPost]
        public IActionResult Index(string toyName, string toyId)
        {
            ViewBag.category = model.Searching(db);

            var toysModel = db.toys
                .Where(x => x.toy_name == toyName)
                .Select(x => new Toys { Id = x.Id, toy_name = x.toy_name, description = x.description })
                .ToList();
           
            IndexViewModel toys = new IndexViewModel { Toys = toysModel };

            return View(toys);
        }


        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            var customers = (from toy in db.toys
                             where toy.toy_name.StartsWith(prefix)
                             select new
                             {
                                 label = toy.toy_name,
                                 val = toy.Id
                             }).ToList();

            return Json(customers);
        }
        public async Task<IActionResult> Orders(int idUser, int idToys) {
            ViewBag.category = model.Searching(db);

            var user = db.users.Include(x => x.toy)
                .Where(u=>u.id == idUser)
                .ToList();
            
                foreach (var u in user)
                {
                var list = u.toy.Where(x => x.Id == idToys).ToList();
                     return View(list);
                }
            return View();
        }

        public async Task<IActionResult> Catalog()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult GatToysFromCategories()
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