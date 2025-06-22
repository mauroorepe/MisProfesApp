namespace MisProfesApp.Models.DTO
{
    public class ProfesorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public List<MateriaSimpleDto>? Materias { get; set; }
    }
}
