using DAL.Core;
using DAL.Core.Models;
using DAL.Extensions;
using DAL.Resources;
using DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class ProjectService<T> : IGenericService<T> where T : class
    {
        private readonly IUnitOfWork uow;

        public ProjectService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<T> Create(T item)
        {
            var project = item.MapTo<Project>();
            try
            {
                uow.ProjectRepository.Add(project);
                await uow.Commit();
            }
            catch
            {
                uow.RejectChanges();
                throw;
            }
            return await GetAsynById(project.Id);
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            var projects = uow.ProjectRepository.GetAll(p => p.CreatedByUser);
            projects = projects.OrderByDescending(p => p.CreatedBy);
            var projectsList = projects.EnumerableTo<T>();
            return projectsList;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsynById(int id)
        {
            var project = await uow.ProjectRepository.GetAsync(m => m.Id == id, p => p.CreatedByUser);
            var projectDto = project.MapTo<T>();
            return await Task.FromResult<T>(projectDto);
        }

        public async Task<T> GetAsynById(int id, bool includeRelated=false)
        {
            var project = await uow.ProjectRepository.GetAsync(m => m.Id == id);
            var projectDto = project.MapTo<T>();
            return await Task.FromResult<T>(projectDto);
        }

       
        public async Task<T> GetAsynById(int id, params string[] includes)
        {
            var project = await uow.ProjectRepository.GetAsyncIncluding(m => m.Id == id, includes);
            var projectDto = project.MapTo<T>();
            return await Task.FromResult<T>(projectDto);
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

        public async Task<T> UpdateAsyn(T item, int id)
        {
            var project = item.MapTo<Project>();
            project.Id = id;

            //tpajvsupervisor.delta@delta.com

            //Attention Nick Bradford
            try
            {
                await uow.ProjectRepository.UpdateAsync(project, id);
                await uow.Commit();
            }
            catch
            {
                uow.RejectChanges();
                throw;
            }
            return await GetAsynById(project.Id);
        }
    }
}
