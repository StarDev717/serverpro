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
    public class BuildingService : IGenericService<BuildingDto>
    {
        private readonly IUnitOfWork uow;

        public BuildingService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<BuildingDto> Create(BuildingDto item)
        {
            var building = item.MapTo<Building>();
            try
            {
                uow.BuildingRepository.Add(building);
                await uow.Commit();
            }
            catch
            {
                uow.RejectChanges();
                throw;
            }
            return await GetAsynById(building.Id);
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public BuildingDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BuildingDto> GetAll()
        {
            var buildings = uow.BuildingRepository.GetAll();
            return buildings.EnumerableTo<BuildingDto>();
        }

        public bool Update(BuildingDto building)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BuildingDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BuildingDto> GetAsynById(int id)
        {
            var building = await uow.BuildingRepository.GetAsync(m => m.Id == id);
            var buildingDto = building.MapTo<BuildingDto>();
            return await Task.FromResult<BuildingDto>(buildingDto);
        }

        public Task<BuildingDto> UpdateAsyn(BuildingDto item, int id)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAsyncFew(Expression<Func<BuildingDto, bool>> predicate, Expression<Func<BuildingDto, object>> select, params Expression<Func<BuildingDto, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<BuildingDto> GetAsynById(int id, params string[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
