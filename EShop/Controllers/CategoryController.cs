using Eshop.DataAccess.DataAccessLayer;
using Eshop.DataAccess.IRepository;
using Eshop.Utility;
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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryController(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        public IActionResult Index()
        {
            var result = _categoryRepository.GetAll();
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
                _categoryRepository.Add(category);
                _categoryRepository.SaveChanges();
                TempData["success"] = "Category created successfully";
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
            var result = _categoryRepository.FirstOrDefault(x=>x.Id==id.Value);

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
                _categoryRepository.Update(category);
                _categoryRepository.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
                return View(category);
        }

        [HttpGet]//get data clikcing on create
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                return NotFound();
            }
            var result = _categoryRepository.FirstOrDefault(x => x.Id == id);

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
            var result = _categoryRepository.FirstOrDefault(x => x.Id == id);
            _categoryRepository.Remove(result);
            _categoryRepository.SaveChanges();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
