using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DAL.Services.Interfaces;
using DAL.Resources;
using ApiServer.Results;
using DAL.Core.Models;
using Microsoft.Extensions.Options;
using DAL.Helpers;
using System.Linq;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingsController : ControllerBase
    {

        private readonly IGenericService<BuildingDto> buildingService;
        private readonly IGenericService<EmployeeDto> employeeService;
        private readonly IOptions<ServiceSettings> _serviceSettings;

        public BuildingsController(IOptions<ServiceSettings> serviceSettings, IGenericService<BuildingDto> buildingService, IGenericService<EmployeeDto> employeeService)
        {
            _serviceSettings = serviceSettings;
            this.buildingService = buildingService;
            this.employeeService = employeeService;
        }

        // GET: api/Buildings
        [HttpGet]
        public GenericResult<IEnumerable<BuildingDto>> GetBuilding()
        {
            var result = new GenericResult<IEnumerable<BuildingDto>>();

            try
            {
                result.Result = buildingService.GetAll();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        // GET: api/Buildings/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBuilding([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new GenericResult<BuildingDto>();

            try
            {
                result.Result = await buildingService.GetAsynById(id);
                if(result.Result == null)
                {
                    result.Success = false;
                    throw new Exception("Requested Building doesn't exist!");
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }
            return Ok(result);
        }

        [HttpGet("{id}/employees")]
        public async Task<IActionResult> GetBuildingEmployees([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = new GenericResult<GoogleDistanceMatrixApi.Response>();

            try
            {
                var Url = _serviceSettings.Value.GMUrl;
                var Key = _serviceSettings.Value.GMApiKey;
                var inputSourceAddresses = buildingService.GetAsynById(id);
                var source = inputSourceAddresses.Result;
                var inputDestinationAddresses = employeeService.GetAll();
                var inputDestinationAddressesArray = new List<string>();
                var inputSourceAddressesArray = new List<string>();
                inputSourceAddressesArray.Add(string.Format("{0}, {1}, {2}, {3}, {4}", source.Address, source.City, source.State, source.Country, source.Zip));

                foreach (var destination in inputDestinationAddresses)
                {
                    inputDestinationAddressesArray.Add(string.Format("{0}, {1}, {2}, {3}, {4}", destination.Address, destination.City, destination.State, destination.Country, destination.Zip));
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
                        element.OriginAddress = originAdresses[i];
                        element.DestinationAddress = destinationAdresses[j];
                    }
                }
                response.Rows = rows.ToArray();
                result.Result = response;
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

        // PUT: api/Buildings/5
        [HttpPut("{id}")]
        public async Task<GenericResult<BuildingDto>> PutBuilding([FromRoute] int id, [FromBody] BuildingDto buildingDto)
        {
            var result = new GenericResult<BuildingDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }

                if (id != buildingDto.Id)
                {
                    result.Success = false;
                    throw new Exception("Invalid Request!");
                }
                var resultData = await buildingService.UpdateAsyn(buildingDto, id);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        // POST: api/Buildings
        [HttpPost]
        public async Task<GenericResult<BuildingDto>> PostBuilding([FromBody] BuildingDto buildingDto)
        {
            var result = new GenericResult<BuildingDto>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }
                var resultData = await buildingService.Create(buildingDto);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }
        
        // DELETE: api/Buildings/5
        [HttpDelete("{id}")]
        public GenericResult<Boolean> DeleteBuilding([FromRoute] int id)
        {
            var result = new GenericResult<Boolean>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Success = false;
                    throw new Exception("Please complete the required fields!");
                }
                var recordExists = BuildingExists(id).Result;
                if (recordExists == false)
                {
                    result.Success = false;
                    throw new Exception("Building not found!");
                }
                var resultData = buildingService.Delete(id);
                result.Result = resultData;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors = new string[] { ex.Message };
            }

            return result;
        }

        private async Task<bool> BuildingExists(int id)
        {
            var resultData = await buildingService.GetAsynById(id);
            bool result = resultData == null ? false : true;
            return await Task.FromResult(result);
        }

    }
}