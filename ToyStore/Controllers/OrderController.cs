using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToyStore.Models;
using ToyStore.SessionExtension;
using ToyStore.ViewsModel;

namespace ToyStore.Controllers
{
    public class OrderController : Controller
    {
        ApplicationContext db;
        private IndexViewModel model = new IndexViewModel();

        public OrderController(ApplicationContext context) {
            db = context;
        }
        // GET: OrderController
        public ActionResult Index(int idUser, int idToys)
        {
            ViewBag.category = model.Searching(db);
                 var cart = HttpContext.Session.GetObjectFromJson<IndexViewModel>("ToyNames");
            if(cart == null)
            {
                ViewBag.err = "Корзина пуста";
            }
                return View(cart);
       
                
        }


      

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        [HttpPost]
        public ActionResult Create(int toyId, int userId)
        {
            userId = ViewBag.currentId;
            return View();
        }


        [HttpPost]
        public ActionResult RemoveSession()
        {
             HttpContext.Session.Remove("ToyNames");
           return RedirectToAction("Index", "Home");
        }

        // POST: OrderController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
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
