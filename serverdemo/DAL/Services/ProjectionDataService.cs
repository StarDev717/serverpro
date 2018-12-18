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
    public class ProjectionDataService : IGenericService<ProjectionDataDto>
    {
        private readonly IUnitOfWork uow;

        public ProjectionDataService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<ProjectionDataDto> Create(ProjectionDataDto item)
        {
            var projectionData = item.MapTo<ProjectionData>();
            try
            {
                uow.ProjectionDataRepository.Add(projectionData);
                await uow.Commit();
            }
            catch
            {
                uow.RejectChanges();
                throw;
            }
            return await GetAsynById(projectionData.Id);
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ProjectionDataDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProjectionDataDto> GetAll()
        {
            var projectionDatas = uow.ProjectionDataRepository.GetAll();
            return projectionDatas.EnumerableTo<ProjectionDataDto>();
        }

        public bool Update(ProjectionDataDto projectionData)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProjectionDataDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectionDataDto> GetAsynById(int id)
        {
            var projectionData = await uow.ProjectionDataRepository.GetAsync(m => m.Id == id);
            var projectionDataDto = projectionData.MapTo<ProjectionDataDto>();
            return await Task.FromResult<ProjectionDataDto>(projectionDataDto);
        }

        public Task<ProjectionDataDto> UpdateAsyn(ProjectionDataDto item, int id)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAsyncFew(Expression<Func<ProjectionDataDto, bool>> predicate, Expression<Func<ProjectionDataDto, object>> select, params Expression<Func<ProjectionDataDto, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectionDataDto> GetAsynById(int id, params string[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
