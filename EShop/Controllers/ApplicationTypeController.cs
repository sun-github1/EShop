using EShop.DataAccessLayer;
using EShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private readonly AppDbContext _context;
        public ApplicationTypeController(AppDbContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            var result = _context.ApplicationTypes.ToList();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ApplicationType applicationType)
        {
            if (ModelState.IsValid)
            {
                _context.ApplicationTypes.Add(applicationType);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
