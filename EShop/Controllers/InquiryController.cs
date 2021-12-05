using Eshop.DataAccess.IRepository;
using Eshop.Utility;
using EShop.Models;
using EShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace EShop.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class InquiryController : Controller
    {

        private readonly IInquiryHeaderRepository _inqHeaderRepository;
        private readonly IInquiryDetailRepository _inqDetRepository;

        [BindProperty]
        public InquiryViewModel InquiryVM { get; set; }

        public InquiryController(IInquiryHeaderRepository inqHeaderRepository,
            IInquiryDetailRepository inqDetRepository)
        {           
            _inqDetRepository = inqDetRepository;
            _inqHeaderRepository = inqHeaderRepository;
           
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                InquiryVM = new InquiryViewModel()
                {
                    InquiryHeader= _inqHeaderRepository.FirstOrDefault(x=>x.Id==id),
                    InquiryDetails=_inqDetRepository.GetAll(u=>u.InquiryHeaderId==id.Value,
                        includeProperties:new List<string>() {"Product" }).ToList()
                };
                return View(InquiryVM);
            }
            else
            { 
                return NotFound(); 
            }
           
        }
        [HttpPost]
        public IActionResult Details()
        {
            InquiryVM.InquiryDetails=_inqDetRepository
                .GetAll(u=>u.InquiryHeader.Id==InquiryVM.InquiryHeader.Id,
                includeProperties: new List<string>() { "Product" }).ToList();

            List<ShoppingCart> listShoppongCart = new List<ShoppingCart>();
            foreach(var detail in InquiryVM.InquiryDetails)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId=detail.ProductId, 
                };
                listShoppongCart.Add(shoppingCart);
            }
           

            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, listShoppongCart);
            HttpContext.Session.Set(WC.SessionInquiryId, InquiryVM.InquiryHeader.Id);

            return RedirectToAction("Index","Cart");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = _inqHeaderRepository.FirstOrDefault(u => u.Id
                   == InquiryVM.InquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetails = _inqDetRepository.GetAll(x => x.InquiryHeaderId
                  == InquiryVM.InquiryHeader.Id);

            _inqDetRepository.RemoveRange(inquiryDetails);
            _inqHeaderRepository.Remove(inquiryHeader);

            _inqHeaderRepository.SaveChanges();

            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Index));
        }

        #region APICalls
        [HttpGet]
        public IActionResult GetInquiryList ()
        {
            return Json(new { data = _inqHeaderRepository.GetAll() });
        }
        #endregion


    }
}
