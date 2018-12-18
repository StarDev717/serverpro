using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        T Get(Func<T, bool> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsyncIncluding(Expression<Func<T, bool>> predicate, params string[] includes);
        void Add(T entity);
        void Attach(T entity);
        Task<T> UpdateAsync(T updated, int key);
        void Delete(T entity);
    }
}
