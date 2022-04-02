using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using ToyStore.Models;
using ToyStore.SessionExtension;
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

       
            [Authorize]
        //[Route("secret")]
        public IActionResult Index()
        {

            var category = model.Searching(db);
            ViewBag.category = category;
            var toysModel = db.toys
                .Select(x => new Toys { Id = x.Id, toy_name = x.toy_name, price = x.price, description = x.description })
                .ToList();
            IndexViewModel toys = new IndexViewModel { Toys = toysModel };

            return View(toys);
        }

       
        [HttpPost]
        public IActionResult Index(int toyId)
        {
            ViewBag.category = model.Searching(db);

            var toysModel = db.toys
                .Where(x => x.Id == toyId)
                .Select(x => new Toys { Id = x.Id, toy_name = x.toy_name,price=x.price,summa=x.summa, description = x.description})
                .ToList();
           
            IndexViewModel toys = new IndexViewModel { Toys = toysModel };

            
            var cart = HttpContext.Session.GetObjectFromJson<IndexViewModel>("ToyNames");
            if (cart != null)
            {
                var newl = cart.Toys.ToList().Concat(toysModel);

                HttpContext.Session.Remove("ToyNames");
                IndexViewModel toysNew = new IndexViewModel { Toys = newl };
              
                HttpContext.Session.SetObjectAsJson("ToyNames", toysNew);

                return RedirectToAction("Index");
            }

            HttpContext.Session.SetObjectAsJson("ToyNames", toys);
            return RedirectToAction("Index");
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