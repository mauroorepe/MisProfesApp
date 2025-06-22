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
    }
}
