using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MisProfesApp.Models;
using MisProfesApp.Models.DTO;
using MisProfesApp.Models.Entities;
using System;
using System.Security.Claims;

namespace MisProfesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionesController : ControllerBase
    {
        private readonly MisProfesContext _context;

        public CalificacionesController(MisProfesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCalificaciones()
        {
            var calificaciones = await _context.Calificaciones
                .Include(c => c.Alumno)
                .Include(c => c.MateriaProfesor)
                    .ThenInclude(mp => mp.Profesor)
                .Include(c => c.MateriaProfesor)
                    .ThenInclude(mp => mp.Materia)
                .Select(c => new CalificacionDto
                {
                    Id = c.Id,
                    Fecha = c.Fecha,
                    Comentario = c.Comentario,
                    CalificacionGeneral = c.CalificacionGeneral,
                    CalificacionClaridad = c.CalificacionClaridad,
                    CalificacionConocimiento = c.CalificacionConocimiento,
                    EsAnonima = c.EsAnonima,
                    Aprobada = c.Aprobada,
                    Alumno = new AlumnoSimpleDto
                    {
                        Id = c.Alumno.Id,
                        Nombre = c.Alumno.Nombre,
                        Apellido = c.Alumno.Apellido
                    },
                    MateriaProfesor = new MateriaProfesorDto
                    {
                        Id = c.MateriaProfesor.Id,
                        NombreMateria = c.MateriaProfesor.Materia.Nombre,
                        Profesor = new ProfesorSimpleDto
                        {
                            Id = c.MateriaProfesor.Profesor.Id,
                            Nombre = c.MateriaProfesor.Profesor.Nombre,
                            Apellido = c.MateriaProfesor.Profesor.Apellido
                        }
                    }
                }).ToListAsync();

            return Ok(calificaciones);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegistrarCalificacion([FromBody] CalificacionRegistroDto dto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int alumnoId = int.Parse(userIdClaim.Value);

            var materiaProfesor = await _context.MateriasProfesores
                .Include(mp => mp.Materia)
                .Include(mp => mp.Profesor)
                .FirstOrDefaultAsync(mp => mp.Id == dto.MateriaProfesorId);

            if (materiaProfesor == null)
                return NotFound("MateriaProfesor no encontrado.");

            var calificacion = new Calificacion
            {
                Id=dto.Id,
                IdAlumno = alumnoId,
                IdMateriaProfesor = dto.MateriaProfesorId,
                Fecha = DateTime.UtcNow,
                Comentario = dto.Comentario,
                CalificacionGeneral = dto.CalificacionGeneral,
                CalificacionClaridad = dto.CalificacionClaridad,
                CalificacionConocimiento = dto.CalificacionConocimiento,
                EsAnonima = dto.EsAnonima,
                Aprobada = false
            };

            _context.Calificaciones.Add(calificacion);
            await _context.SaveChangesAsync();

            return Ok();
            //return CreatedAtAction(nameof(GetCalificacionById), new { id = calificacion.Id }, calificacion);
        }

        [HttpPut("{id}/aprobar")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AprobarCalificacion(int id)
        {
            var calificacion = await _context.Calificaciones.FindAsync(id);
            if (calificacion == null)
                return NotFound();

            calificacion.Aprobada = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCalificacion(int id)
        {
            var calificacion = await _context.Calificaciones.FindAsync(id);
            if (calificacion == null)
            {
                return NotFound();
            }

            _context.Calificaciones.Remove(calificacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
