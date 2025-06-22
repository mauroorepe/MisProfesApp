using Azure;
using Microsoft.AspNetCore.Mvc;
using MisProfesApp.Services.Models;
using MisProfesApp.Services;
using MisProfesApp.Models.DTO;
using MisProfesApp.Models;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> AuthenticatePatient([FromBody] AlumnoSimpleDto alumno)
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
