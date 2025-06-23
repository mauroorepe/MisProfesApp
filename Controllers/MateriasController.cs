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

        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaDto>> GetMateriaById(int id)
        {
            var materia = await _context.Materias
                .Include(m => m.AlumnoMaterias)
                    .ThenInclude(am => am.Alumno)
                .Include(m => m.MateriaProfesores)
                    .ThenInclude(mp => mp.Profesor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (materia == null)
                return NotFound();

            var result = new MateriaDto
            {
                Id = materia.Id,
                Nombre = materia.Nombre,
                Profesores = materia.MateriaProfesores?
                    .Select(mp => new ProfesorSimpleDto
                    {
                        Id = mp.Profesor.Id,
                        Nombre = mp.Profesor.Nombre,
                        Apellido = mp.Profesor.Apellido
                    }).ToList() ?? new List<ProfesorSimpleDto>(),
                Alumnos = materia.AlumnoMaterias?
                    .Select(am => new AlumnoSimpleDto
                    {
                        Id = am.Alumno.Id,
                        Nombre = am.Alumno.Nombre,
                        Apellido = am.Alumno.Apellido
                    }).ToList() ?? new List<AlumnoSimpleDto>()
            };

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Materia>> CrearMateria([FromBody] MateriaSimpleDto nuevaMateriaDto)
        {
            var nuevaMateria = new Materia
            {
                Id = nuevaMateriaDto.Id,
                Nombre = nuevaMateriaDto.Nombre
            };

            _context.Materias.Add(nuevaMateria);
            await _context.SaveChangesAsync();

            return Ok(nuevaMateria);
            //return CreatedAtAction(nameof(GetMateriaById), new { id = nuevaMateria.Id }, nuevaMateria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarMateria(int id, [FromBody] MateriaSimpleDto materiaEditada)
        {
            if (id != materiaEditada.Id)
                return BadRequest("El ID de la URL no coincide con el del cuerpo.");

            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
                return NotFound();

            materia.Nombre = materiaEditada.Nombre;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMateria(int id)
        {
            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
                return NotFound();

            _context.Materias.Remove(materia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/asignar-materia")]
        public async Task<IActionResult> AsignarMateriaAAlumno(int id, [FromBody] AsignarAlumnoDto dto)
        {
            if (id != dto.AlumnoId)
                return BadRequest("El ID del alumno en la URL no coincide con el del cuerpo de la solicitud.");

            var alumno = await _context.Alumnos.FindAsync(dto.AlumnoId);
            var materia = await _context.Materias.FindAsync(dto.MateriaId);

            if (alumno == null || materia == null)
                return NotFound("Alumno o materia no encontrados.");

            var yaAsignado = await _context.AlumnosMaterias
                .AnyAsync(am => am.Alumno.Id == dto.AlumnoId && am.Materia.Id == dto.MateriaId);

            if (yaAsignado)
                return Conflict("Esta materia ya está asignada a este alumno.");

            var alumnoMateria = new AlumnoMateria
            {
                Id = dto.Id,
                IdAlumno = dto.AlumnoId,
                IdMateria = dto.MateriaId
            };

            _context.AlumnosMaterias.Add(alumnoMateria);
            await _context.SaveChangesAsync();

            return Ok("Materia asignada al alumno correctamente.");
        }
    }
}
