using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Timelogger.Infrastructure.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void AddAndSave(T entity);
        T FirstOrDefault(Expression<Func<T, bool>> expression);
        void Update(T entity);
        void UpdateAndSave(T entity);
    }
}
