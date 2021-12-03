using Eshop.DataAccess.DataAccessLayer;
using Eshop.DataAccess.IRepository;
using Eshop.Utility;
using EShop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product entity)
        {
            _context.Update(entity);
            //var objFromDb = base.FirstOrDefault(u => u.Id == entity.Id);
            //if (objFromDb != null)
            //{
            //    objFromDb.ProductName = entity.ProductName;
            //    objFromDb.Description = entity.Description;
            //    objFromDb.ShortDesc = entity.ShortDesc;
            //    objFromDb.Price = entity.Price;
            //    objFromDb.ImagePath = entity.ImagePath;
            //}
        }

        public IEnumerable<SelectListItem> GetAllDropdownList(string objName)
        {
            if (objName == WC.CategoryName)
            {
                return _context.Categories.Select(
                    x => new SelectListItem
                    { 
                        Text=x.CategoryName,
                        Value=x.Id.ToString()
                    });
            }
            else if (objName == WC.ApplicationTypeName)
            {
                return _context.ApplicationTypes.Select(
                    x => new SelectListItem
                    {
                        Text = x.ApplicationName,
                        Value = x.Id.ToString()
                    });
            }
            else
            {
                return null;
            }
        }
    }
}
