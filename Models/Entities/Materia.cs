namespace MisProfesApp.Models.Entities
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<AlumnoMateria> AlumnoMaterias { get; set; }
        public ICollection<MateriaProfesor> MateriaProfesores { get; set; }
    }
}