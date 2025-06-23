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
    public class ProfesoresController : ControllerBase
    {
        private readonly MisProfesContext _context;

        public ProfesoresController(MisProfesContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfesorDto>>> GetProfesores()
        {
            var profesores = await _context.Profesores
                .Include(p => p.MateriaProfesores)
                    .ThenInclude(mp => mp.Materia)
                .ToListAsync();

            var result = profesores.Select(p => new ProfesorDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Materias = p.MateriaProfesores?
                    .Select(mp => new MateriaSimpleDto
                    {
                        Id = mp.Materia.Id,
                        Nombre = mp.Materia.Nombre
                    }).ToList()
            });

            return Ok(result);
        }



        [HttpPost("nuevo")]
        public async Task<ActionResult<Alumno>> NuevoProfesor([FromBody] ProfesorSimpleDto profesorDto)
        {
            var nuevoProfesor = new Profesor
            {
                Id = profesorDto.Id,
                Nombre = profesorDto.Nombre,
                Apellido = profesorDto.Apellido
            };

            _context.Profesores.Add(nuevoProfesor);
            await _context.SaveChangesAsync();

            return Ok();
            //return CreatedAtAction(nameof(GetProfesores), new { id = nuevoProfesor.Id }, nuevoProfesor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ModificarProfesor(int id, [FromBody] ProfesorSimpleDto profesorEditado)
        {
            if (id != profesorEditado.Id)
                return BadRequest("ID en URL y en el cuerpo no coinciden");

            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor == null)
                return NotFound();

            // Mapear valores
            profesor.Nombre = profesorEditado.Nombre;
            profesor.Apellido = profesorEditado.Apellido;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Profesores.Any(a => a.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProfesor(int id)
        {
            var profesor = await _context.Profesores.FindAsync(id);
            if (profesor == null)
                return NotFound();

            _context.Profesores.Remove(profesor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/asignarMateria")]
        public async Task<IActionResult> AsignarMateriaAProfesor(int id, [FromBody] AsignarMateriaDto dto)
        {
            if (id != dto.ProfesorId)
                return BadRequest("El ID del profesor en la URL no coincide con el del cuerpo de la solicitud.");

            var profesor = await _context.Profesores.FindAsync(dto.ProfesorId);
            var materia = await _context.Materias.FindAsync(dto.MateriaId);

            if (profesor == null || materia == null)
                return NotFound("Profesor o materia no encontrados.");

            var yaAsignado = await _context.MateriasProfesores
                .AnyAsync(mp => mp.Profesor.Id == dto.ProfesorId && mp.Materia.Id == dto.MateriaId);

            if (yaAsignado)
                return Conflict("Esta materia ya está asignada a este profesor.");

            var materiaProfesor = new MateriaProfesor
            {
                Id=dto.Id,
                IdProfesor = dto.ProfesorId,
                IdMateria = dto.MateriaId
            };

            _context.MateriasProfesores.Add(materiaProfesor);
            await _context.SaveChangesAsync();

            return Ok("Materia asignada al profesor correctamente.");
        }


    }
}
