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
    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _applicationTypeRepository;
        public ApplicationTypeController(IApplicationTypeRepository applicationTypeRepository)
        {
            this._applicationTypeRepository = applicationTypeRepository;
        }

        public IActionResult Index()
        {
            var result = _applicationTypeRepository.GetAll();
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
                _applicationTypeRepository.Add(applicationType);
                _applicationTypeRepository.SaveChanges();
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "ApplicationType could not be updated";
            return View();
        }

        [HttpGet]//get data clikcing on create
        public IActionResult Edit(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                return NotFound();
            }
            var result = _applicationTypeRepository.FirstOrDefault(x => x.Id == id);

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
                _applicationTypeRepository.Update(appType);
                _applicationTypeRepository.SaveChanges();
                TempData[WC.Success] = "ApplicationType updated successfully";
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "ApplicationType could not be updated";
            return View(appType);
        }

        [HttpGet]//get data clikcing on create
        public IActionResult Delete(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                return NotFound();
            }
            var result = _applicationTypeRepository.FirstOrDefault(x => x.Id == id);

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
            var result = _applicationTypeRepository.FirstOrDefault(x => x.Id == id);
            _applicationTypeRepository.Remove(result);
            _applicationTypeRepository.SaveChanges();
            TempData[WC.Success] = "ApplicationType Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
