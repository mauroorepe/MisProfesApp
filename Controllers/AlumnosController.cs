using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MisProfesApp.Models;
using MisProfesApp.Models.DTO;
using MisProfesApp.Models.Entities;
using System;

namespace MisProfesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly MisProfesContext _context;

        public AlumnosController(MisProfesContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoDto>>> GetAlumnos()
        {
            var alumnos = await _context.Alumnos
                .Include(a => a.AlumnoMaterias)
                    .ThenInclude(am => am.Materia)
                .Include(a => a.Calificaciones)
                    .ThenInclude(c => c.MateriaProfesor)
                        .ThenInclude(mp => mp.Materia)
                .Include(a => a.Calificaciones)
                    .ThenInclude(c => c.MateriaProfesor)
                        .ThenInclude(mp => mp.Profesor)
                .ToListAsync();

            var result = alumnos.Select(a => new AlumnoDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Apellido = a.Apellido,
                Email = a.Email,
                EsAdmin = a.EsAdmin,
                Materias = a.AlumnoMaterias?
                    .Select(am => new MateriaSimpleDto
                    {
                        Id = am.Materia.Id,
                        Nombre = am.Materia.Nombre
                    }).ToList(),
                Calificaciones = a.Calificaciones?
                    .Select(c => new CalificacionSimpleDto
                    {
                        Id = c.Id,
                        MateriaId = c.MateriaProfesor.Materia.Id,
                        MateriaNombre = c.MateriaProfesor.Materia.Nombre,
                        ProfesorNombre = $"{c.MateriaProfesor?.Profesor?.Nombre ?? "N/A"} {c.MateriaProfesor?.Profesor?.Apellido ?? ""}",
                        Fecha = c.Fecha,
                        Comentario = c.Comentario,
                        CalificacionGeneral = c.CalificacionGeneral,
                        EsAnonima = c.EsAnonima
                    }).ToList()
            });

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlumnoDto>> GetAlumno(int id)
        {
            var alumno = await _context.Alumnos
                .Include(a => a.AlumnoMaterias)
                    .ThenInclude(am => am.Materia)
                .Include(a => a.Calificaciones)
                    .ThenInclude(c => c.MateriaProfesor)
                        .ThenInclude(mp => mp.Materia)
                .Include(a => a.Calificaciones)
                    .ThenInclude(c => c.MateriaProfesor)
                        .ThenInclude(mp => mp.Profesor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (alumno == null)
            {
                return NotFound();
            }

            var result = new AlumnoDto
            {
                Id = alumno.Id,
                Nombre = alumno.Nombre,
                Apellido = alumno.Apellido,
                Email = alumno.Email,
                EsAdmin = alumno.EsAdmin,
                Materias = alumno.AlumnoMaterias?
                    .Select(am => new MateriaSimpleDto
                    {
                        Id = am.Materia.Id,
                        Nombre = am.Materia.Nombre
                    }).ToList(),
                Calificaciones = alumno.Calificaciones?
                    .Select(c => new CalificacionSimpleDto
                    {
                        Id = c.Id,
                        MateriaId = c.MateriaProfesor.Materia.Id,
                        MateriaNombre = c.MateriaProfesor.Materia.Nombre,
                        ProfesorNombre = $"{c.MateriaProfesor.Profesor.Nombre} {c.MateriaProfesor.Profesor.Apellido}",
                        Fecha = c.Fecha,
                        Comentario = c.Comentario,
                        CalificacionGeneral = c.CalificacionGeneral,
                        EsAnonima = c.EsAnonima
                    }).ToList()
            };

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarAlumno(int id, [FromBody] AlumnoRegistroDto alumnoEditado)
        {
            if (id != alumnoEditado.Id)
                return BadRequest("ID en URL y en el cuerpo no coinciden");

            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
                return NotFound();

            // Mapear valores
            alumno.Nombre = alumnoEditado.Nombre;
            alumno.Apellido = alumnoEditado.Apellido;
            alumno.Email = alumnoEditado.Email;
            alumno.EsAdmin = alumnoEditado.EsAdmin;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Alumnos.Any(a => a.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarAlumno(int id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
                return NotFound();

            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
