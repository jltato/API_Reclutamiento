using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Reclutamiento.Models;

namespace API_Reclutamiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomiciliosController : ControllerBase
    {
        private readonly MyDbContext _context;

        public DomiciliosController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Domicilios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domicilio>>> GetDomicilios()
        {
            return await _context.Domicilios
                .Include(d => d.Localidad)
                .ToListAsync();
        }

        // GET: api/Domicilios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Domicilio>> GetDomicilio(int id)
        {
            var domicilio = await _context.Domicilios.FindAsync(id);

            if (domicilio == null)
            {
                return NotFound();
            }

            return domicilio;
        }

        // PUT: api/Domicilios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDomicilio(int id, Domicilio domicilio)
        {
            if (id != domicilio.DomicilioId)
            {
                return BadRequest();
            }

            _context.Entry(domicilio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DomicilioExists(id))
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

        // POST: api/Domicilios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Domicilio>> PostDomicilio(Domicilio domicilio)
        {
            _context.Domicilios.Add(domicilio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDomicilio", new { id = domicilio.DomicilioId }, domicilio);
        }

        // DELETE: api/Domicilios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDomicilio(int id)
        {
            var domicilio = await _context.Domicilios.FindAsync(id);
            if (domicilio == null)
            {
                return NotFound();
            }

            _context.Domicilios.Remove(domicilio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DomicilioExists(int id)
        {
            return _context.Domicilios.Any(e => e.DomicilioId == id);
        }
    }
}
