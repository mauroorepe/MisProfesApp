using MisProfesApp.Services.Models;

namespace MisProfesApp.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(AuthRequest req);
    }
}
