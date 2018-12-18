using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.Core.Models;
using DAL.Persistence;

namespace ApiServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectionDatasController : ControllerBase
    {
        private readonly ApiContext _context;

        public ProjectionDatasController(ApiContext context)
        {
            _context = context;
        }

        // GET: api/ProjectionDatas
        [HttpGet]
        public IEnumerable<ProjectionData> GetProjectionData()
        {
            return _context.ProjectionData;
        }

        // GET: api/ProjectionDatas/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectionData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectionData = await _context.ProjectionData.FindAsync(id);

            if (projectionData == null)
            {
                return NotFound();
            }

            return Ok(projectionData);
        }

        // PUT: api/ProjectionDatas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectionData([FromRoute] int id, [FromBody] ProjectionData projectionData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectionData.Id)
            {
                return BadRequest();
            }

            _context.Entry(projectionData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectionDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProjectionDatas
        [HttpPost]
        public async Task<IActionResult> PostProjectionData([FromBody] ProjectionData projectionData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ProjectionData.Add(projectionData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjectionData", new { id = projectionData.Id }, projectionData);
        }

        // DELETE: api/ProjectionDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectionData([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var projectionData = await _context.ProjectionData.FindAsync(id);
            if (projectionData == null)
            {
                return NotFound();
            }

            _context.ProjectionData.Remove(projectionData);
            await _context.SaveChangesAsync();

            return Ok(projectionData);
        }

        private bool ProjectionDataExists(int id)
        {
            return _context.ProjectionData.Any(e => e.Id == id);
        }
    }
}