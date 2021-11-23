using EShop.DataAccessLayer;
using EShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            var result = _context.Categories.ToList();
            return View(result);
        }
        [HttpGet]//get data clikcing on create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]//Post data clikcing on create
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }


        [HttpGet]//get data clikcing on create
        public IActionResult Edit(int? id)
        {
            if(!id.HasValue || id ==0)
            {
                return NotFound();
            }
            var result = _context.Categories.FirstOrDefault(x => x.Id == id);

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
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                //var existingCat = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
                //existingCat.CategoryName = category.CategoryName;
                //existingCat.DisplayOrder = category.DisplayOrder;
                _context.Categories.Update(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
                return View(category);
        }
    }
}
