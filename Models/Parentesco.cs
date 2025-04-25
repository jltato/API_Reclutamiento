using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Parentesco
    {
        public int ParentescoId { get; set; }
        public required string ParentescoNombre { get; set; }


        [JsonIgnore]
        public List<Familiares>? Familiares { get; set; } = [];
    }
}
