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
    public class InquiryHeaderRepository : GenericRepository<InquiryHeader>, IInquiryHeaderRepository
    {
        private readonly AppDbContext _context;
        public InquiryHeaderRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(InquiryHeader entity)
        {
            _context.Update(entity);
        }

       
    }
}
