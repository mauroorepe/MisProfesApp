namespace MisProfesApp.Models.DTO
{
    public class MateriaProfesorDto
    {
        public int Id { get; set; }
        public string NombreMateria { get; set; }
        public ProfesorSimpleDto Profesor { get; set; }
    }
}
