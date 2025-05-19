using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class Familiares
    {
        public int FamiliaresId { get; set; }
        public required int PostulanteId { get; set; }
        public required int Dni { get; set; }
        public required string Apellido { get; set; }
        public required string Nombre { get; set; }
        public required bool Convive { get; set; } = false;
        public required int ParentescoId { get; set; }
        public bool EsEmpleado {  get; set; } = false;
        public bool Activo { get; set; } = false;


        [JsonIgnore]
        public Postulante? Postulante { get; set; }
        
        public Parentesco? Parentesco { get; set; }
    }
}
