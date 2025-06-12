namespace MisProfesApp.Models.Entities
{
    public class Alumno
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public bool EsAdmin { get; set; }

        public ICollection<AlumnoMateria> AlumnoMaterias { get; set; }
        public ICollection<Calificacion> Calificaciones { get; set; }
    }
}