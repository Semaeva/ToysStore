using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Authorize]
        // GET: OrderController
        public ActionResult Index(int idUser, int idToys)
        {
            ViewBag.category = model.Searching(db);
         
            List<SelectListItem> itemsCart = CartList();
            return View(itemsCart);        
        }


        [HttpPost]
        public IActionResult Index(string[] carts)
        {
            ViewBag.category = model.Searching(db);
            int newList = 0;
            var cart = HttpContext.Session.GetObjectFromJson<IndexViewModel>("ToyNames").Toys.ToList();
            foreach (var c in carts) {

                cart.RemoveAll(r => r.Id == Int32.Parse(c));
                
            }
            IndexViewModel toys = new IndexViewModel { Toys = cart };

            HttpContext.Session.Remove("ToyNames");
            HttpContext.Session.SetObjectAsJson("ToyNames", toys);

            List<SelectListItem> items = CartList();
         
            return View(items);
        }

        private  List<SelectListItem> CartList( ) {
            var cart = HttpContext.Session.GetObjectFromJson<IndexViewModel>("ToyNames");
          var test = cart.Toys.ToList();
            ViewBag.cart = cart.Toys.ToList();

            //list item
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in cart.Toys)
            {
                items.Add(new SelectListItem
                {
                    Text = item.toy_name,
                    Value = item.Id.ToString() 
                });
            }
            return items;
        }


        [HttpPost]
        public ActionResult RemoveItems(int idUser, int idToys, bool IsRemoved)
        {
            ViewBag.category = model.Searching(db);
            var cart = HttpContext.Session.GetObjectFromJson<IndexViewModel>("ToyNames").Toys.ToList();
            var itemToRemove = cart.Single(r => r.Id == idToys);

            var newCart = cart.Remove(itemToRemove);
            return View(newCart);
        }


        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        [HttpPost]
        public ActionResult Create(int toyId, int userId, bool isRemoved)
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
