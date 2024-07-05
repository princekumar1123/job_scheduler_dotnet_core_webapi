using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Job_Scheduler_webAPI.Data;
using Job_Scheduler_webAPI.Models;
using OfficeOpenXml;

namespace Job_Scheduler_webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CountriesController : ControllerBase
    {
        private readonly Job_Scheduler_webAPIContext _context;

        public CountriesController(Job_Scheduler_webAPIContext context)
        {
            _context = context;
        }

    
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Countries>>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Countries>> GetCountries(string id)
        {
            var countries = await _context.Countries.FindAsync(id);

            if (countries == null)
            {
                return NotFound();
            }

            return countries;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountries(string id, Countries countries)
        {
            if (id != countries.CountryID)
            {
                return BadRequest();
            }

            _context.Entry(countries).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountriesExists(id))
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

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Countries>> PostCountries(Countries countries)
        {
            _context.Countries.Add(countries);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountries", new { id = countries.CountryID }, countries);
        }


        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountries(string id)
        {
            var countries = await _context.Countries.FindAsync(id);
            if (countries == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(countries);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CountriesExists(string id)
        {
            return _context.Countries.Any(e => e.CountryID == id);
        }
    }
}
