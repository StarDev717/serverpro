using DAL.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL.Repository
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        private readonly ApiContext _context;
        private DbSet<T> _objectSet;
        public GenericRepository(ApiContext context)
        {
            _context = context;
            _objectSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _objectSet.Add(entity);
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _objectSet.AsQueryable();
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public T Get(Func<T, bool> predicate)
        {
            return _objectSet.First(predicate);
        }

        public void Attach(T entity)
        {
        }

        public void Delete(T entity)
        {
            _objectSet.Remove(entity);
        }

        //public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        //{
        //    var result = await _objectSet.FirstOrDefaultAsync(predicate);
        //    return result;
        //}
        public async Task<T> UpdateAsync(T updated, int key)
        {
            if (updated == null)
                return null;

            T existing = await _objectSet.FindAsync(key);
            var buildings = _context.Building;
            var Newbuildings = buildings;
            if (existing != null)
            {
                _context.Entry(existing).State = EntityState.Modified;
                _context.Attach(existing);
                var entry = _context.Entry(existing);
                var properties = existing.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.GetValue(updated) != null)
                    {
                        var newvalue = property.GetValue(updated);
                        property.SetValue(existing, newvalue);
                    }
                }
                entry.CurrentValues.SetValues(updated);
                await _context.SaveChangesAsync();
            }
            return existing;
        }
        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> set = _objectSet.Where(predicate);
            return (await includes.Aggregate(set, (current, include) => current.Include(include)).FirstOrDefaultAsync() ?? default(T));
        }

        public async Task<object> GetAsyncFew(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> select, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> set = _objectSet.Where(predicate);
            return (await includes.Aggregate(set, (current, include) => current.Include(include)).Select(select).FirstOrDefaultAsync() ?? default(T));
        }

        public async Task<T> GetAsyncIncluding(Expression<Func<T, bool>> predicate, params string[] includes)
        {
            IQueryable<T> set = _objectSet.Where(predicate);

            foreach (var navProperty in includes)
            {
                set = set.Include(navProperty);
            }
            return (await includes.Aggregate(set, (current, include) => current.Include(include)).FirstOrDefaultAsync() ?? default(T));
        }
    }
}
