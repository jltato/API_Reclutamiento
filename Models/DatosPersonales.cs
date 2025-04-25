using Newtonsoft.Json;

namespace API_Reclutamiento.Models
{
    public class DatosPersonales
    {
        public int DatosPersonalesId { get; set; } // Primary Key
        public required int PostulanteId { get; set; } // Foreingn Key
        public required int GeneroId { get; set; } // Foreingn Key
        public required int EstadoCivilId { get; set; } // Foreingn Key

        public int Altura { get; set; }
        public int Peso { get; set; }
        public bool TieneTatuajes { get; set; } = false;
        public string? CantidadTatuajes { get; set; }
        public string? Observaciones { get; set; }


        [JsonIgnore]
        public  Postulante? Postulante { get; set; }
        [JsonIgnore]
        public  Genero? Genero { get; set; }
        [JsonIgnore]
        public  EstadoCivil? EstadoCivil { get; set; }
        
    }
}
