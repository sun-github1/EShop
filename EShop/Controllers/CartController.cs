using EShop.Utility;
using EShop.Models;
using EShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Eshop.Utility;
using Eshop.DataAccess.DataAccessLayer;

namespace EShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ProductUserViewModel ProductUserVM { get; set; }

        public CartController(AppDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IEmailSender emailSender)
        {
            this._context = context;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> prodList = null;
            List<ShoppingCart> lisifShoppingCart = new List<ShoppingCart>();
            var cart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);
            if (cart != null && cart.Count() > 0)
            {
                List<int> productInCart = cart.Select(x => x.ProductId).ToList();
                prodList = _context.Products.Where(x => productInCart.Contains(x.Id));
            }
            else
            {
                prodList = new List<Product>();
            }

            return View(prodList);
        }


        public IActionResult Remove(int? id)
        {
            IEnumerable<Product> prodList = new List<Product>();
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
            }
            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult Summary()
        {
            //user info
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            // var userId = User.FindFirstValue(ClaimTypes.Name);
            List<ShoppingCart> lisifShoppingCart = new List<ShoppingCart>();
            var cart = HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart);

            if (cart != null && cart.Count() > 0)
            {
                lisifShoppingCart = cart.ToList();
            }
            List<int> productInCart = cart.Select(x => x.ProductId).ToList();
            IEnumerable<Product> prodList = _context.Products.Where(x => productInCart.Contains(x.Id));

            ProductUserVM = new ProductUserViewModel() { 
                ProductList= prodList.ToList(),
                ApplicationUser=_context.ApplicationUsers.FirstOrDefault(u=>u.Id==claim.Value)
            };

            return View(ProductUserVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserViewModel productvm)
        {
            var pathToTemplate=_webHostEnvironment.WebRootPath
                +Path.DirectorySeparatorChar.ToString()
                +"template"+Path.DirectorySeparatorChar.ToString()
                +"Inquiry.html";
            var subject = "New Inquiry";
            
            byte[] data = System.IO.File.ReadAllBytes(pathToTemplate);
            var htmlBody = Convert.ToString(data);

            StringBuilder productListSB=new StringBuilder();
            foreach(var prod in ProductUserVM.ProductList)
            {
                productListSB.Append($" - Name {prod.ProductName} <span style='font-size:14px;'> (ID: {prod.Id}) </span><br/>");
            }
            string messagebody = string.Format(htmlBody,
                ProductUserVM.ApplicationUser.FullName,
                ProductUserVM.ApplicationUser.Email,
                ProductUserVM.ApplicationUser.PhoneNumber,
                productListSB.ToString()
                );


            await _emailSender.SendEmailAsync("",subject,messagebody);

            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }
    }
}
