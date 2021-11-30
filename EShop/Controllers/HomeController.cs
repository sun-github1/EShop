using EShop.DataAccessLayer;
using EShop.Extension;
using EShop.Models;
using EShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger,
            AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel()
            {
                Products = _context.Products.Include(x => x.Category)
                .Include(y => y.ApplicationType),
                Categories = _context.Categories.ToList()
            };
            return View(homeVM);
        }


        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                DetailsViewModel detailsVM = new DetailsViewModel()
                {
                    Product = _context.Products.Include(x => x.Category)
                        .Include(y => y.ApplicationType).FirstOrDefault(x => x.Id == id.Value),
                    ExistsInCart = false
                };

                List<ShoppingCart> lisifShoppingCart = new List<ShoppingCart>();
                var cart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

                if (cart != null && cart.Count() > 0)
                {
                    lisifShoppingCart = cart.ToList();
                    if(lisifShoppingCart.FirstOrDefault(x=>x.ProductId==detailsVM.Product.Id)!=null)
                    {
                        detailsVM.ExistsInCart = true;
                    }
                }
                else
                {
                    detailsVM.ExistsInCart = false;
                }

                return View(detailsVM);
            }
            return NotFound();
            
        }

        [HttpPost, ActionName("Details")]
        public IActionResult DetailsPost(int? id)
        {
            if (id.HasValue)
            {
                List<ShoppingCart> lisifShoppingCart = new List<ShoppingCart>();
                var cart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

                if (cart!=null && cart.Count()>0)
                {
                    lisifShoppingCart = cart.ToList();
                }
                lisifShoppingCart.Add(new ShoppingCart() { ProductId = id.Value });
                HttpContext.Session.Set<IEnumerable<ShoppingCart>>(WC.SessionCart, lisifShoppingCart);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();

        }


        public IActionResult RemoveFromCart(int? id)
        {
            if (id.HasValue)
            {
                List<ShoppingCart> lisifShoppingCart = new List<ShoppingCart>();
                var cart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

                if (cart != null && cart.Count() > 0)
                {
                    lisifShoppingCart = cart.ToList();
                    var item = lisifShoppingCart.FirstOrDefault(x => x.ProductId == id.Value);
                    if (item != null)
                    {
                        lisifShoppingCart.Remove(item);
                    }
                }
                HttpContext.Session.Set<IEnumerable<ShoppingCart>>(WC.SessionCart, lisifShoppingCart);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();

        }


        public IActionResult Privacy()
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
