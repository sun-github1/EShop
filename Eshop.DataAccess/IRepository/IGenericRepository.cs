﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Eshop.DataAccess.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        T Find(int id);
        IEnumerable<T> GetAll(
             Expression<Func<T, bool>> filter=null,
             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy=null,
             List<string> includeProperties =null,
             bool isTracking=true
            );

        T FirstOrDefault(
            Expression<Func<T, bool>> filter = null,
             List<string> includeProperties = null,
             bool isTracking = true
            );

        void Add(T entity);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);

        //void Update(T entity);

        void SaveChanges();
    }
}
