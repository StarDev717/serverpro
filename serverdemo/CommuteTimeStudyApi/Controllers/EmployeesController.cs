using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL.Services.Interfaces;
using DAL.Resources;
using ApiServer.Results;
using Microsoft.AspNetCore.Authorization;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IGenericService<EmployeeDto> employeeService;

        public EmployeesController(IGenericService<EmployeeDto> employeeService)
        {
            this.employeeService = employeeService;
        }

        // GET: api/Employees
        [Authorize]
        [HttpGet]
        public GenericResult<IEnumerable<EmployeeDto>> GetEmployee()
        {
            var result = new GenericResult<IEnumerable<EmployeeDto>>();

            try
            {
                result.Result = employeeService.GetAll();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new GenericResult<EmployeeDto>();

            try
            {
                result.Result = await employeeService.GetAsynById(id);
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            if (result.Result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<GenericResult<EmployeeDto>> PutEmployee([FromRoute] int id, [FromBody] EmployeeDto employeeDto)
        {
            var result = new GenericResult<EmployeeDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }

                if (id != employeeDto.Id)
                {
                    result.Success = false;
                    throw new Exception("Invalid Request!");
                }
                var resultData = await employeeService.UpdateAsyn(employeeDto, id);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<GenericResult<EmployeeDto>> PostEmployee([FromBody] EmployeeDto employeeDto)
        {
            var result = new GenericResult<EmployeeDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }
                var resultData = await employeeService.Create(employeeDto);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public GenericResult<Boolean> DeleteEmployee([FromRoute] int id)
        {
            var result = new GenericResult<Boolean>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }
                var recordExists = EmployeeExists(id).Result;
                if (recordExists == false)
                {
                    result.Success = false;
                    throw new Exception("Employee not found!");
                }

                var resultData = employeeService.Delete(id);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        private async Task<bool> EmployeeExists(int id)
        {
            var resultData = await employeeService.GetAsynById(id);
            bool result = resultData == null ? false : true;
            return await Task.FromResult(result);
        }

    }
}