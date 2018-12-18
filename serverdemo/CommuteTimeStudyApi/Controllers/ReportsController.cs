using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Core.Models;
using DAL.Persistence;
using DAL.Resources;
using ApiServer.Results;
using DAL.Services.Interfaces;
using DAL.Helpers;
using Microsoft.Extensions.Options;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IOptions<ServiceSettings> _serviceSettings;
        private readonly IGenericService<ReportDto> reportService;
        private readonly IGenericService<CreateReportDto> createReportService;
        private readonly IGenericService<ProjectDto> projectService;

        public ReportsController(IOptions<ServiceSettings> serviceSettings, IGenericService<ProjectDto> projectService, IGenericService<ReportDto> reportService, IGenericService<CreateReportDto> createReportService)
        {
            _serviceSettings = serviceSettings;
            this.projectService = projectService;
            this.reportService = reportService;
            this.createReportService = createReportService;
        }

        // GET: api/Reports
        [HttpGet]
        public GenericResult<IEnumerable<ReportDto>> GetReport()
        {
            var result = new GenericResult<IEnumerable<ReportDto>>();

            try
            {
                result.Result = reportService.GetAll();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        // GET: api/Reports/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReport([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new GenericResult<ReportDto>();

            try
            {
                result.Result = await reportService.GetAsynById(id);
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

        // GET: api/Reports/5/buildings
        [HttpGet("{id}/buildings")]
        public async Task<IActionResult> GetReportBuidlings([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new GenericResult<ReportDto>();

            try
            {
                result.Result = await reportService.GetAsynById(id);
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

        // PUT: api/Reports/5
        [HttpPut("{id}")]
        public async Task<GenericResult<ReportDto>> PutReport([FromRoute] int id, [FromBody] ReportDto reportDto)
        {
            var result = new GenericResult<ReportDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }

                if (id != reportDto.Id)
                {
                    result.Success = false;
                    throw new Exception("Invalid Request!");
                }
                var resultData = await reportService.UpdateAsyn(reportDto, id);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        // POST: api/Reports
        [HttpPost]
        public async Task<GenericResult<CreateReportDto>> PostReport([FromBody] CreateReportDto reportDto)
        {
            var Url = _serviceSettings.Value.GMUrl;
            var Key = _serviceSettings.Value.GMApiKey;
            var result = new GenericResult<CreateReportDto>();
            var projectionsData = new List<CreateProjectionDto>();

            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }

                var data = await projectService.GetAsynById(reportDto.ProjectId, new string[] { "Buildings", "Employees" });
                var buildings = data.Buildings;
                var employees = data.Employees;
                var inputDestinationAddressesArray = new List<string>();
                var inputSourceAddressesArray = new List<string>();
                var buildingDictionary = new Dictionary<int, BuildingDto>();
                var employeeDictionary = new Dictionary<int, EmployeeDto>();
                int employeeIndex = 0;
                int buildingIndex = 0;
                foreach (var source in employees)
                {
                    var employeeAddress = string.Format("{0}, {1}, {2}, {3}, {4}", source.Address, source.City, source.State, source.Country, source.Zip);
                    if (!employeeDictionary.ContainsKey(employeeIndex))
                    {
                        inputSourceAddressesArray.Add(employeeAddress);
                        employeeDictionary.Add(employeeIndex, new EmployeeDto
                        {
                            FirstName = source.FirstName,
                            LastName = source.LastName,
                            City = source.City,
                            Address = source.Address,
                            Designation = source.Designation,
                            Email = source.Email,
                            Phone = source.Phone,
                            Zip = source.Zip,
                            State = source.State,
                            Country = source.Country
                        });
                        employeeIndex++;
                    }
                }

                foreach (var source in buildings)
                {
                    var buildingAddress = string.Format("{0}, {1}, {2}, {3}, {4}", source.Address, source.City, source.State, source.Country, source.Zip);
                    if (!buildingDictionary.ContainsKey(buildingIndex))
                    {
                        inputDestinationAddressesArray.Add(buildingAddress);
                        buildingDictionary.Add(buildingIndex, new BuildingDto
                        {
                            Title = source.Title,
                            Zip = source.Zip,
                            City = source.City,
                            Address = source.Address,
                            State = source.State,
                            Country = source.Country
                        });
                        buildingIndex++;
                    }
                }
                GoogleDistanceMatrixApi api = new GoogleDistanceMatrixApi(Url, Key, inputSourceAddressesArray.ToArray(), inputDestinationAddressesArray.ToArray());
                var response = await api.GetResponse();
                var originAdresses = response.OriginAddresses.ToList();
                var destinationAdresses = response.DestinationAddresses.ToList();
                var rows = response.Rows.ToList();
                foreach (var row in rows)
                {
                    int i = rows.IndexOf(row);
                    var elements = row.Elements.ToList();

                    foreach (var element in elements)
                    {
                        int j = elements.IndexOf(element);
                        var point = api.GetLongitudeAndLatitude(originAdresses[i], "false").Result;
                        //element.OriginLat = point.Latitude;
                        //element.OriginLong = point.Longitude;
                        //point = api.GetLongitudeAndLatitude(destinationAdresses[j], "false").Result;
                        //element.DestinationLat = point.Latitude;
                        //element.DestinationLong = point.Longitude;
                        element.OriginAddress = originAdresses[i];
                        element.DestinationAddress = destinationAdresses[j];

                        var address = destinationAdresses[j];
                        var curBuilding = buildingDictionary[j];
                        var curEmployee = employeeDictionary[i];

                        var projectionData = new CreateProjectionDto
                        {
                            BuildingAddress = curBuilding.Address,
                            BuildingCity = curBuilding.City,
                            BuildingCountry = curBuilding.Country,
                            BuildingState = curBuilding.State,
                            BuildingTitle = curBuilding.Title,
                            BuildingZip = curBuilding.Zip,
                            Designation = curEmployee.Designation,
                            Email = curEmployee.Email,
                            EmployeeAddress = curEmployee.Address,
                            EmployeeCity = curEmployee.City,
                            EmployeeCountry = curEmployee.Country,
                            EmployeeState = curEmployee.State,
                            EmployeeZip = curEmployee.Zip,
                            FirstName = curEmployee.FirstName,
                            LastName = curEmployee.LastName,
                            Phone = curEmployee.Phone,
                            Distance = Convert.ToSingle(element.Distance.Value/1609.344),
                            Duration = element.Duration.Value/60,
                            BuildingLat = 0,
                            BuildingLong = 0,
                            EmployeeLat = 0,
                            EmployeeLong = 0
                        };

                        projectionsData.Add(projectionData);
                    }
                }
                reportDto.Projections = projectionsData;
                var resultData = await createReportService.Create(reportDto);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }
        
        // DELETE: api/Reports/5
        [HttpDelete("{id}")]
        public GenericResult<Boolean> DeleteReport([FromRoute] int id)
        {
            var result = new GenericResult<Boolean>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }
                var recordExists = ReportExists(id).Result;
                if(recordExists == false)
                {
                    result.Success = false;
                    throw new Exception("Report not found!");
                }
                var resultData = reportService.Delete(id);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }


        private async Task<bool> ReportExists(int id)
        {
            var resultData = await reportService.GetAsynById(id);
            bool result = resultData == null ? false : true;
            return await Task.FromResult(result);
        }
    }
}