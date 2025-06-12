namespace MisProfesApp.Models.Entities
{
    public class Calificacion
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public int IdMateriaProfesor { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; }
        public double CalificacionGeneral { get; set; }
        public double CalificacionClaridad { get; set; }
        public double CalificacionConocimiento { get; set; }
        public bool EsAnonima { get; set; }
        public bool Aprobada { get; set; }


        public Alumno Alumno { get; set; }
        public MateriaProfesor MateriaProfesor { get; set; }
    }
}