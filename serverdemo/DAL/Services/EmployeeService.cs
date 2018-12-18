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
    public class EmployeeService : IGenericService<EmployeeDto>
    {
        private readonly IUnitOfWork uow;

        public EmployeeService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<EmployeeDto> Create(EmployeeDto item)
        {
            var employee = item.MapTo<Employee>();
            try
            {
                uow.EmployeeRepository.Add(employee);
                await uow.Commit();
            }
            catch
            {
                uow.RejectChanges();
                throw;
            }

            return await GetAsynById(employee.Id);
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public EmployeeDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmployeeDto> GetAll()
        {
            var employees = uow.EmployeeRepository.GetAll();
            return employees.EnumerableTo<EmployeeDto>();
        }

        public bool Update(EmployeeDto employee)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeDto> GetAsynById(int id)
        {
            var employee = await uow.EmployeeRepository.GetAsync(m => m.Id == id);
            var employeeDto = employee.MapTo<EmployeeDto>();
            return await Task.FromResult<EmployeeDto>(employeeDto);
        }

        public async Task<EmployeeDto> UpdateAsyn(EmployeeDto item, int id)
        {
            var employee = item.MapTo<Employee>();
            employee.Id = id;
            try
            {
                await uow.EmployeeRepository.UpdateAsync(employee, id);
                await uow.Commit();
            }
            catch
            {
                uow.RejectChanges();
                throw;
            }
            return await GetAsynById(employee.Id);
        }

        public Task<object> GetAsyncFew(Expression<Func<EmployeeDto, bool>> predicate, Expression<Func<EmployeeDto, object>> select, params Expression<Func<EmployeeDto, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeDto> GetAsynById(int id, params string[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
