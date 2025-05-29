namespace API_Reclutamiento.Models.DTOs
{
    public class PostulanteExcelDTO
    {
        public int PostulanteId { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public int Dni { get; set; }
        public string Email { get; set; }
        public string Telefonos { get; set; }   
        public string Localidad_Solicitud { get; set; }
        public string Sexo { get; set; }
        public string FechaNac { get; set; }
        public string Etapa { get; set; }
        public string Fecha { get; set; }
        public bool Asistio { get; set; }
        public bool? Apto { get; set; }
        public bool Notificado { get; set; }


    }
}
