using MisProfesApp.Models.DTO;

namespace MisProfesApp.Models.NewFolder
{
    public class MateriaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<ProfesorSimpleDto> Profesores { get; set; }
        public List<AlumnoSimpleDto> Alumnos { get; set; }
    }
}
