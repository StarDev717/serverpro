using DAL.Core;
using DAL.Core.Models;
using DAL.Extensions;
using DAL.Resources;
using DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class ReportService<T> : IGenericService<T> where T : class
    {
        private readonly IUnitOfWork uow;

        public ReportService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<T> Create(T item)
        {
            var report = item.MapTo<Report>();
            try
            {
                uow.ReportRepository.Add(report);
                await uow.Commit();
            }
            catch
            {
                uow.RejectChanges();
                throw;
            }
            return await GetAsynById(report.Id);
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            var reports = uow.ReportRepository.GetAll();
            return reports.EnumerableTo<T>();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsynById(int id)
        {
            var report = await uow.ReportRepository.GetAsync(m => m.Id == id, x => x.Projections);
            var reportDto = report.MapTo<T>();
            return await Task.FromResult<T>(reportDto);
        }

        public async Task<T> GetAsynById(int id, params string[] includes)
        {
            var report = await uow.ReportRepository.GetAsyncIncluding(m => m.Id == id, includes);
            var reportDto = report.MapTo<T>();
            return await Task.FromResult<T>(reportDto);
        }

        public Task<object> GetAsyncFew(Expression<Func<T, bool>> predicate, Expression<Func<T, object>> select, params Expression<Func<T, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {

            throw new NotImplementedException();
        }

        public bool Update(T item)
        {
            throw new NotImplementedException();
        }

        public Task<T> UpdateAsyn(T item, int id)
        {
            throw new NotImplementedException();
        }
    }
}
