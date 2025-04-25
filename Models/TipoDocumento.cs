

using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class TipoDocumento
    {
        public int TipoDocumentoId { get; set; }
        public required string TipoDocumentoNombre { get; set; }

        [JsonIgnore]
        public List<Documento>? Documentos { get; set; } = [];   
    }
}
