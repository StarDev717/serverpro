using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL.Resources;
using DAL.Services.Interfaces;
using ApiServer.Results;
using System.IO;
using System.Net.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Microsoft.Extensions.Options;
using DAL.Core.Models;
using DAL.Helpers;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IOptions<S3ServiceSettings> _S3ServiceSettings;
        private readonly IGenericService<ProjectDto> projectService;
        private readonly IGenericService<CreateProjectDto> createProjectService;
        private readonly IGenericService<BuildingDto> buildingService;
        private readonly IGenericService<EmployeeDto> employeeService;

        public ProjectsController(IOptions<S3ServiceSettings> _s3ServiceSettings, IGenericService<EmployeeDto> employeeService, IGenericService<BuildingDto> buildingService, IGenericService<ProjectDto> projectService, IGenericService<CreateProjectDto> createProjectService)
        {
            _S3ServiceSettings = _s3ServiceSettings;
            this.employeeService = employeeService;
            this.buildingService = buildingService;
            this.projectService = projectService;
            this.createProjectService = createProjectService;
        }

        // GET: api/Projects
        [HttpGet]
        public GenericResult<IEnumerable<ProjectDto>> GetProject()
        {
            var result = new GenericResult<IEnumerable<ProjectDto>>();
            try
            {
                result.Result = projectService.GetAll();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }
        
        // GET: api/Projects/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new GenericResult<ProjectDto>();

            try
            {
                result.Result = await projectService.GetAsynById(id);
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

        [HttpGet("{id}/buildings")]
        public async Task<IActionResult> GetProjectBuildings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new GenericResult<ProjectDto>();

            try
            {
                result.Result = await projectService.GetAsynById(id, new string[] { "Buildings" });
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

        [HttpGet("{id}/employees")]
        public async Task<IActionResult> GetProjectEmployees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new GenericResult<ProjectDto>();

            try
            {
                result.Result = await projectService.GetAsynById(id, new string[] { "Employees" });
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

        [HttpGet("{id}/reports")]
        public async Task<IActionResult> GetProjectReports([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new GenericResult<ProjectDto>();

            try
            {
                result.Result = await projectService.GetAsynById(id, new string[] { "Reports" });
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

        // PUT: api/Projects/5
        [HttpPut("{id}")]
        public async Task<GenericResult<ProjectDto>> PutProject([FromRoute] int id, [FromBody] ProjectDto projectDto)
        {
            var result = new GenericResult<ProjectDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }

                if (id != projectDto.Id)
                {
                    result.Success = false;
                    throw new Exception("Invalid Request!");
                }
                var resultData = await projectService.UpdateAsyn(projectDto, id);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        // POST: api/Projects
        [HttpPost]
        public async Task<GenericResult<CreateProjectDto>> PostProject([FromBody] CreateProjectDto projectDto)
        {
            StringValues headerValues;
            var UserId = string.Empty;
            if (Request.Headers.TryGetValue("UserId", out headerValues))
            {
                UserId = headerValues.FirstOrDefault();
            }
            projectDto.CreatedBy = UserId;
            var result = new GenericResult<CreateProjectDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }
                projectDto.IsActive = true;
                var resultData = await createProjectService.Create(projectDto);
                var projectId = resultData.Id;

                string fileName = projectDto.Buildings;
                var uploadService = new UploadService(_S3ServiceSettings.Value.AccessKey, _S3ServiceSettings.Value.SecretKey, fileName);
                string csvData = uploadService.GetObjectData(fileName).Result;

                List<string> records = new List<string>();

                int count = 1;
                foreach (string row in csvData.Split("\r\n"))
                {
                    if (!string.IsNullOrEmpty(row) && count != 1)
                    {
                        var str = row.Split(',');
                        await buildingService.Create(new BuildingDto
                        {
                            ProjectId = (int)projectId,
                            Title = str[0],
                            Class = str[1],
                            Submarket = str[2],
                            Address = str[3],
                            City = str[4],
                            Country = str[5],
                            State = str[6],
                            Zip = str[7],
                            IsActive = true
                        });
                    }
                    count++;
                }

                fileName = projectDto.Employees;
                records = new List<string>();
                csvData = uploadService.GetObjectData(fileName).Result;

                count = 1;
                foreach (string row in csvData.Split("\r\n"))
                {
                    if (!string.IsNullOrEmpty(row) && count != 1)
                    {
                        var str = row.Split(',');
                        await employeeService.Create(new EmployeeDto
                        {
                            ProjectId = (int)projectId,
                            FirstName = str[0],
                            LastName = str[1],
                            Email = str[2],
                            Phone = str[3],
                            Address = str[4],
                            City = str[5],
                            Country = str[6],
                            State = str[7],
                            Zip = str[8],
                            IsActive = true
                        });
                    }
                    count++;
                }
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public GenericResult<Boolean> DeleteProject([FromRoute] int id)
        {
            var result = new GenericResult<Boolean>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }
                var recordExists = ProjectExists(id).Result;
                if (recordExists == false)
                {
                    result.Success = false;
                    throw new Exception("Project not found!");
                }
                var resultData = projectService.Delete(id);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        private async Task<bool> ProjectExists(int id)
        {
            var resultData = await projectService.GetAsynById(id);
            bool result = resultData == null ? false : true;
            return await Task.FromResult(result);
        }

    }
}