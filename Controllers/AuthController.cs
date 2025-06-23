using Azure;
using Microsoft.AspNetCore.Mvc;
using MisProfesApp.Services.Models;
using MisProfesApp.Services;
using MisProfesApp.Models.DTO;
using MisProfesApp.Models;
using Microsoft.EntityFrameworkCore;
using MisProfesApp.Models.Entities;

namespace MisProfesApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService AuthService;
        private readonly MisProfesContext _context;

        public AuthController(IAuthService authService, MisProfesContext context)
        {
            AuthService = authService;
            _context = context;
        }

        [HttpPost("signUp")]
        public async Task<ActionResult<Alumno>> SignUpAlumno([FromBody]AlumnoRegistroDto alumnoDto)
        {
            var nuevoAlumno = new Alumno
            {
                Id=alumnoDto.Id,
                Nombre = alumnoDto.Nombre,
                Apellido = alumnoDto.Apellido,
                Email = alumnoDto.Email,
                EsAdmin = alumnoDto.EsAdmin
            };

            _context.Alumnos.Add(nuevoAlumno);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(AlumnosController.GetAlumnos), new { id = nuevoAlumno.Id }, nuevoAlumno);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> AuthenticateAlumno([FromBody] AlumnoSimpleDto alumno)
        {
            if (string.IsNullOrEmpty(alumno.Nombre) || string.IsNullOrEmpty(alumno.Apellido))
                return BadRequest("Nombre y apellido son obligatorios.");

            var alumnoEncontrado = await _context.Alumnos
                .FirstOrDefaultAsync(a =>
                    a.Nombre.ToLower() == alumno.Nombre.ToLower() &&
                    a.Apellido.ToLower() == alumno.Apellido.ToLower());

            if (alumnoEncontrado == null)
                return NotFound("Alumno no encontrado.");

            var authRequest = new AuthRequest
            {
                Id = alumnoEncontrado.Id,
                Role = "Alumno"
            };

            var authResponse = await AuthService.Login(authRequest);

            return Ok(authResponse);
        }
    }
}
