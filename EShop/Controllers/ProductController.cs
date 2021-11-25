using EShop.DataAccessLayer;
using EShop.Models;
using EShop.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(AppDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this._webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> listOfProduct = _context.Products
                .Include(x => x.Category).ToList();
            return View(listOfProduct);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> categorydropdown = _context.
            //    Categories.Select(i => new SelectListItem { 
            //        Text=i.CategoryName,
            //        Value = i.Id.ToString()
            //    });
            //ViewBag.CategoryDropdown = categorydropdown;

            ProductViewModel productVM = new ProductViewModel()
            {
                Product = new Product(),
                CategorySelectList = _context.
                    Categories.Select(i => new SelectListItem
                    {
                        Text = i.CategoryName,
                        Value = i.Id.ToString()
                    })
            };

            if (id.HasValue && id > 0)
            {//for update
                var result = _context.Products.FirstOrDefault(x => x.Id == id);

                if (result != null)
                {
                    productVM.Product = result;
                    return View(productVM);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return View(productVM);
            }
        }

        [HttpPost]//Post data clikcing on create
        public IActionResult Upsert(ProductViewModel productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webrootpath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id > 0)
                {//for update
                    _context.Products.Update(productVM.Product);
                    _context.SaveChanges();
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    string upload = webrootpath + WC.ImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream
                        (Path.Combine(upload, filename, extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.ImagePath = filename + extension;

                    _context.Products.Add(productVM.Product);
                    _context.SaveChanges();
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction("Index");
                }
            }
            return View(productVM);
        }


        //[HttpGet]//get data clikcing on create
        //public IActionResult Edit(int? id)
        //{
        //    if (!id.HasValue || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    var result = _context.Products.FirstOrDefault(x => x.Id == id);

        //    if (result != null)
        //    {
        //        return View(result);
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //[HttpPost]//get data clikcing on create
        //public IActionResult Edit(Product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Products.Update(product);
        //        _context.SaveChanges();
        //        TempData["success"] = "Product updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View(product);
        //}

        [HttpGet]//get data clikcing on create
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                return NotFound();
            }
            var result = _context.Products.FirstOrDefault(x => x.Id == id);

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
            var result = _context.Products.FirstOrDefault(x => x.Id == id);
            _context.Products.Remove(result);
            _context.SaveChanges();
            TempData["success"] = "Product Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
