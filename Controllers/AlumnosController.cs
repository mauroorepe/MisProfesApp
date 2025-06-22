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
    }
}
