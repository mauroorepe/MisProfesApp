using Microsoft.EntityFrameworkCore;
using MisProfesApp.Models.Entities;

namespace MisProfesApp.Models
{
    public class MisProfesContext : DbContext
    {
        public MisProfesContext(DbContextOptions<MisProfesContext> options)
         : base(options)
        {
        }

        public virtual DbSet<Alumno> Alumnos { get; set; }
        public virtual DbSet<AlumnoMateria> AlumnosMaterias { get; set; }
        public virtual DbSet<Profesor> Profesores { get; set; }
        public virtual DbSet<Materia> Materias { get; set; }
        public virtual DbSet<MateriaProfesor> MateriasProfesores { get; set; }
        public virtual DbSet<Calificacion> Calificaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Alumno
            modelBuilder.Entity<Alumno>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Alumno>()
                .HasMany(a => a.AlumnoMaterias)
                .WithOne(am => am.Alumno)
                .HasForeignKey(am => am.IdAlumno);

            modelBuilder.Entity<Alumno>()
                .HasMany(a => a.Calificaciones)
                .WithOne(c => c.Alumno)
                .HasForeignKey(c => c.IdAlumno);

            // Profesor
            modelBuilder.Entity<Profesor>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Profesor>()
                .HasMany(p => p.MateriaProfesores)
                .WithOne(mp => mp.Profesor)
                .HasForeignKey(mp => mp.IdProfesor);

            // Materia
            modelBuilder.Entity<Materia>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Materia>()
                .HasMany(m => m.AlumnoMaterias)
                .WithOne(am => am.Materia)
                .HasForeignKey(am => am.IdMateria);

            modelBuilder.Entity<Materia>()
                .HasMany(m => m.MateriaProfesores)
                .WithOne(mp => mp.Materia)
                .HasForeignKey(mp => mp.IdMateria);

            // AlumnoMateria
            modelBuilder.Entity<AlumnoMateria>()
                .HasKey(am => am.Id); // Puedes cambiar por clave compuesta si lo prefieres

            // MateriaProfesor
            modelBuilder.Entity<MateriaProfesor>()
                .HasKey(mp => mp.Id); // También puedes usar clave compuesta

            modelBuilder.Entity<MateriaProfesor>()
                .HasMany(mp => mp.Calificaciones)
                .WithOne(c => c.MateriaProfesor)
                .HasForeignKey(c => c.IdMateriaProfesor);

            // Calificacion
            modelBuilder.Entity<Calificacion>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Calificacion>()
                .Property(c => c.Comentario)
                .HasMaxLength(250);

            modelBuilder.Entity<Alumno>().ToTable("Alumno");
            modelBuilder.Entity<AlumnoMateria>().ToTable("AlumnoMateria");
            modelBuilder.Entity<Profesor>().ToTable("Profesor");
            modelBuilder.Entity<Materia>().ToTable("Materia");
            modelBuilder.Entity<MateriaProfesor>().ToTable("MateriaProfesor");
            modelBuilder.Entity<Calificacion>().ToTable("Calificacion");
        }
    }
}
