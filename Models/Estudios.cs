using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Estudios
    {
        public int EstudiosId { get; set; } //Primary Key
        public  required int PostulanteId { get; set; } //Foreign Key
        public required int NivelEstudiosId { get; set; } //Foreign Key
        public required string Titulo { get; set; }
        public required string InstitutoEducativo { get; set; }
        public bool EnCurso { get; set; } = false;
        public required int AnoEgreso { get; set; }


        [JsonIgnore]
        public  Postulante? Postulante { get; set; }
        [JsonIgnore]
        public NivelEstudios? NivelEstudios { get; set; }
    }
}
