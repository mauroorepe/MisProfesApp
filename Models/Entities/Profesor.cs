namespace MisProfesApp.Models.Entities
{
    public class Profesor
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public ICollection<MateriaProfesor> MateriaProfesores { get; set; }
    }
}
