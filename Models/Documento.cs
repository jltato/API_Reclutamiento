using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Documento
    {
        public int DocumentoId { get; set; }
        public required int PostulanteId { get; set; }
        public required string DocumentoNombre { get; set; }
        public required int TipoDocumentoId { get; set; }



        [JsonIgnore]
        public Postulante? Postulante { get; set; }
        [JsonIgnore]
        public TipoDocumento? TipoDocumento { get; set; }
    }
}
