using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DAL.Core.Models;
using DAL.Helpers;
using DAL.Resources;
using DAL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace YdsHouston.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IGenericService<BuildingDto> buildingService;
        private readonly IOptions<S3ServiceSettings> _serviceSettings;

        public UploadController(IOptions<S3ServiceSettings> serviceSettings, IGenericService<BuildingDto> buildingService, IHostingEnvironment hostingEnvironment)
        {
            _serviceSettings = serviceSettings;
            this.buildingService = buildingService;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET api/upload
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value3" };
        }

        // POST api/upload
        [HttpPost("{id}/buildings"), DisableRequestSizeLimit]
        public async Task<ActionResult> UploadProjectBuildings()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "general";

                string webRootPath = _hostingEnvironment.WebRootPath;
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folderName}");
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    List<string> records = new List<string>();
                    string csvData = System.IO.File.ReadAllText(fullPath);

                    //Execute a loop over the rows.
                    int count = 1;
                    foreach (string row in csvData.Split("\r\n"))
                    {
                        if (!string.IsNullOrEmpty(row) && count != 1)
                        {
                            var str = row.Split(',');
                            await buildingService.Create(new BuildingDto
                            {
                                ProjectId = Convert.ToInt32(str[0]),
                                Title = str[1],
                                Class = str[2],
                                Submarket = str[3],
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
                }
                return Json("Upload Successful.");
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }

        [HttpPost("{id}/employees"), DisableRequestSizeLimit]
        public async Task<ActionResult> UploadProjectEmployees()
        {
            try
            {
                var file = Request.Form.Files[0];
                string folderName = "general";

                string webRootPath = _hostingEnvironment.WebRootPath;
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{folderName}");
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    List<string> records = new List<string>();
                    string csvData = System.IO.File.ReadAllText(fullPath);

                    //Execute a loop over the rows.
                    int count = 1;
                    foreach (string row in csvData.Split("\r\n"))
                    {
                        if (!string.IsNullOrEmpty(row) && count != 1)
                        {
                            var str = row.Split(',');
                            await buildingService.Create(new BuildingDto
                            {
                                ProjectId = Convert.ToInt32(str[0]),
                                Title = str[1],
                                Class = str[2],
                                Submarket = str[3],
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
                }
                return Json("Upload Successful.");
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        public async Task<ActionResult> Upload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var filePath = Path.GetTempFileName();
                string fileName = string.Format(@"{0}.csv", Guid.NewGuid());

                if (file.Length > 0)
                {
                    string fullPath = _hostingEnvironment.WebRootPath + $@"\general" + $@"\{fileName}";

                    using (var fs = System.IO.File.Create(fullPath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    var uploadService = new UploadService(_serviceSettings.Value.AccessKey, _serviceSettings.Value.SecretKey, fileName);
                    var result = await uploadService.UploadFileAsync(fullPath);
                }

                var r = new UploadResult() { Success = true, Msg = "Upload Successful", FileName = fileName };
                return Ok(r);
            }
            catch (System.Exception ex)
            {
                return Json("Upload Failed: " + ex.Message);
            }
        }
    }
}