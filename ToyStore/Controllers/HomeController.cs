using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using ToyStore.Models;
using ToyStore.ViewsModel;

namespace ToyStore.Controllers
{
    public class HomeController : Controller
    {

        ApplicationContext db;

       
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index(int? categoryId)
        {
           // ViewBag.Categories = db.categories.ToList();

            try
            {

                List<Category> categoriesModels = db.categories
              .Select(c => new Category { Id = c.Id, toy_name = c.toy_name })
              .ToList();
                categoriesModels.Insert(0, new Category { Id = 0, toy_name = "Каталог" });
                IndexViewModel category = new IndexViewModel { categories = categoriesModels };

                return View(category);
            }
            catch (Exception ex) { }
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