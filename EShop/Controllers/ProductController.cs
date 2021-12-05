using Eshop.DataAccess.DataAccessLayer;
using Eshop.DataAccess.IRepository;
using Eshop.Utility;
using EShop.Models;
using EShop.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository productRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            this._productRepository = productRepository;
            this._webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> listOfProduct = _productRepository.GetAll(null,
                null,
                includeProperties: new List<string>() { "Category", "ApplicationType" }
                , false)
                .ToList();
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
                CategorySelectList = _productRepository.
                    GetAllDropdownList(WC.CategoryName),
                ApplicationTypeSelectList = _productRepository.
                    GetAllDropdownList(WC.ApplicationTypeName),
            };

            if (id.HasValue && id > 0)
            {//for update
                var result = _productRepository.FirstOrDefault(x => x.Id == id, 
                    includeProperties: new List<string>() { "Category", "ApplicationType" },
                    isTracking:false);

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
                    var existingProduct = _productRepository
                        .FirstOrDefault(x=>x.Id== productVM.Product.Id, isTracking:false);

                    if(files.Count>0)
                    {
                        string upload = webrootpath + WC.ImagePath;
                        string filename = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        using (var fileStream = new FileStream
                             (Path.Combine(upload, filename+extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.ImagePath = filename + extension;
                        //delete old file if exists
                        var oldfile = Path.Combine(upload, existingProduct.ImagePath);

                        if (System.IO.File.Exists(oldfile))
                        {
                            System.IO.File.Delete(oldfile);
                        }
                        
                    }
                    else//file not updated, use the old image only
                    {
                        productVM.Product.ImagePath = existingProduct.ImagePath;
                    }
                    _productRepository.Update(productVM.Product);
                    _productRepository.SaveChanges();
                    TempData[WC.Success] = "Product updated successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    string upload = webrootpath + WC.ImagePath;
                    string filename = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    using (var fileStream = new FileStream
                        (Path.Combine(upload, filename+extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.ImagePath = filename + extension;

                    _productRepository.Add(productVM.Product);
                    _productRepository.SaveChanges();
                    TempData[WC.Success] = "Product created successfully";
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
        //        TempData[WC.Success] = "Product updated successfully";
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
            var result = _productRepository.FirstOrDefault(x => x.Id == id);

           

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
            var result = _productRepository.FirstOrDefault(x => x.Id == id);
            if (!string.IsNullOrEmpty(result.ImagePath))
            {
                string webrootpath = _webHostEnvironment.WebRootPath;
                string upload = webrootpath + WC.ImagePath;
               
                //delete old file if exists
                var oldfile = Path.Combine(upload, result.ImagePath);

                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);
                }
            }

            _productRepository.Remove(result);
            _productRepository.SaveChanges();
            TempData[WC.Success] = "Product Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
