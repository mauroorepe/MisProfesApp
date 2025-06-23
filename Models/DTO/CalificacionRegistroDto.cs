namespace MisProfesApp.Models.DTO
{
    public class CalificacionRegistroDto
    {
        public int Id { get; set; }
        public int MateriaProfesorId { get; set; }
       public string Comentario { get; set; }
       public double CalificacionGeneral { get; set; }
       public double CalificacionClaridad { get; set; }
       public double CalificacionConocimiento { get; set; }
       public bool EsAnonima { get; set; }
    }
}
