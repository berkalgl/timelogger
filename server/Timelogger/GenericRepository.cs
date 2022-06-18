using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Timelogger.Infrastructure.Interfaces;

namespace Timelogger.Infrastructure
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IApiContext _context;
        protected readonly DbSet<T> _dbSet;
        public GenericRepository(IApiContext context)
        {
            this._context = context;
            _dbSet = context.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public void AddAndSave(T entity)
        {
            _dbSet.Add(entity);
            Save();
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet;
        }
        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }
        public void Save()
        {
            _context.Save();
        }
    }
}
