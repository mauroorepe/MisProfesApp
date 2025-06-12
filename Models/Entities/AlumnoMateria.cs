namespace MisProfesApp.Models.Entities
{
    public class AlumnoMateria
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public int IdMateria { get; set; }
        public bool FueCalificada { get; set; }

        public Alumno Alumno { get; set; }
        public Materia Materia { get; set; }
    }
}