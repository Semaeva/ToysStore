using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ToyStore.EmailServices;
using ToyStore.Models;
using ToyStore.SessionExtension;
using ToyStore.ViewsModel;

namespace ToyStore.Controllers
{
    public class OrderController : Controller
    {
        ApplicationContext db;
        private IndexViewModel model = new IndexViewModel();
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public OrderController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext context) {
            db = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        // GET: OrderController
        public ActionResult Index()
        {
            ViewBag.category = model.Searching(db);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// will give the user's userId
            ViewBag.userId = db.Users.Find(userId).Id;
            ViewBag.name = db.Users.Find(userId).UserName;
            ViewBag.email = db.Users.Find(userId).Email;
            ViewBag.area = db.Users.Find(userId).Area;
            ViewBag.city = db.Users.Find(userId).City;
            ViewBag.phone = db.Users.Find(userId).PhoneNumber;
            ViewBag.street = db.Users.Find(userId).Street;
            ViewBag.house = db.Users.Find(userId).House;
            List<SelectListItem> itemsCart = CartList();
            return View(itemsCart);
        }

        [HttpPost]
        public IActionResult Index(string[] carts, User user)
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

        [HttpPost]
        public async Task<ActionResult> SendEmail(string name, string comment, string quantity, string email, string phone, string userId, string area, List<int> toyId, string city, List<string> toys, string street, string house)
        {
           
            try
            {
                foreach (var t in toyId)
                {
                    var orders = new toysusers()
                    {
                        toysID = t,
                        usersId = userId,
                        quantity = Int32.Parse(quantity),
                        comment = comment
                    };
               
                    db.Add(orders);
                    db.SaveChanges();
                }
            }
            catch (Exception ex) { }
            var getTrackingUrl = Url.Action(
                "GetTracking",
                "Order",
                new { usersId=userId,toysId=toyId,quantity= quantity },
                protocol: HttpContext.Request.Scheme);

            EmailService emailService = new EmailService();
            var items = "";
            foreach (var item in toys)
            {
                items+= "," +item;
            }
            await emailService.SendEmailAsync(email, "Заказ принят",
                $" ФИО: {name}, Товар: {items}, Адрес:{city}, {area},{street}, {house}, ссылка на слежение за состоянием заказа:  <a href='{getTrackingUrl}'>ссылка</a>");
           
            return Content("Детали заказа можете увидеть в сообщении, высланном на почту");

        }

        public async Task<IActionResult> GetTracking(string userId, string toyId, string quantity)
        {

            return View();
        }


        private List<SelectListItem> CartList( ) {
            List<SelectListItem> items = new List<SelectListItem>();
             try
            {
                var cart = HttpContext.Session.GetObjectFromJson<IndexViewModel>("ToyNames");
                var test = cart.Toys.ToList();
                ViewBag.cart = cart.Toys.ToList();
           
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

            catch (Exception ex) { }
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
        public async Task<ActionResult> Details(int? id)
            {
            ViewBag.category = model.Searching(db);

            if (id != null)
            {
                Toys toy = await db.toys.FirstOrDefaultAsync(p => p.Id == id);
                if (toy != null)
                    return View(toy);
            }
            return NotFound();
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
