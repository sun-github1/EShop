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

    public class ApplicationUserRepository : GenericRepository<ApplicationUser>,
        IApplicationUserRepository
    {
        private readonly AppDbContext _context;
        public ApplicationUserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

      
    }
}
