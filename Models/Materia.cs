namespace MisProfesApp.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}


namespace MisProfesApp.Models
{
    public class AlumnoMateria
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public int IdMateria { get; set; }
        public bool FueCalificada { get; set; }
    }
}


namespace MisProfesApp.Models
{
    public class Alumno
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public bool EsAdmin { get; set; }
    }
}


namespace MisProfesApp.Models
{
    public class Profesor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}


namespace MisProfesApp.Models
{
    public class MateriaProfesor
    {
        public int Id { get; set; }
        public int IdProfesor { get; set; }
        public int IdMateria { get; set; }
    }
}


namespace MisProfesApp.Models
{
    public class Calificacion
    {
        public int Id { get; set; }
        public int IdAlumno { get; set; }
        public int IdMateriaProfesor { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; }
        public int CalificacionGeneral { get; set; }
        public int CalificacionClaridad { get; set; }
        public int CalificacionConocimiento { get; set; }
        public bool EsAnonima { get; set; }
        public bool Aprobada { get; set; }
    }
}

