using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<T> Create(T item);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetById(int id);
        Task<T> GetAsynById(int id);
        Task<T> GetAsynById(int id, params string[] includes);
        Task<object> GetAsyncFew(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> select, params Expression<Func<T, object>>[] includes);
        Task<T> UpdateAsyn(T item, int id);
        bool Update(T item);
        bool Delete(int id);
    }
}
