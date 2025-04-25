using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Reclutamiento.Models;
using Microsoft.Extensions.Configuration;

namespace API_Reclutamiento.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentosController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public DocumentosController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; 
        }

        // GET: api/Documentos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Documento>>> GetDocumentos()
        {
            return await _context.Documentos.ToListAsync();
        }

        // GET: api/Documentos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Documento>> GetDocumento(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);

            if (documento == null)
            {
                return NotFound();
            }

            return documento;
        }

        // PUT: api/Documentos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocumento(int id, Documento documento)
        {
            if (id != documento.DocumentoId)
            {
                return BadRequest();
            }

            _context.Entry(documento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentoExists(id))
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

        // POST: api/Documentos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostDocumento()
        {
            if (Request.Form.Files.Count == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var file = Request.Form.Files[0];

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Obtiene el PostulanteId de los datos del formulario
            if (!int.TryParse(Request.Form["PostulanteId"], out int PostulanteId))
            {
                return BadRequest("PostulanteId is not a valid integer.");
            }

            var fileType = file.ContentType.ToString();
            int idType = 0;


            if (fileType.Contains("image"))
            {
                idType = 1;
            }

            var FileStoragePath = _configuration["FileStorage:BasePath"];
            if (!Directory.Exists(FileStoragePath) && FileStoragePath != null)
            {
                Directory.CreateDirectory(FileStoragePath);
            }
            else
            {
                return BadRequest("No se encuentra y no se puede crear la carpeta de archivos");
            }

            var fileName = $"{PostulanteId}_{DateTime.Now:yyyyMMdd_HHmmss}_{idType}";

            // Guardar el archivo en el servidor con el nombre único
            var filePath = Path.Combine(FileStoragePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            var documento = new Documento { 
                DocumentoNombre = fileName,
                TipoDocumentoId = idType,
                PostulanteId = PostulanteId
            };

            _context.Documentos.Add(documento);
            await _context.SaveChangesAsync();

            return Ok(new { message = "File uploaded successfully"});
        }

        // DELETE: api/Documentos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumento(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentoExists(int id)
        {
            return _context.Documentos.Any(e => e.DocumentoId == id);
        }
    }
}
