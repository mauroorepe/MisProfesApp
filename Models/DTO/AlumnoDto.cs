namespace MisProfesApp.Models.DTO
{
    public class AlumnoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public bool EsAdmin { get; set; }
        public List<MateriaSimpleDto> Materias { get; set; }
        public List<CalificacionSimpleDto> Calificaciones { get; set; }
    }
}
