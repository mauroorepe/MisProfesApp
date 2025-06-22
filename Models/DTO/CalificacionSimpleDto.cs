namespace MisProfesApp.Models.DTO
{
    public class CalificacionSimpleDto
    {
        public int Id { get; set; }
        public int MateriaId { get; set; }
        public string MateriaNombre { get; set; }
        public string ProfesorNombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Comentario { get; set; }
        public double CalificacionGeneral { get; set; }
        public bool EsAnonima { get; set; }
    }
}
