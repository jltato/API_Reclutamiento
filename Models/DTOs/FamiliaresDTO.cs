namespace API_Reclutamiento.Models.DTOs
{
    public class FamiliaresDTO
    {

        public required int Dni { get; set; }
        public required string Apellido { get; set; }
        public required string Nombre { get; set; }
        public required bool Convive { get; set; } = false;
        public required string Parentesco { get; set; }
        public bool EsEmpleado { get; set; } = false;
        public bool Activo { get; set; } = false;
        public bool Visita { get; set; } = false;
        public bool ExInterno { get; set; } = false;

    }
}
