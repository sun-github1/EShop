using EShop.DataAccessLayer;
using EShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
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

        [HttpGet]//get data clikcing on create
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                return NotFound();
            }
            var result = _context.ApplicationTypes.FirstOrDefault(x => x.Id == id);

            if (result != null)
            {
                return View(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]//get data clikcing on create
        public IActionResult Edit(ApplicationType appType)
        {
            if (ModelState.IsValid)
            {
                _context.ApplicationTypes.Update(appType);
                _context.SaveChanges();
                TempData["success"] = "ApplicationType updated successfully";
                return RedirectToAction("Index");
            }
            return View(appType);
        }

        [HttpGet]//get data clikcing on create
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                return NotFound();
            }
            var result = _context.ApplicationTypes.FirstOrDefault(x => x.Id == id);

            if (result != null)
            {
                return View(result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult DeleteConfirm(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                return NotFound();
            }
            var result = _context.ApplicationTypes.FirstOrDefault(x => x.Id == id);
            _context.ApplicationTypes.Remove(result);
            _context.SaveChanges();
            TempData["success"] = "ApplicationType Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
