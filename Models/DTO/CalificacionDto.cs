namespace MisProfesApp.Models.DTO
{
    public class CalificacionDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; }
        public double CalificacionGeneral { get; set; }
        public double CalificacionClaridad { get; set; }
        public double CalificacionConocimiento { get; set; }
        public bool EsAnonima { get; set; }
        public bool Aprobada { get; set; }

        public AlumnoSimpleDto Alumno { get; set; }
        public MateriaProfesorDto MateriaProfesor { get; set; }
    }
}
