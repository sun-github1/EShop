using Eshop.DataAccess.DataAccessLayer;
using Eshop.DataAccess.IRepository;
using EShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }   

        public void Update(Category entity)
        {
            var objFromDb = base.FirstOrDefault(u => u.Id == entity.Id);
            if (objFromDb != null)
            {
                objFromDb.CategoryName = entity.CategoryName;
                objFromDb.DisplayOrder=entity.DisplayOrder;
            }
        }

        
    }
}
