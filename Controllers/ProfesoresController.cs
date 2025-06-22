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
    }
}
