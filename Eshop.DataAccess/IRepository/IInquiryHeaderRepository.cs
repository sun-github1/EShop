using EShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.IRepository
{
    public interface IInquiryHeaderRepository : IGenericRepository<InquiryHeader>
    {
        void Update(InquiryHeader entity);
   


    }
}
