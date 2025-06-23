namespace MisProfesApp.Models.DTO
{
    public class AlumnoRegistroDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public bool EsAdmin { get; set; }
    }
}
