namespace MisProfesApp.Models.Entities
{
    public class MateriaProfesor
    {
        public int Id { get; set; }
        public int IdProfesor { get; set; }
        public int IdMateria { get; set; }

        public Profesor Profesor { get; set; }
        public Materia Materia { get; set; }
        public ICollection<Calificacion> Calificaciones { get; set; }
    }
}
