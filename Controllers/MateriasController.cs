using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MisProfesApp.Models;
using MisProfesApp.Models.DTO;
using MisProfesApp.Models.Entities;
using MisProfesApp.Models.NewFolder;
using System;

namespace MisProfesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MateriasController : ControllerBase
    {
        private readonly MisProfesContext _context;

        public MateriasController(MisProfesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaDto>>> GetMaterias()
        {
            var materias = await _context.Materias
                .Include(m => m.MateriaProfesores)
                    .ThenInclude(mp => mp.Profesor)
                .Include(m => m.AlumnoMaterias)
                    .ThenInclude(am => am.Alumno)
                .ToListAsync();

            var result = materias.Select(m => new MateriaDto
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Profesores = m.MateriaProfesores?
                    .Select(mp => new ProfesorSimpleDto
                    {
                        Id = mp.Profesor.Id,
                        Nombre = mp.Profesor.Nombre,
                        Apellido = mp.Profesor.Apellido
                    }).ToList() ?? new List<ProfesorSimpleDto>(),
                Alumnos = m.AlumnoMaterias?
                    .Select(am => new AlumnoSimpleDto
                    {
                        Id = am.Alumno.Id,
                        Nombre = am.Alumno.Nombre,
                        Apellido = am.Alumno.Apellido
                    }).ToList() ?? new List<AlumnoSimpleDto>()
            });

            return Ok(result);
        }
    }
}
