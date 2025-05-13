using API_Reclutamiento.Models;
using API_Reclutamiento.Models.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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
            var Establecimientos = await _context.Establecimientos
                 .Where(e => e.EstablecimientoId != 3 && e.EstablecimientoId != 4)
                 .ToListAsync();
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
                var fechaLimite = (DateTime.Now).AddMonths(-6);
                var existeDni = await _context.Postulantes
                   .Include(p => p.Seguimiento)
                   .AnyAsync(p => p.Dni == dto.Dni && p.Seguimiento.TipoInscripcionId == dto.TipoInscripcionId && p.FechaSoclicitud > fechaLimite );
                var existeEmail = await _context.Postulantes
                    .Include(p => p.Seguimiento)
                    .AnyAsync(p => p.Mail == dto.Mail && p.Seguimiento.TipoInscripcionId == dto.TipoInscripcionId && p.FechaSoclicitud > fechaLimite);

                return Ok(new { existeDni, existeEmail });
            }
            catch(Exception ex)
            {
                return Ok(new { existeDni = false, existeEmail = false });
            }
           
        }

        // Post: api/PostulantesList
        [HttpPost("postulantesList")]
        public async Task<IActionResult> PostulantesList()
        {
            try
            {
                // Obtener parámetros de la solicitud
                var searchValue = Request.Form["search[value]"].FirstOrDefault() ?? "";
                var start = int.Parse(Request.Form["start"].FirstOrDefault() ?? "0");
                var length = int.Parse(Request.Form["length"].FirstOrDefault() ?? "0");
                var orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault() ?? "";
                var orderDirection = Request.Form["order[0][dir]"] == "asc" ? "OrderBy" : "OrderByDescending";
                var draw = int.Parse(Request.Form["draw"].FirstOrDefault() ?? "0");

                // Obtener parametros personalizados
                var tipoPostulanteId = int.Parse(Request.Form["tipoPostulante"].FirstOrDefault() ?? "0");
                var estadoId = int.Parse(Request.Form["estado"].FirstOrDefault() ?? "0");

                var allData = await _context.Postulantes
                        .Include(p => p.Establecimiento)
                        .Include(p => p.Seguimiento)
                            .ThenInclude(s => s.Estado)
                        .Include(p => p.Seguimiento)
                            .ThenInclude(s => s.EstadosSeguimiento)
                            .ThenInclude(es => es.EtapaSeguimiento)
                        .Include(p => p.Sexo)
                        .Where(p =>
                                (estadoId == 0 || p.Seguimiento.EstadoId == estadoId) &&
                                p.Seguimiento.TipoInscripcionId == tipoPostulanteId
                                )
                        .Select(p => new ItemPostulante
                        {
                            Id = p.PostulanteId,
                            Apellido = p.Apellido,
                            Nombre = p.Nombre,
                            Dni = p.Dni,
                            FechaSolicitud = p.FechaSoclicitud.ToString("dd-MM-yyyy"),
                            estabSolicitud = p.Establecimiento.EstablecimientoCiudad,
                            EstadoNombre = p.Seguimiento.EstadoSeguimientoActual.EtapaSeguimiento.NombreEtapa ?? "",
                            EstadoFecha = p.Seguimiento.EstadoSeguimientoActual.FechaTurno.ToString("dd-MM-yy HH:mm"),
                            Sexo = p.Sexo.SexoName,

                        })
                        .ToListAsync();

                // Contar registros totales (sin paginación)
                var totalRecords = allData.Count();

                // Aplicar filtrado en memoria
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchValue = searchValue.ToLower(); // Convertir a minúsculas para una búsqueda sin distinción de mayúsculas/minúsculas

                    allData = allData.Where(i =>
                        i.Id.ToString().Contains(searchValue) ||
                        (i.Dni != 0 && i.Dni.ToString().Contains(searchValue)) ||
                        (i.FechaSolicitud.Contains(searchValue)) ||
                        (i.Nombre != null && i.Nombre.ToLower().Contains(searchValue)) ||
                        (i.Apellido != null && i.Apellido.ToLower().Contains(searchValue)) ||
                        (i.estabSolicitud != null && i.estabSolicitud.ToLower().Contains(searchValue)) ||
                        (i.EstadoNombre != null && i.EstadoNombre.ToLower().Contains(searchValue))
                    ).ToList();
                }

                // Contar registros filtrados
                var totalRecordsFiltered = allData.Count();

                //// Aplicar ordenación en memoria
                switch (orderColumnIndex)
                {
                    case "0":
                        allData = orderDirection == "OrderBy"
                            ? allData.OrderBy(i => i.Id).ToList()
                            : allData.OrderByDescending(i => i.Id).ToList();
                        break;
                    case "1":
                        allData = orderDirection == "OrderBy"
                            ? allData.OrderBy(i => i.FechaSolicitud).ToList()
                            : allData.OrderByDescending(i => i.FechaSolicitud).ToList();
                        break;
                    case "2":
                        allData = orderDirection == "OrderBy"
                            ? allData.OrderBy(i => i.Apellido).ToList()
                            : allData.OrderByDescending(i => i.Apellido).ToList();
                        break;

                    case "3":
                        allData = orderDirection == "OrderBy"
                            ? allData.OrderBy(i => i.Nombre).ToList()
                            : allData.OrderByDescending(i => i.Nombre).ToList();
                        break;
                    case "4":
                        allData = orderDirection == "OrderBy"
                            ? allData.OrderBy(i => i.Dni).ToList()
                            : allData.OrderByDescending(i => i.Dni).ToList();
                        break;
                    case "5":
                        allData = orderDirection == "OrderBy"
                            ? allData.OrderBy(i => i.estabSolicitud).ToList()
                            : allData.OrderByDescending(i => i.estabSolicitud).ToList();
                        break;
                    case "6":
                        allData = orderDirection =="orderBy"
                            ? allData.OrderBy(i => i.EstadoNombre).ToList()
                            : allData.OrderByDescending(i => i.EstadoNombre).ToList();
                        break;

                    default:

                        break;
                }

                if (length == -1)
                {
                    length = totalRecordsFiltered;
                }

                // Paginación
                var paginatedResult = allData
                    .Skip(start)
                    .Take(length)
                    .ToList();

                var json = new JsonResult(new
                {
                    draw = draw,
                    recordsTotal = totalRecords,
                    recordsFiltered = totalRecordsFiltered,
                    data = paginatedResult
                });

                return Ok(json.Value);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
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
                 .Include(p => p.Seguimiento).ThenInclude(s=>s.EstadosSeguimiento).ThenInclude(es=>es.EtapaSeguimiento)
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
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                _context.Postulantes.Add(postulante);
                await _context.SaveChangesAsync();

                var estadoInicial = postulante.Seguimiento?.EstadosSeguimiento.FirstOrDefault();
                if (estadoInicial != null)
                {
                    postulante.Seguimiento.EstadoSeguimientoActualId = estadoInicial.EstadoSeguimientoId;
                    _context.Seguimientos.Update(postulante.Seguimiento);
                    await _context.SaveChangesAsync();
                }

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
