using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DAL.Core.Models;
using DAL.Helpers;
using DAL.Resources;
using DAL.Services.Interfaces;
using Geocoding;
using Geocoding.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IOptions<ServiceSettings> _serviceSettings;
        private readonly IGenericService<BuildingDto> buildingService;
        private readonly IGenericService<EmployeeDto> employeeService;

        public ValuesController(IOptions<ServiceSettings> serviceSettings, IGenericService<BuildingDto> buildingService, IGenericService<EmployeeDto> employeeService)
        {
            _serviceSettings = serviceSettings;
            this.buildingService = buildingService;
            this.employeeService = employeeService;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value3", "value5" };
        }
        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }


        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] List<String> addresses)
        {
            var mapData = new List<MapData>();
            foreach (var address in addresses)
            {
                var Url = _serviceSettings.Value.GMUrl;
                var Key = _serviceSettings.Value.GMApiKey;

                IGeocoder geocoder = new GoogleGeocoder() { ApiKey =  Key};
                var point =  await geocoder.GeocodeAsync(address);
                //foreach (Address adrs in addresses)
                //{
                //    Response.Write("address:" + adrs.Coordinates);
                //}
                //GoogleDistanceMatrixApi api = new GoogleDistanceMatrixApi(Url, Key, new String[] { }, new String[] { });
                //var point = await api.GetLongitudeAndLatitude(address, "false");
                mapData.Add(new MapData
                {
                    Latitude = point.First().Coordinates.Latitude,
                    Longitude = point.First().Coordinates.Longitude
                });
            }
            return Ok(mapData);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
