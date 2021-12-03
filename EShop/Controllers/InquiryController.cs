using Eshop.DataAccess.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Controllers
{
    public class InquiryController : Controller
    {

        private readonly IInquiryHeaderRepository _inqHeaderRepository;
        private readonly IInquiryDetailRepository _inqDetRepository;


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

        #region APICalls
        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _inqDetRepository.GetAll() });
        }
        #endregion
    }
}
