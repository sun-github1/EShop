using Eshop.DataAccess.DataAccessLayer;
using Eshop.DataAccess.IRepository;
using EShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.Repository
{
    public class ApplicationTypeRepository : GenericRepository<ApplicationType>,
        IApplicationTypeRepository
    {
        private readonly AppDbContext _context;
        public ApplicationTypeRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(ApplicationType entity)
        {
            var objFromDb = base.FirstOrDefault(u => u.Id == entity.Id);
            if (objFromDb != null)
            {
                objFromDb.ApplicationName = entity.ApplicationName;
                
            }
        }
    }
}
