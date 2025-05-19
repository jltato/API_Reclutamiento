using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Reclutamiento.Models;
using API_Reclutamiento.Migrations;

namespace API_Reclutamiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeguimientosController : ControllerBase
    {
        private readonly MyDbContext _context;

        public SeguimientosController(MyDbContext context)
        {
            _context = context;
        }

        // GET: api/Seguimientos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seguimiento>>> GetSeguimientos()
        {
            return await _context.Seguimientos.ToListAsync();
        }

        // GET: api/Seguimientos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Seguimiento>> GetSeguimiento(int id)
        {
            var seguimiento = await _context.Seguimientos.FindAsync(id);

            if (seguimiento == null)
            {
                return NotFound();
            }

            return seguimiento;
        }

        // PUT: api/Seguimientos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeguimiento(int id, Seguimiento seguimiento)
        {
            if (id != seguimiento.SeguimientoId)
            {
                return BadRequest();
            }

            _context.Entry(seguimiento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeguimientoExists(id))
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

        // POST: api/Seguimientos
       /// <summary>
       /// Modifica las observaciones del seguimiento y agrega un estadoSeguimiento
       /// </summary>
       /// <remarks>Si la etapa del segimiento enviado es el mismo que el actual, lo modifica si no lo crea.</remarks>
       /// <param name="seguimiento"></param>
       /// <returns>ok, Listado de Estados del Seguimiento</returns>
        [HttpPost]
        public async Task<ActionResult<Seguimiento>> PostSeguimiento(Seguimiento seguimiento)
        {
            try
            {
                var seguimientoActual = await _context.Seguimientos
                        .Include(x => x.EstadoSeguimientoActual)
                        .Where(x => x.SeguimientoId == seguimiento.SeguimientoId)
                        .FirstOrDefaultAsync();
                if (seguimientoActual == null) { return NotFound(); }

                seguimientoActual.Observaciones = seguimiento.Observaciones;
                seguimientoActual.TipoInscripcionId = seguimiento.TipoInscripcionId;
                seguimientoActual.EstadoId = seguimiento.EstadoId;

                var estadoActual = seguimientoActual.EstadoSeguimientoActual;

                if (estadoActual != null &&
                    estadoActual.EtapaSeguimientoId == seguimiento.EstadoSeguimientoActual.EtapaSeguimientoId)
                {
                    estadoActual.Asistencia = seguimiento.EstadoSeguimientoActual.Asistencia;
                    estadoActual.Apto = seguimiento.EstadoSeguimientoActual.Apto;
                    estadoActual.Notificado = seguimiento.EstadoSeguimientoActual.Notificado;
                    estadoActual.EtapaSeguimientoId = seguimiento.EstadoSeguimientoActual.EtapaSeguimientoId;
                    estadoActual.FechaTurno = seguimiento.EstadoSeguimientoActual.FechaTurno;
                    
                }
                else
                {
                    var estadoNuevo = seguimiento.EstadoSeguimientoActual;
                    estadoNuevo.SeguimientoId = seguimientoActual.SeguimientoId;


                    _context.EstadoSeguimientos.Add(estadoNuevo);
                    await _context.SaveChangesAsync();
                    seguimientoActual.EstadoSeguimientoActualId = estadoNuevo.EstadoSeguimientoId;

                }

                await _context.SaveChangesAsync();

                var nuevoSeguimiento = await _context.Seguimientos
                     .Where(e => e.SeguimientoId == seguimiento.SeguimientoId)
                     .Include(e => e.EstadosSeguimiento).ThenInclude(e => e.EtapaSeguimiento)
                     .Include(e => e.EstadoSeguimientoActual).ThenInclude(e => e.EtapaSeguimiento)
                      .FirstOrDefaultAsync();
                nuevoSeguimiento.EstadosSeguimiento = [.. nuevoSeguimiento.EstadosSeguimiento.OrderBy(e => e.FechaTurno)];

                return Ok(nuevoSeguimiento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Seguimientos/5
        /// <summary>
        /// Elimina EstadoSeguimiento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeguimiento(int id)
        {
            var Estadoseg = await _context.EstadoSeguimientos.FindAsync(id);
            if (Estadoseg == null)
            {
                return NotFound();
            }
            var seguimiento = await _context.Seguimientos
                   .Include(e => e.EstadosSeguimiento).ThenInclude(e => e.EtapaSeguimiento)
                   .Include(e => e.EstadoSeguimientoActual).ThenInclude(e => e.EtapaSeguimiento)
                   .Where(e => e.SeguimientoId == Estadoseg.SeguimientoId)
                   .FirstOrDefaultAsync();

            if (seguimiento.EstadoSeguimientoActualId == Estadoseg.EstadoSeguimientoId)
            {
                return BadRequest("No se puede eliminar el estado actual");
            }

            _context.EstadoSeguimientos.Remove(Estadoseg);
            await _context.SaveChangesAsync();

           

            seguimiento.EstadosSeguimiento = [.. seguimiento.EstadosSeguimiento.OrderBy(e => e.FechaTurno)];

            return Ok(seguimiento);
        }

        private bool SeguimientoExists(int id)
        {
            return _context.Seguimientos.Any(e => e.SeguimientoId == id);
        }
    }
}
