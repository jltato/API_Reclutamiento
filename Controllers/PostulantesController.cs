using API_Reclutamiento.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace API_Reclutamiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostulantesController : ControllerBase
    {
        private readonly MyDbContext _context;

        public PostulantesController(MyDbContext context)
        {
            _context = context;
        }

        //GET: api/postulantes/FormData
        [HttpGet("formdata")]
        public async Task<IActionResult> GetFormData()
        {
            var Parentescos = await _context.Parentescos.ToListAsync();
            var Localidades = await _context.Localidades.OrderBy(l => l.LocalidadName).ToListAsync();
            var Generos = await _context.Generos.ToListAsync(); 
            var EstadosCiviles = await _context.EstadosCiviles.ToListAsync();
            var NivelEstudios = await _context.NivelEstudios.ToListAsync();
            var Nacionalidades = await _context.Nacionalidades.ToListAsync();
            var Establecimientos = await _context.establecimientos.ToListAsync();
            var Sexos = await _context.Sexos.ToListAsync();
            
            return Ok(new
            {
                Parentescos,
                Localidades,
                Generos,
                EstadosCiviles,
                NivelEstudios,
                Nacionalidades,
                Sexos,
                Establecimientos
            });
        }

        //POST: api/postulantes/verificar
        [HttpPost("verificar")]
        public async Task<ActionResult<string>> VerificarPostulante([FromBody] VerificarPostulanteDto dto)
        {
            try
            {
                var existeDni = await _context.Postulantes
               .Include(p => p.Seguimiento)
               .AnyAsync(p => p.Dni == dto.Dni && p.Seguimiento.TipoInscripcionId == dto.TipoInscripcionId);
                var existeEmail = await _context.Postulantes
                    .Include(p => p.Seguimiento)
                    .AnyAsync(p => p.Mail == dto.Mail && p.Seguimiento.TipoInscripcionId == dto.TipoInscripcionId);

                return Ok(new { existeDni, existeEmail });
            }
            catch(Exception ex)
            {
                return Ok(new { existeDni = false, existeEmail = false });
            }
           

            
        }

        // GET: api/Postulantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Postulante>>> GetPostulantes()
        {
            return await _context.Postulantes
                .Include( p => p.Domicilios).ThenInclude(d => d.Localidad)
                .Include(p => p.Contactos)
                .Include(p => p.Sexo)
                .Include(p => p.DatosPersonales)
                .Include(p => p.Estudios).ThenInclude(e => e.NivelEstudios)
                .Include(p => p.Familiares)
                .Include(p => p.Trabajos)
                .Include(p => p.Documentos)
                .Include(p => p.Seguimiento)
                .ToListAsync();
        }

        // GET: api/Postulantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Postulante>> GetPostulante(int id)
        {
            var postulante = await _context.Postulantes
                 .Include(p => p.Nacionalidad)
                 .Include(p => p.Establecimiento)
                 .Include(p => p.Domicilios).ThenInclude(d => d.Localidad)
                 .Include(p => p.Contactos)
                 .Include(p => p.Sexo)
                 .Include(p => p.DatosPersonales)
                 .Include(p => p.Estudios).ThenInclude(e => e.NivelEstudios)
                 .Include(p => p.Familiares).ThenInclude (f => f.Parentesco)
                 .Include(p => p.Trabajos)
                 .Include(p => p.Documentos)
                 .Include(p => p.Seguimiento).ThenInclude(s => s.TipoInscripcion)
                 .Include(p => p.Seguimiento).ThenInclude(s => s.Estado)
                 .FirstOrDefaultAsync(p => p.PostulanteId == id);

            if (postulante == null)
            {
                return NotFound();
            }
            
            return postulante;
        }

        // PUT: api/Postulantes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPostulante(int id, Postulante postulante)
        {
            if (id != postulante.PostulanteId)
            {
                return BadRequest();
            }

            _context.Entry(postulante).State = EntityState.Modified;

            if (postulante.Domicilios != null)
            {
                foreach (var dom in postulante.Domicilios)
                {
                    _context.Entry(dom).State = EntityState.Modified;
                }                
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostulanteExists(id))
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

        // POST: api/Postulantes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Postulante>> PostPostulante(Postulante postulante)
        {
            try
            {
                _context.Postulantes.Add(postulante);
                await _context.SaveChangesAsync();


                return CreatedAtAction("GetPostulante", new { id = postulante.PostulanteId }, postulante);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
          
        }

        // DELETE: api/Postulantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostulante(int id)
        {
            var postulante = await _context.Postulantes.FindAsync(id);
            if (postulante == null)
            {
                return NotFound();
            }

            _context.Postulantes.Remove(postulante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostulanteExists(int id)
        {
            return _context.Postulantes.Any(e => e.PostulanteId == id);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchPostulante(int id, JsonPatchDocument<Postulante> patchDoc)
        {
            var postulante = await _context.Postulantes.Include(p => p.Domicilios).FirstOrDefaultAsync(p => p.PostulanteId == id);

            if (postulante == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(postulante, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostulanteExists(id))
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
    }

    public class VerificarPostulanteDto
    {
        [JsonPropertyName("dni")]
        public int Dni { get; set; }

        [JsonPropertyName("mail")]
        public string Mail { get; set; } = string.Empty;

        [JsonPropertyName("tipoInscripcionId")]
        public int TipoInscripcionId { get; set; }
    }
}
