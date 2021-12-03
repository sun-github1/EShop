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
using Eshop.DataAccess.IRepository;

namespace EShop.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductRepository _prodRepository;
        private readonly IApplicationUserRepository _appUserRepository;
        private readonly IInquiryHeaderRepository _inqHeaderRepository;
        private readonly IInquiryDetailRepository _inqDetRepository;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;
        [BindProperty]
        public ProductUserViewModel ProductUserVM { get; set; }

        public CartController(IProductRepository prodRepository,
            IApplicationUserRepository appUserRepository,
            IInquiryHeaderRepository inqHeaderRepository,
            IInquiryDetailRepository inqDetRepository,
            IWebHostEnvironment webHostEnvironment,
            IEmailSender emailSender)
        {
            this._prodRepository = prodRepository;
            _appUserRepository = appUserRepository;
            _inqDetRepository = inqDetRepository;
            _inqHeaderRepository = inqHeaderRepository;
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
                prodList = _prodRepository.GetAll(x => productInCart.Contains(x.Id), isTracking:false);
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
            IEnumerable<Product> prodList = _prodRepository.GetAll(x => productInCart.Contains(x.Id), isTracking: false);

            ProductUserVM = new ProductUserViewModel() { 
                ProductList= prodList.ToList(),
                ApplicationUser=_appUserRepository.FirstOrDefault(u=>u.Id==claim.Value)
            };

            return View(ProductUserVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(ProductUserViewModel productvm)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

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

            //add inquiry header

            InquiryHeader inquiryHeader = new InquiryHeader() { 
            ApplicationUserId= claim.Value,
            FullName= ProductUserVM.ApplicationUser.FullName,
            Email = ProductUserVM.ApplicationUser.Email,
            PhoneNumber = ProductUserVM.ApplicationUser.PhoneNumber,
            InquiryDate = DateTime.Now,
            };

            _inqHeaderRepository.Add(inquiryHeader);
            _inqHeaderRepository.SaveChanges();
            foreach (var prod in ProductUserVM.ProductList)
            {
                InquiryDetail inquiryDetail = new InquiryDetail()
                {
                    InquiryHeaderId = inquiryHeader.Id,
                    ProductId=prod.Id
                };
                _inqDetRepository.Add(inquiryDetail);             
            }
            _inqDetRepository.SaveChanges();

            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }
    }
}
