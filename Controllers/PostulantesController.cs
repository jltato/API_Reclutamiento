using API_Reclutamiento.Models;
using API_Reclutamiento.Models.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ClosedXML;
using DocumentFormat.OpenXml.Spreadsheet;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            var EtapaSeguimiento = await _context.EtapaSeguimientos.ToListAsync();
            var estados = await _context.Estados.ToListAsync();
            var tiposInscripcion = await _context.TipoInscripcions.ToListAsync();
            
            return Ok(new
            {
                Parentescos,
                Localidades,
                Generos,
                EstadosCiviles,
                NivelEstudios,
                Nacionalidades,
                Sexos,
                Establecimientos,
                EtapaSeguimiento,
                estados,
                tiposInscripcion
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
        /// <summary>
        /// Es un endpoint para el DataTables.
        /// </summary>
        /// <remarks>Espera un JSON con los filtros en el body (formato DataTables).</remarks>
        [HttpPost("postulantesList")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                                p.Seguimiento.TipoInscripcionId == tipoPostulanteId &&
                                p.EliminadoLogico == false
                                )
                        .Select(p => new ItemPostulante
                        {
                            Id = p.PostulanteId,
                            Apellido = p.Apellido,
                            Nombre = p.Nombre,
                            Dni = p.Dni,
                            FechaSolicitud = p.FechaSoclicitud,
                            estabSolicitud = p.Establecimiento.EstablecimientoCiudad,
                            EstadoNombre = p.Seguimiento.EstadoSeguimientoActual.EtapaSeguimiento.NombreEtapa,
                            EstadoFecha = p.Seguimiento.EstadoSeguimientoActual.FechaTurno,
                            Sexo = p.Sexo.SexoName
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
                        (i.FechaSolicitud.ToString().Contains(searchValue)) ||
                        (i.Nombre != null && i.Nombre.ToLower().Contains(searchValue)) ||
                        (i.Apellido != null && i.Apellido.ToLower().Contains(searchValue)) ||
                        (i.estabSolicitud != null && i.estabSolicitud.ToLower().Contains(searchValue)) ||
                        (i.EstadoNombre != null && i.EstadoNombre.ToLower().Contains(searchValue)) ||
                        (i.EstadoNombre != null && i.Sexo.ToLower().Contains(searchValue))||
                        (i.EstadoNombre != null && i.EstadoFecha.ToString().Contains(searchValue))
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
                            ? allData.OrderBy(i => i.Sexo).ToList()
                            : allData.OrderByDescending(i => i.Sexo).ToList();
                        break;
                    case "5":
                        allData = orderDirection == "OrderBy"
                            ? allData.OrderBy(i => i.Dni).ToList()
                            : allData.OrderByDescending(i => i.Dni).ToList();
                        break;
                    case "6":
                        allData = orderDirection == "OrderBy"
                            ? allData.OrderBy(i => i.estabSolicitud).ToList()
                            : allData.OrderByDescending(i => i.estabSolicitud).ToList();
                        break;
                    case "7":
                        allData = orderDirection =="OrderBy"
                            ? allData.OrderBy(i => i.EstadoNombre).ToList()
                            : allData.OrderByDescending(i => i.EstadoNombre).ToList();
                        break;
                    case "8":
                        allData = orderDirection == "OrderBy"
                            ? allData.OrderBy(i => i.EstadoFecha).ToList()
                            : allData.OrderByDescending(i => i.EstadoFecha).ToList();
                        break;
                    default:
                        break;
                }

                if (length == -1)
                {
                    length = totalRecordsFiltered;
                }

                var filteredIds = allData.Select(a => a.Id).ToArray();

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
                    data = paginatedResult,
                    filteredIds = filteredIds
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
            
            if (postulante.Seguimiento != null)
            {
                postulante.Seguimiento.EstadosSeguimiento = [.. postulante.Seguimiento.EstadosSeguimiento.OrderBy(e => e.FechaTurno)];
            }
            return postulante;
        }

        /// <summary>
        /// Devuelve el listado de familiares verificados
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// GET: api/Familiares/5
        [HttpGet("Familiares/{Id}")]
        public async Task<IActionResult> Familiares(int Id)
            {

            var Familiares = await _context.Familiares
                .Where(f => f.PostulanteId == Id)
                .Select(f => new FamiliaresDTO
                {
                    Dni = f.Dni,
                    Apellido = f.Apellido,
                    Nombre = f.Nombre,
                    EsEmpleado = f.EsEmpleado,
                    Activo = f.Activo,
                    ParentescoId = f.ParentescoId,
                    Convive = f.Convive,
                })
                .ToListAsync();

            foreach (var f in Familiares){
               VerificacionFamiliarDTO Verificacion = VerificarFamiliar(f.Dni); 
                
            }



            foreach (var f in Familiares)
            {
                
            }


            return Ok();

            }

        private VerificacionFamiliarDTO VerificarFamiliar(int dni)
        {

            return new VerificacionFamiliarDTO()
            { 
                ExInterno = true, Visitante = true 
            };


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
        /// <summary>
        /// Eliminado Logico del posutlante
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePostulante(int id)
        {
            var postulante = await _context.Postulantes
                .Include(p =>p.Seguimiento)
                .Where(p => p.PostulanteId == id)
                .FirstOrDefaultAsync();
            if (postulante == null)
            {
                return NotFound();
            }

            postulante.EliminadoLogico = true;
            postulante.Seguimiento.Modify_At = DateTime.Now;
            //Falta Implementar
            postulante.Seguimiento.Modify_By = "Usaurio Anonimo"; 

            _context.Postulantes.Update(postulante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostulanteExists(int id)
        {
            return _context.Postulantes.Any(e => e.PostulanteId == id);
        }

        /// <summary>
        /// Modificacion de postulante
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns></returns>
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

        [HttpPost("Excel")]
        public async Task<IActionResult> GetExcel([FromBody] List<int> Listado)
        {
            try
            {
                var datos =  await _context.Postulantes // List<Postulante>
                        .Where(p => Listado.Contains(p.PostulanteId))
                        .Include(p => p.Sexo)
                        .Include(p => p.Establecimiento)
                        .Include(p => p.Seguimiento)
                            .ThenInclude(s => s.EstadoSeguimientoActual)
                            .ThenInclude(e => e.EtapaSeguimiento)
                        .Include(p => p.Contactos)
                        .Select (p => new PostulanteExcelDTO
                        {
                            PostulanteId = p.PostulanteId,
                            Nombre = p.Nombre,
                            Apellido = p.Apellido,
                            Dni = p.Dni,
                            Email = p.Mail,
                            Telefonos = p.Contactos[0].Telefono,
                            Localidad_Solicitud = p.Establecimiento.EstablecimientoCiudad,
                            Sexo = p.Sexo.SexoName,
                            Etapa = p.Seguimiento.EstadoSeguimientoActual.EtapaSeguimiento.NombreEtapa,
                            Fecha = p.Seguimiento.EstadoSeguimientoActual.FechaTurno.ToString("dd-MM-yyyy"),
                        })
                        .OrderBy(p => p.Apellido)
                        .ToListAsync();
                if(datos.Count == 0)
                {
                    return BadRequest("No se encontraron postulantes con los IDs proporcionados.");
                }

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Postulantes");
                worksheet.Cell(1, 1).InsertTable(datos);
                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                return File(stream.ToArray(),
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            "postulantes.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
    
    public class VerificacionFamiliarDTO
    {
        public bool ExInterno { get; set; }
        public bool Visitante { get; set; }
    }
}
